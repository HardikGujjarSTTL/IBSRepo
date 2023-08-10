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
    public class ConsigneeComplaintsRepository : IConsigneeComplaintsRepository
    {
        private readonly ModelContext context;

        public ConsigneeComplaintsRepository(ModelContext context)
        {
            this.context = context;
        }

        public ConsigneeComplaints FindByID(string CASE_NO,string BK_NO,string SET_NO)
        {
            ConsigneeComplaints model = new ConsigneeComplaints();
            DataTable dt = new DataTable();

            OracleParameter[] par = new OracleParameter[4];
            par[0] = new OracleParameter("p_case_no", OracleDbType.Varchar2, CASE_NO, ParameterDirection.Input);
            par[1] = new OracleParameter("p_bk_no", OracleDbType.Varchar2, BK_NO, ParameterDirection.Input);
            par[2] = new OracleParameter("p_set_no", OracleDbType.Varchar2, SET_NO, ParameterDirection.Input);
            par[3] = new OracleParameter("RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);



            var ds = DataAccessDB.GetDataSet("GetConsigneeComplaintDetails", par, 1);
            dt = ds.Tables[0];

            string regiondata = "";
            string wRegion = dt.Rows[0].Field<string>("region_code");
            if (wRegion == "N") { regiondata = "Northern Region"; }
            else if (wRegion == "E") { regiondata = "Eastern Region"; }
            else if (wRegion == "W") { regiondata = "Western Region"; }
            else if (wRegion == "S") { regiondata = "Southern Region"; }
            else if (wRegion == "C") { regiondata = "Central Region"; }
            else { regiondata = ""; }

            List<ConsigneeComplaints> list = dt.AsEnumerable().Select(row => new ConsigneeComplaints
            {
                CASE_NO = row.Field<string>("case_no"),
                PO_NO = row.Field<string>("po"),
                BK_NO = row.Field<string>("bk_no"),
                SET_NO = row.Field<string>("set_no"),
                IC_NO = row.Field<string>("ic_no"),
                Consignee = row.Field<string>("CONSIGNEE"),
                VEND_NAME = row.Field<string>("vendor"),
                FormattedIC_DATE = row.Field<string>("IC_Dt"),
                Railway = row.Field<string>("rly_cd"),
                CoName = row.Field<string>("co_name"),
                VendCd = row.Field<int>("vend_cd"),
                ie_name = row.Field<string>("ie_name"),
                ConsigneeCd = row.Field<int>("Consignee_cd"),
            }).ToList();

            //var result = context.T08IeControllOfficers
            //    .Where(co => co.CoRegion == CASE_NO.Substring(0, 1))
            //    .OrderBy(co => co.CoName)
            //    .ToList();

            //var dropdownItems = result.Select(item => new SelectListItem
            //{
            //    Value = item.CoCd.ToString(),
            //    Text = item.CoName
            //});
            //string selectedCoCdFromDataTable = dt.Rows[0]["co_name"].ToString();

            if (ds != null)
            {
                //model.ControllingManagers = new SelectList(dropdownItems, "Value", "Text");
                //model.CoName = selectedCoCdFromDataTable;
                model.CASE_NO = list[0].CASE_NO;
                    model.ComplaintDate = list[0].ComplaintDate;
                    model.PO_DT = list[0].PO_DT;
                    model.ComplaintId = list[0].ComplaintId;
                    model.FormattedPO_DT = list[0].PO_DT?.ToString("MM-dd-yyyy");
                    model.FormattedComplaintDate = list[0].ComplaintDate?.ToString("MM-dd-yyyy");
                    model.PO_NO = list[0].PO_NO;
                    model.VEND_NAME = list[0].VEND_NAME;
                    model.ConsigneeCd = list[0].ConsigneeCd;
                    model.VendCd = list[0].VendCd;
                    model.InspRegion = regiondata;
                    model.BK_NO = list[0].BK_NO;
                    model.SET_NO = list[0].SET_NO;
                    model.ie_name = list[0].ie_name;
                    model.CoName = list[0].CoName;
                    model.Consignee = list[0].Consignee;
                    model.FormattedIC_DATE = list[0].FormattedIC_DATE;
                    model.RejMemoDt = list[0].RejMemoDt;
                    model.RejMemoNo = list[0].RejMemoNo;
                    model.Railway = list[0].Railway;
                    model.IC_NO = list[0].IC_NO;
                    model.ItemDesc = list[0].ItemDesc;
                    model.QtyOffered = list[0].QtyOffered;
                    model.QtyRejected = list[0].QtyRejected;
                    model.Rate = list[0].Rate;
                    model.RejectionReason = list[0].RejectionReason;
                    model.InspectionBy = list[0].InspectionBy;
            }
            return model;
        }

        public DTResult<ConsigneeComplaints> GetDataList(DTParameters dtParameters)
        {
            DTResult<ConsigneeComplaints> dTResult = new() { draw = 0 };
            IQueryable<ConsigneeComplaints>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "PoNo";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
            }
            else
            {
                orderCriteria = "PoNo";
                orderAscendingDirection = true;
            }

            ConsigneeComplaints model = new ConsigneeComplaints();
            DataTable dt = new DataTable();
            List<ConsigneeComplaints> modelList = new List<ConsigneeComplaints>();

            string PoNo = "", PoDt = "";

            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["PoNo"]))
            {
                PoNo = Convert.ToString(dtParameters.AdditionalValues["PoNo"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["PoDt"]))
            {
                PoDt = Convert.ToString(dtParameters.AdditionalValues["PoDt"]);
            }

            PoNo = PoNo.ToString() == "" ? string.Empty : PoNo.ToString();
            DateTime? _PoDt = PoDt == "" ? null : DateTime.ParseExact(PoDt, "dd-MM-yyyy", null);


            OracleParameter[] par = new OracleParameter[3];
            par[0] = new OracleParameter("p_po_no_param", OracleDbType.Varchar2, PoNo, ParameterDirection.Input);
            par[1] = new OracleParameter("p_po_date_param", OracleDbType.Varchar2, PoDt, ParameterDirection.Input);
            par[2] = new OracleParameter("RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GetFilteredConsigneeComplaints", par, 1);
            dt = ds.Tables[0];

            List<ConsigneeComplaints> list = dt.AsEnumerable().Select(row => new ConsigneeComplaints
            {
                CASE_NO = row.Field<string>("case_no"),
                PO_NO = row.Field<string>("po_no"),
                PO_DT = row.Field<DateTime?>("PO_DT"),
                BK_NO = row.Field<string>("bk_no"),
                SET_NO = row.Field<string>("set_no"),
                IC_NO = row.Field<string>("IC_NO"),
                VEND_NAME = row.Field<string>("VEND_NAME"),
                Consignee = row.Field<string>("Consignee"),
                IC_DATE = row.Field<DateTime?>("IC_DATE"),
                Railway = row.Field<string>("rly_cd"),
            }).ToList();


            //if (!string.IsNullOrEmpty(searchBy))
            //{
            //    query = query.Where(w => Convert.ToString(w.PO_NO).ToLower().Contains(searchBy.ToLower()) || Convert.ToString(w.set).ToLower().Contains(searchBy.ToLower())).AsQueryable();
            //}


            query = list.AsQueryable();

            dTResult.recordsTotal = ds.Tables[0].Rows.Count;

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;

        }

        //public int ComplaintsDetailsInsertUpdate(ConsigneeComplaints model)
        //{
        //    int Complaintid = 0;
        //    var Complaint = (from r in context.T40ConsigneeComplaints where r.ComplaintId == model.ComplaintId select r).FirstOrDefault();
        //    #region Complaint save
        //    if (Complaint == null)
        //    {
        //        ConsigneeComplaints obj = new ConsigneeComplaints();
        //        obj.RejMemoNo = model.RejMemoNo;
        //        obj.CASE_NO = model.CASE_NO;
        //        obj.BK_NO = model.BK_NO;
        //        obj.SET_NO = model.SET_NO;
        //        obj.InspRegion = model.InspRegion;
        //        obj.ConsigneeCd = model.ConsigneeCd;
        //        obj.RejMemoNo = model.RejMemoNo;
        //        obj.VendCd = model.VendCd;
        //        obj.ItemDesc = model.ItemDesc;
        //        obj.QtyRejected = model.QtyRejected;
        //        obj.QtyRejected = model.QtyRejected;
        //        obj.ComplaintDate = DateTime.Now;
        //        obj.RejMemoDt = DateTime.Now;

        //        //context.T40ConsigneeComplaints.Add(obj);
        //        context.SaveChanges();
        //        Complaintid = Convert.ToInt32(obj.ComplaintId);
        //    }
        //    else
        //    {
        //        Complaint.RejMemoNo = model.RejMemoNo;
        //        Complaint.CaseNo = model.CASE_NO;
        //        Complaint.BkNo = model.BK_NO;
        //        Complaint.SetNo = model.SET_NO;
        //        Complaint.InspRegion = model.InspRegion;
        //        Complaint.ConsigneeCd = model.ConsigneeCd;
        //        Complaint.RejMemoNo = model.RejMemoNo;
        //        Complaint.VendCd = model.VendCd;
        //        Complaint.ItemDesc = model.ItemDesc;
        //        Complaint.QtyRejected = model.QtyRejected;
        //        Complaint.QtyRejected = model.QtyRejected;
        //        Complaint.ComplaintDt = DateTime.Now;
        //        Complaint.RejMemoDt = DateTime.Now;
        //        context.SaveChanges();
        //        Complaintid = Convert.ToInt32(Complaint.ComplaintId);
        //    }
        //    #endregion
        //    return Complaintid;
        //}
    }
}
