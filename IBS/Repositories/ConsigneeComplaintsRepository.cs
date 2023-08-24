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
using System.Security.Cryptography;
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

            if (ds != null)
            {
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

        public List<ConsigneeComplaints> GetDataListComplaint(string poNo, string poDt)
        {
            DTResult<ConsigneeComplaints> dTResult = new() { draw = 0 };
            IQueryable<ConsigneeComplaints>? query = null;

            ConsigneeComplaints model = new ConsigneeComplaints();
            DataTable dt = new DataTable();
            List<ConsigneeComplaints> modelList = new List<ConsigneeComplaints>();

            OracleParameter[] par1 = new OracleParameter[3];
            par1[0] = new OracleParameter("p_PONo", OracleDbType.Varchar2, poNo, ParameterDirection.Input);
            par1[1] = new OracleParameter("p_PODate", OracleDbType.Varchar2, poDt, ParameterDirection.Input);
            par1[2] = new OracleParameter("p_ResultSet", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds2 = DataAccessDB.GetDataSet("GetConsigneeComplaint", par1, 1);
            DataTable dt2 = ds2.Tables[0];

            List<ConsigneeComplaints> list2 = dt2.AsEnumerable().Select(row => new ConsigneeComplaints
            {
                CASE_NO = row.Field<string>("case_no"),
                PO_NO = row.Field<string>("po_no"),
                BK_NO = row.Field<string>("bk_no"),
                SET_NO = row.Field<string>("set_no"),
                IC_NO = row.Field<string>("IC_NO"),
                JiSno = row.Field<string>("ji_sno"),
                ComplaintId = row.Field<string>("complaint_id"),
                RejMemoNo = row.Field<string>("rej_memo_no"),
                PO_DT = row.Field<DateTime?>("PO_DT"),
                IC_DATE = row.Field<DateTime?>("IC_DT"),
                RejMemoDt = row.Field<DateTime?>("rej_memo_dt"),
            }).ToList();

            return list2;
        }
        public List<ConsigneeComplaints> GetDataListConsignee(string poNo, string poDt)
        {
            DTResult<ConsigneeComplaints> dTResult = new() { draw = 0 };
            IQueryable<ConsigneeComplaints>? query = null;

            ConsigneeComplaints model = new ConsigneeComplaints();
            DataTable dt = new DataTable();
            List<ConsigneeComplaints> modelList = new List<ConsigneeComplaints>();

            OracleParameter[] par = new OracleParameter[3];
            par[0] = new OracleParameter("p_po_no_param", OracleDbType.Varchar2, poNo, ParameterDirection.Input);
            par[1] = new OracleParameter("p_po_date_param", OracleDbType.Varchar2, poDt, ParameterDirection.Input);
            par[2] = new OracleParameter("RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds1 = DataAccessDB.GetDataSet("GetFilteredConsigneeComplaints", par, 1);
            DataTable dt1 = ds1.Tables[0];

            List<ConsigneeComplaints> list1 = dt1.AsEnumerable().Select(row => new ConsigneeComplaints
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

            return list1;

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
