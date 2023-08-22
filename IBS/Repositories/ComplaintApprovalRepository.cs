using IBS.Controllers;
using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Interfaces.Vendor;
using IBS.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGeneration;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Dynamic;
using System.Globalization;
using System.Net.Mail;
using System.Xml;
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace IBS.Repositories
{
    public class ComplaintApprovalRepository : IComplaintApprovalRepository
    {
        private readonly ModelContext context;
        private readonly ISendMailRepository pSendMailRepository;

        public ComplaintApprovalRepository(ModelContext context, ISendMailRepository pSendMailRepository)
        {
            this.context = context;
            this.pSendMailRepository = pSendMailRepository;
        }

        public DTResult<OnlineComplaints> GetRejComplaints(DTParameters dtParameters)
        {

            DTResult<OnlineComplaints> dTResult = new() { draw = 0 };
            IQueryable<OnlineComplaints>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "CaseNo";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                orderCriteria = "CaseNo";
                orderAscendingDirection = true;
            }

            query = from t in context.TempOnlineComplaints
                    where t.Status == null
                    orderby t.TempComplaintId, t.TempComplaintId
                    select new OnlineComplaints
                    {
                        CaseNo = t.CaseNo,
                        TEMP_COMPLAINT_ID = t.TempComplaintId,
                        TempComplaintDt = t.TempComplaintDt,
                        Name = t.ConsigneeName,
                        Designation = t.ConsigneeDesig,
                        Email = t.ConsigneeEmail,
                        MobileNO = t.ConsigneeMobile,
                        BKNo = t.BkNo,
                        SetNo = t.SetNo,
                        InspRegion = t.InspRegion,
                        RejMemono = t.RejMemoNo,
                        RejMemodate = t.RejMemoDt,
                        RejectionValue = t.RejectionValue,
                        RejectionReason = t.RejectionReason,
                        Remarks = t.Remarks,
                        //COMP_DOC = "Online_Complaints/" + t.TEMP_COMPLAINT_ID + ".pdf"
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.CaseNo).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.Name).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public OnlineComplaints FindByID(string TEMP_COMPLAINT_ID, string SetNo, string BKNo, string CaseNo)
        {
            OnlineComplaints model = new OnlineComplaints();
            DataTable dt = new DataTable();

            OracleParameter[] par = new OracleParameter[5];
            par[0] = new OracleParameter("p_CaseNo", OracleDbType.Varchar2, CaseNo, ParameterDirection.Input);
            par[1] = new OracleParameter("p_BkNo", OracleDbType.Varchar2, BKNo, ParameterDirection.Input);
            par[2] = new OracleParameter("p_SetNo", OracleDbType.Varchar2, SetNo, ParameterDirection.Input);
            par[3] = new OracleParameter("p_TempComplaintId", OracleDbType.Varchar2,TEMP_COMPLAINT_ID, ParameterDirection.Input);
            par[4] = new OracleParameter("p_ResultCursor", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GET_REJECTIONCOMPLAINT_DETAILS", par, 1);
            dt = ds.Tables[0];

            if (ds == null)
                throw new Exception("complaint Record Not found");
            else
            {
                model.TEMP_COMPLAINT_ID = dt.Rows[0]["TEMP_COMPLAINT_ID"].ToString();

                if (DateTime.TryParse(dt.Rows[0]["TEMP_COMPLAINT_DATE"].ToString(), out DateTime tempComplaintDt))
                {
                    model.TempComplaintDt = tempComplaintDt;
                }

                if (DateTime.TryParse(dt.Rows[0]["PO_DT"].ToString(), out DateTime poDt))
                {
                    model.Date = poDt;
                }

                model.BKNo = dt.Rows[0]["BK_NO"].ToString();
                model.SetNo = dt.Rows[0]["SET_NO"].ToString();
                model.Regioncode = dt.Rows[0]["region_code"].ToString();
                model.Contract = dt.Rows[0]["PO_NO"].ToString();

                if (DateTime.TryParse(dt.Rows[0]["IC_DATE"].ToString(), out DateTime icDt))
                {
                    model.IC_DT = icDt;
                }

                model.IC_NO = dt.Rows[0]["IC_NO"].ToString();
                model.Consignee = dt.Rows[0]["CONSIGNEE"].ToString();
                model.Vendor = dt.Rows[0]["VENDOR"].ToString();
                model.InspER = dt.Rows[0]["IE_NAME"].ToString();
                model.Item = dt.Rows[0]["ITEM_DESC"].ToString();

                if (int.TryParse(dt.Rows[0]["QTY_OFFERED"].ToString(), out int qtyperIC))
                {
                    model.QtyperIC = qtyperIC;
                }

                if (int.TryParse(dt.Rows[0]["QTY_REJECTED"].ToString(), out int qtyRejected))
                {
                    model.QtyRejected = qtyRejected;
                }

                if (decimal.TryParse(dt.Rows[0]["REJECTION_VALUE"].ToString(), out decimal rejectionValue))
                {
                    model.RejectionValue = rejectionValue;
                }

                model.RejectionReason = dt.Rows[0]["REJECTION_REASON"].ToString();

                if (DateTime.TryParse(dt.Rows[0]["rej_memo_dt"].ToString(), out DateTime memodt))
                {
                    model.RejMemodate = memodt;
                }
                model.RejMemono = dt.Rows[0]["rej_memo_no"].ToString();


                return model;
            }
        }

        public string RejectComp(OnlineComplaints model)
        {
            string msg = "";
            var complaint = context.TempOnlineComplaints.SingleOrDefault(c => c.TempComplaintId == model.TEMP_COMPLAINT_ID);

            if (complaint != null)
            {
                complaint.Status = "R";
                complaint.TempCompRejReason = model.Reasonforreject;
                context.SaveChanges();
                msg = "Reject Successfully!";
            }

            send_Consignee_Email_for_Rejected_Complaints(model);

            return msg;
        }

        public void send_Consignee_Email_for_Rejected_Complaints(OnlineComplaints model)
        {
            string wRegion = "";
            string sender = "";

            if(model.Regioncode == "N")
            {
                wRegion = "NORTHERN REGION \n 12th FLOOR,CORE-II,SCOPE MINAR,LAXMI NAGAR, DELHI - 110092 \n Phone : +918800018691-95 \n Fax : 011-22024665";
                sender = "nrinspn@rites.com";
            }
            else if(model.Regioncode == "S")
            {
                wRegion = "SOUTHERN REGION \n CTS BUILDING - 2ND FLOOR, BSNL COMPLEX, NO. 16, GREAMS ROAD,  CHENNAI - 600 006 \n Phone : 044-28292807/044- 28292817 \n Fax : 044-28290359";
                sender = "srinspn@rites.com";
            }
            else if(model.Regioncode == "E")
            {
                wRegion = "EASTERN REGION \n CENTRAL STATION BUILDING(METRO), 56, C.R. AVENUE,3rd FLOOR,KOLKATA-700 012  \n Fax : 033-22348704";
                sender = "erinspn@rites.com";
            }
            else if(model.Regioncode == "W")
            {
                wRegion = "WESTERN REGION \n 5TH FLOOR, REGENT CHAMBER, ABOVE STATUS RESTAURANT, NARIMAN POINT,MUMBAI-400021 \n Phone : 022-68943400/68943445";
                sender = "wrinspn@rites.com";
            }
            else if(model.Regioncode == "C")
            {
                wRegion = "Central Region";
            }

            var complaint = context.TempOnlineComplaints.SingleOrDefault(c => c.TempComplaintId == model.TEMP_COMPLAINT_ID);

            if (complaint != null)
            {
                string consignee = complaint.ConsigneeName;
                string consigneeEmail = complaint.ConsigneeEmail;
                string rejMemoNo = complaint.RejMemoNo;
                string callLetterDt = complaint.RejMemoDt.HasValue
                ? complaint.RejMemoDt.Value.ToString("dd/MM/yyyy")
                : "NIL";
            }

            string mailBody = "Dear Sir/Madam,\n\n Online Consignee Complaint vide Rej Memo Letter dated:  " + complaint.RejMemoNo + " for JI of material against PO No. - " + model.Contract + " dated - " + model.Date + ", on date: " + complaint.TempComplaintDt + ". The Complaint is rejected due to following Reason:- " + model.Reasonforreject + ", so Complaint not registered. \n\n Thanks for using RITES Inspection Services. \n NATIONAL INSPECTION HELP LINE NUMBER : 1800 425 7000 (TOLL FREE). \n\n" + wRegion + ".";

            SendMailModel SendMailModel = new SendMailModel();
            SendMailModel.To = complaint.ConsigneeEmail; ;
            SendMailModel.From = "nrinspn@gmail.com"; ;
            SendMailModel.Subject = "Your Consignee Complaint For RITES";
            SendMailModel.Message = mailBody;

            bool isSend = pSendMailRepository.SendMail(SendMailModel, null);

        }
    }
}
