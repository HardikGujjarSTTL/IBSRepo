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
using System.Xml;
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace IBS.Repositories
{
    public class ComplaintApprovalRepository : IComplaintApprovalRepository
    {
        private readonly ModelContext context;

        public ComplaintApprovalRepository(ModelContext context)
        {
            this.context = context;
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


                return model;
            }
        }
    }
}
