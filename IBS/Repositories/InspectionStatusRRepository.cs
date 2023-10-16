using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using IBS.Models.Reports;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Reflection.Emit;
using static IBS.Helper.Enums;

namespace IBS.Repositories
{
    public class InspectionStatusRRepository : IInspectionStatusRepository
    {
        private readonly ModelContext context;

        public InspectionStatusRRepository(ModelContext context)
        {
            this.context = context;
        }
        public InspectionStatusModel SummaryConsigneeWiseInsp(string ReportType, string Month, string Year, string ForGiven, string ReportBasedon, string MaterialValue, string FromDate, string ToDate, string ForParticular, string lstParticular, string Regin)
        {

            InspectionStatusModel model = new();
            List<InspectionStatusModel> lstSummary = new();
                       

            OracleParameter[] par = new OracleParameter[11];
            par[0] = new OracleParameter("P_REGION_CODE", OracleDbType.NVarchar2, Regin, ParameterDirection.Input);
            par[1] = new OracleParameter("P_ForGPer", OracleDbType.Boolean, Convert.ToBoolean(ForGiven), ParameterDirection.Input);
            par[2] = new OracleParameter("P_rdbICDT", OracleDbType.Boolean, Convert.ToBoolean(ReportBasedon), ParameterDirection.Input);
            par[3] = new OracleParameter("P_rdbPart", OracleDbType.Boolean, ForParticular, ParameterDirection.Input);
            par[4] = new OracleParameter("P_rdbMatValue", OracleDbType.Boolean, MaterialValue, ParameterDirection.Input);
            par[5] = new OracleParameter("p_From_Date", OracleDbType.Date, FromDate, ParameterDirection.Input);
            par[6] = new OracleParameter("p_To_Date", OracleDbType.Date, ToDate, ParameterDirection.Input);
            par[7] = new OracleParameter("P_lstConsignee", OracleDbType.NVarchar2, lstParticular, ParameterDirection.Input);
            par[8] = new OracleParameter("p_Year", OracleDbType.NVarchar2, Year, ParameterDirection.Input);
            par[9] = new OracleParameter("p_Month", OracleDbType.NVarchar2, Month, ParameterDirection.Input);
            par[10] = new OracleParameter("P_CONSIGNEE_INFO", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GET_CONSIGNEE_INFO", par, 10);
            int recCount = 0;
            if (ds.Tables[0].Rows.Count != 0)
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                    lstSummary = JsonConvert.DeserializeObject<List<InspectionStatusModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                    recCount = recCount + 1;
                    //lstPerformance.ToList().ForEach(i => { i.C3 = decimal.Truncate(i.C3); i.C7 = decimal.Truncate(i.C7); i.CM7 = decimal.Truncate(i.CM7); i.C10 = decimal.Truncate(i.C10); i.CALLS = decimal.Truncate(i.CALLS); i.CALL_CANCEL = decimal.Truncate(i.CALL_CANCEL); i.REJECTIONS = decimal.Truncate(i.REJECTIONS); });
                    model.SrNo = recCount;
                    model.CONSIGNEE = Convert.ToString(ds.Tables[0].Rows[0]["CONSIGNEE"]);
                    model.NO_OF_INSP = Convert.ToString(ds.Tables[0].Rows[0]["NO_OF_INSP"]);
                    model.MATERIAL_VALUE = Convert.ToString(ds.Tables[0].Rows[0]["MATERIAL_VALUE"]);
                    model.INSP_FEE = Convert.ToString(ds.Tables[0].Rows[0]["INSP_FEE"]);
                }
                model.lstSummaryConreport = lstSummary;
            }
            return model;
        }
        public InspectionStatusModel SummaryVendorWiseInsp(string ReportType, string Month, string Year, string ForGiven, string ReportBasedon, string MaterialValue, string FromDate, string ToDate, string ForParticular, string lstParticular, string Regin)
        {

            InspectionStatusModel model = new();
            List<InspectionStatusModel> lstSummary = new();


            OracleParameter[] par = new OracleParameter[11];
            par[0] = new OracleParameter("P_REGION_CODE", OracleDbType.NVarchar2, Regin, ParameterDirection.Input);
            par[1] = new OracleParameter("P_ForGPer", OracleDbType.Boolean, Convert.ToBoolean(ForGiven), ParameterDirection.Input);
            par[2] = new OracleParameter("P_rdbICDT", OracleDbType.Boolean, Convert.ToBoolean(ReportBasedon), ParameterDirection.Input);
            par[3] = new OracleParameter("P_rdbPart", OracleDbType.Boolean, ForParticular, ParameterDirection.Input);
            par[4] = new OracleParameter("P_rdbMatValue", OracleDbType.Boolean, MaterialValue, ParameterDirection.Input);
            par[5] = new OracleParameter("p_From_Date", OracleDbType.Date, FromDate, ParameterDirection.Input);
            par[6] = new OracleParameter("p_To_Date", OracleDbType.Date, ToDate, ParameterDirection.Input);
            par[7] = new OracleParameter("P_lstVendor", OracleDbType.NVarchar2, lstParticular, ParameterDirection.Input);
            par[8] = new OracleParameter("p_Year", OracleDbType.NVarchar2, Year, ParameterDirection.Input);
            par[9] = new OracleParameter("p_Month", OracleDbType.NVarchar2, Month, ParameterDirection.Input);
            par[10] = new OracleParameter("P_Vendor_INFO", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GET_Vendor_INFO", par, 10);
            int recCount = 0;
            if (ds.Tables[0].Rows.Count != 0)
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                    lstSummary = JsonConvert.DeserializeObject<List<InspectionStatusModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                    recCount = recCount + 1;
                    //lstPerformance.ToList().ForEach(i => { i.C3 = decimal.Truncate(i.C3); i.C7 = decimal.Truncate(i.C7); i.CM7 = decimal.Truncate(i.CM7); i.C10 = decimal.Truncate(i.C10); i.CALLS = decimal.Truncate(i.CALLS); i.CALL_CANCEL = decimal.Truncate(i.CALL_CANCEL); i.REJECTIONS = decimal.Truncate(i.REJECTIONS); });
                    model.SrNo = recCount;
                    model.VENDOR = Convert.ToString(ds.Tables[0].Rows[0]["VENDOR"]);
                    model.NO_OF_INSP = Convert.ToString(ds.Tables[0].Rows[0]["NO_OF_INSP"]);
                    model.MATERIAL_VALUE = Convert.ToString(ds.Tables[0].Rows[0]["MATERIAL_VALUE"]);
                    model.INSP_FEE = Convert.ToString(ds.Tables[0].Rows[0]["INSP_FEE"]);
                }
                model.lstSummaryConreport = lstSummary;
            }
            return model;
        }
        public InspectionStatusModel SummaryInsp(string ReportType, string Month, string Year, string ForGiven, string ReportBasedon, string FromDate, string ToDate, string ForParticular, string lstParticular, string Regin, string TextPurchaser)
        {

            InspectionStatusModel model = new();
            List<InspectionStatusModel> lstSummary = new();


            OracleParameter[] par = new OracleParameter[11];
            par[0] = new OracleParameter("P_REGION_CODE", OracleDbType.NVarchar2, Regin, ParameterDirection.Input);
            par[1] = new OracleParameter("P_ForGPer", OracleDbType.Boolean, Convert.ToBoolean(ForGiven), ParameterDirection.Input);
            par[2] = new OracleParameter("P_rdbICDT", OracleDbType.Boolean, Convert.ToBoolean(ReportBasedon), ParameterDirection.Input);
            par[3] = new OracleParameter("P_rdbPart", OracleDbType.Boolean, ForParticular, ParameterDirection.Input);
            par[4] = new OracleParameter("P_TextPur", OracleDbType.NVarchar2, TextPurchaser, ParameterDirection.Input);
            par[5] = new OracleParameter("p_From_Date", OracleDbType.Date, FromDate, ParameterDirection.Input);
            par[6] = new OracleParameter("p_To_Date", OracleDbType.Date, ToDate, ParameterDirection.Input);
            par[7] = new OracleParameter("P_lstPurchaser", OracleDbType.NVarchar2, lstParticular, ParameterDirection.Input);
            par[8] = new OracleParameter("p_Year", OracleDbType.NVarchar2, Year, ParameterDirection.Input);
            par[9] = new OracleParameter("p_Month", OracleDbType.NVarchar2, Month, ParameterDirection.Input);
            par[10] = new OracleParameter("P_DetailsPurchase_INFO", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GET_DetailsPurchase_INFO", par, 10);

            if (ds.Tables[0].Rows.Count != 0)
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                    lstSummary = JsonConvert.DeserializeObject<List<InspectionStatusModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                    model.PURCHASER = Convert.ToString(ds.Tables[0].Rows[0]["PURCHASER"]);
                    model.CASE_NO = Convert.ToString(ds.Tables[0].Rows[0]["CASE_NO"]);
                    model.PO_NO = Convert.ToString(ds.Tables[0].Rows[0]["PO_NO"]);
                    model.PO_DT = Convert.ToString(ds.Tables[0].Rows[0]["PO_DT"]);
                    model.REMARKS = Convert.ToString(ds.Tables[0].Rows[0]["REMARKS"]);
                    model.IC_NO = Convert.ToString(ds.Tables[0].Rows[0]["IC_NO"]);
                    model.IC_DT = Convert.ToString(ds.Tables[0].Rows[0]["IC_DT"]);
                    model.BK_NO = Convert.ToString(ds.Tables[0].Rows[0]["BK_NO"]);
                    model.SET_NO = Convert.ToString(ds.Tables[0].Rows[0]["SET_NO"]);
                    model.BILL_NO = Convert.ToString(ds.Tables[0].Rows[0]["BILL_NO"]);
                    model.BILL_DATE = Convert.ToString(ds.Tables[0].Rows[0]["BILL_DATE"]);
                    model.IE_SNAME = Convert.ToString(ds.Tables[0].Rows[0]["IE_SNAME"]);
                    model.BPO = Convert.ToString(ds.Tables[0].Rows[0]["BPO"]);
                    model.VENDOR = Convert.ToString(ds.Tables[0].Rows[0]["VENDOR"]);
                    model.INSP_FEE = Convert.ToString(ds.Tables[0].Rows[0]["INSP_FEE"]);
                    model.VISITS = Convert.ToString(ds.Tables[0].Rows[0]["VISITS"]);
                    model.VALUE = Convert.ToString(ds.Tables[0].Rows[0]["VALUE"]);
                    model.ITEM_DESC = Convert.ToString(ds.Tables[0].Rows[0]["ITEM_DESC"]);
                }
                model.lstSummaryConreport = lstSummary;
            }
            return model;
        }
        public InspectionStatusModel VendorWiseInsp(string ReportType, string Month, string Year, string FromDate, string ToDate, string rdbGIE, string rdbForMonth, string ForGPer, string ddlVender, string Regin)
        {

            InspectionStatusModel model = new();
            List<InspectionStatusModel> lstSummary = new();


            OracleParameter[] par = new OracleParameter[10];
            par[0] = new OracleParameter("P_REGION_CODE", OracleDbType.NVarchar2, Regin, ParameterDirection.Input);
            par[1] = new OracleParameter("rdbGIE", OracleDbType.Boolean, Convert.ToBoolean(rdbGIE), ParameterDirection.Input);
            par[2] = new OracleParameter("rdbForMonth", OracleDbType.Boolean, Convert.ToBoolean(rdbForMonth), ParameterDirection.Input);
            par[3] = new OracleParameter("ForGPer", OracleDbType.Boolean, ForGPer, ParameterDirection.Input);
            par[4] = new OracleParameter("p_From_Date", OracleDbType.Date, FromDate, ParameterDirection.Input);
            par[5] = new OracleParameter("p_To_Date", OracleDbType.Date, ToDate, ParameterDirection.Input);
            par[6] = new OracleParameter("p_Year", OracleDbType.NVarchar2, Year, ParameterDirection.Input);
            par[7] = new OracleParameter("p_Month", OracleDbType.NVarchar2, Month, ParameterDirection.Input);
            par[8] = new OracleParameter("ddlVender", OracleDbType.NVarchar2, ddlVender, ParameterDirection.Input);
            par[9] = new OracleParameter("P_Vendorwise_INFO", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("Get_VendorWiseInspStatus", par, 9);

            if (ds.Tables[0].Rows.Count != 0)
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                    lstSummary = JsonConvert.DeserializeObject<List<InspectionStatusModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                    model.VENDOR = Convert.ToString(ds.Tables[0].Rows[0]["VENDOR"]);
                    model.IE_NAME = Convert.ToString(ds.Tables[0].Rows[0]["IE_NAME"]);
                    model.CASE_NO = Convert.ToString(ds.Tables[0].Rows[0]["CASE_NO"]);
                    model.PO_NO = Convert.ToString(ds.Tables[0].Rows[0]["PO_NO"]);
                    model.PO_DATE = Convert.ToString(ds.Tables[0].Rows[0]["PO_DATE"]);
                    model.CALL_RECV_DT = Convert.ToString(ds.Tables[0].Rows[0]["CALL_RECV_DT"]);
                    model.CALL_SNO = Convert.ToString(ds.Tables[0].Rows[0]["CALL_SNO"]);
                    model.FIRST_INSP_DATE = Convert.ToString(ds.Tables[0].Rows[0]["FIRST_INSP_DATE"]);
                    model.LAST_INSP_DATE = Convert.ToString(ds.Tables[0].Rows[0]["LAST_INSP_DATE"]);
                    model.IC_DATE = Convert.ToString(ds.Tables[0].Rows[0]["IC_DATE"]);
                    model.ITEM_DESC_PO = Convert.ToString(ds.Tables[0].Rows[0]["ITEM_DESC_PO"]);
                    model.QTY_PASSED = Convert.ToString(ds.Tables[0].Rows[0]["QTY_PASSED"]);
                    model.QTY_REJECTED = Convert.ToString(ds.Tables[0].Rows[0]["QTY_REJECTED"]);
                   
                }
                model.lstSummaryConreport = lstSummary;
            }
            return model;
        }

        public List<railway_dropdown> GetValue(string selectedValue)
        {
            var query = from railway in context.T91Railways
                        where railway.RlyCd != "CORE"
                        orderby railway.RlyCd
                        select new railway_dropdown
                        {
                            RLY_CD = railway.RlyCd,
                            RAILWAY_ORGN = railway.Railway
                        };


            return query.ToList();
        }
        public List<railway_dropdown> GetValue2(string selectedValue)
        {
            var query = context.T12BillPayingOfficers
             .Where(bpo => bpo.BpoType == selectedValue)
             .Select(bpo => new railway_dropdown
             {
                 //BPO_RLY = bpo.BPO_RLY,
                 RLY_CD = bpo.BpoRly,
                 RAILWAY_ORGN = bpo.BpoOrgn
             })
             .OrderBy(item => item.RLY_CD)
             .ToList();



            return query.ToList();
        }
        public DTResult<InspectionStatusModel> gridData(DTParameters dtParameters)
        {
            DTResult<InspectionStatusModel> dTResult = new() { draw = 0 };
            IQueryable<InspectionStatusModel>? query = null;

            string ClientType = dtParameters.AdditionalValues?.GetValueOrDefault("ClientType");
            string SelectClient = dtParameters.AdditionalValues?.GetValueOrDefault("SelectClient");
            string PODate = dtParameters.AdditionalValues?.GetValueOrDefault("PODate");



            query = (from t13 in context.T13PoMasters
                     join t17 in context.T17CallRegisters on t13.CaseNo equals t17.CaseNo
                     where t13.RlyNonrly == ClientType &&
                           t13.RlyCd == SelectClient &&
                           t13.PoDt == Convert.ToDateTime(PODate)
                     group new { t13, t17 } by new
                     {
                         t13.L5noPo,
                         t13.PoNo,
                         t13.PoDt,
                         t13.RlyNonrly,
                         t13.RlyCd
                     } into grouped
                     orderby grouped.Key.PoDt
                     select new InspectionStatusModel
                     {
                         L5NO_PO = grouped.Key.L5noPo,
                         PO_NO = grouped.Key.PoNo,
                         PO_DT = Convert.ToString(grouped.Key.PoDt),
                         RLY_NONRLY = grouped.Key.RlyNonrly,
                         RLY_CD = grouped.Key.RlyCd
                     });

            query = query.Distinct();


            var result = query.ToList();
            dTResult.data = result;

            return dTResult;
        }

        public InspectionStatusModel ICDetailsPO(string ReportType, string PO_NO, string PO_DT, string RLY_NONRLY, string RLY_CD)
        {

            InspectionStatusModel model = new();
            List<InspectionStatusModel> lstSummary = new();

            string trimmedPO_NO = PO_NO.Substring(PO_NO.Length - 5);
            OracleParameter[] par = new OracleParameter[5];
            par[0] = new OracleParameter("P_PNO", OracleDbType.NVarchar2, trimmedPO_NO, ParameterDirection.Input);
            par[1] = new OracleParameter("P_PDT", OracleDbType.Date, PO_DT, ParameterDirection.Input);
            par[2] = new OracleParameter("P_CLT", OracleDbType.NVarchar2, RLY_NONRLY, ParameterDirection.Input);
            par[3] = new OracleParameter("P_RLYCD", OracleDbType.NVarchar2, RLY_CD, ParameterDirection.Input);
            par[4] = new OracleParameter("P_PurchaseDetails", OracleDbType.RefCursor, ParameterDirection.Output);
            //par[5] = new OracleParameter("P_ICDetails", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("Get_ICDetailsPO", par, 4);

            if (ds.Tables[0].Rows.Count != 0)
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                    lstSummary = JsonConvert.DeserializeObject<List<InspectionStatusModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                    model.VENDOR = Convert.ToString(ds.Tables[0].Rows[0]["VENDOR"]);
                    model.PURCHASER = Convert.ToString(ds.Tables[0].Rows[0]["PURCHASER"]);
                    model.PO_NO = Convert.ToString(ds.Tables[0].Rows[0]["PO_NO"]);
                    model.PO_DT = Convert.ToString(ds.Tables[0].Rows[0]["PO_DT"]);
                    model.IC_NO = Convert.ToString(ds.Tables[0].Rows[0]["IC_NO"]);
                    model.IC_DATE = Convert.ToString(ds.Tables[0].Rows[0]["IC_DATE"]);
                    model.BK_NO = Convert.ToString(ds.Tables[0].Rows[0]["BK_NO"]);
                    model.SET_NO = Convert.ToString(ds.Tables[0].Rows[0]["SET_NO"]);
                    model.IE_NAME = Convert.ToString(ds.Tables[0].Rows[0]["IE_NAME"]);
                    model.ITEM_DESC_PO = Convert.ToString(ds.Tables[0].Rows[0]["ITEM_DESC_PO"]);
                    model.QTY_TO_INSP = Convert.ToString(ds.Tables[0].Rows[0]["QTY_TO_INSP"]);
                    model.QTY_PASSED = Convert.ToString(ds.Tables[0].Rows[0]["QTY_PASSED"]);
                    model.HOLOGRAM = Convert.ToString(ds.Tables[0].Rows[0]["HOLOGRAM"]);
                    model.IC_PHOTO = Convert.ToString(ds.Tables[0].Rows[0]["IC_PHOTO"]);

                }
                model.lstSummaryConreport = lstSummary;
                CallsDetailsPO(ReportType, PO_NO,PO_DT, RLY_NONRLY,RLY_CD);
            }
            //if (ds.Tables[1].Rows.Count != 0)
            //{
            //    InspectionStatusModel InspectionStatusModel = new();
            //    if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
            //    {
            //        string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
            //        lstSummary = JsonConvert.DeserializeObject<List<InspectionStatusModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            //        model.VENDOR = Convert.ToString(ds.Tables[0].Rows[0]["VENDOR"]);
            //        model.MANUFACTURER = Convert.ToString(ds.Tables[0].Rows[0]["MANUFACTURER"]);
            //        model.ITEM_DESC_PO = Convert.ToString(ds.Tables[0].Rows[0]["ITEM_DESC_PO"]);
            //        model.CALL_MARK_DT = Convert.ToString(ds.Tables[0].Rows[0]["CALL_MARK_DT"]);
            //        model.IE_NAME = Convert.ToString(ds.Tables[0].Rows[0]["IE_NAME"]);
            //        model.IE_PHONE_NO = Convert.ToString(ds.Tables[0].Rows[0]["IE_PHONE_NO"]);
            //        model.PO_NO = Convert.ToString(ds.Tables[0].Rows[0]["PO_NO"]);
            //        model.PO_DATE = Convert.ToString(ds.Tables[0].Rows[0]["PO_DATE"]);
            //        model.CASE_NO = Convert.ToString(ds.Tables[0].Rows[0]["CASE_NO"]);
            //        model.CALL_STATUS = Convert.ToString(ds.Tables[0].Rows[0]["CALL_STATUS"]);
            //        model.HOLOGRAM = Convert.ToString(ds.Tables[0].Rows[0]["HOLOGRAM"]);
            //        model.IC_PHOTO = Convert.ToString(ds.Tables[0].Rows[0]["IC_PHOTO"]);
            //        model.MFG_PERS = Convert.ToString(ds.Tables[0].Rows[0]["MFG_PERS"]);
            //        model.MFG_PHONE = Convert.ToString(ds.Tables[0].Rows[0]["MFG_PHONE"]);
            //        model.CALL_SNO = Convert.ToString(ds.Tables[0].Rows[0]["CALL_SNO"]);
            //        model.CO_NAME = Convert.ToString(ds.Tables[0].Rows[0]["CO_NAME"]);
            //        model.REMARKS = Convert.ToString(ds.Tables[0].Rows[0]["REMARK"]);


            //    }
            //    model.lstCallDetails = lstSummary;
            //}
            return model;
        }
        public InspectionStatusModel CallsDetailsPO(string ReportType, string PO_NO, string PO_DT, string RLY_NONRLY, string RLY_CD)
        {

            InspectionStatusModel model = new();
            List<InspectionStatusModel> lstSummary = new();

            string trimmedPO_NO = PO_NO.Substring(PO_NO.Length - 5);
            OracleParameter[] par = new OracleParameter[5];
            par[0] = new OracleParameter("P_PNO", OracleDbType.NVarchar2, trimmedPO_NO, ParameterDirection.Input);
            par[1] = new OracleParameter("P_PDT", OracleDbType.Date, PO_DT, ParameterDirection.Input);
            par[2] = new OracleParameter("P_CLT", OracleDbType.NVarchar2, RLY_NONRLY, ParameterDirection.Input);
            par[3] = new OracleParameter("P_RLYCD", OracleDbType.NVarchar2, RLY_CD, ParameterDirection.Input);
            par[4] = new OracleParameter("P_ICDetails", OracleDbType.RefCursor, ParameterDirection.Output);
            //par[5] = new OracleParameter("P_ICDetails", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("Get_CallDetailsPO", par, 4);


            if (ds.Tables[1].Rows.Count != 0)
            {
                InspectionStatusModel InspectionStatusModel = new();
                if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                {
                    string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                    lstSummary = JsonConvert.DeserializeObject<List<InspectionStatusModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                    model.VENDOR = Convert.ToString(ds.Tables[0].Rows[0]["VENDOR"]);
                    model.MANUFACTURER = Convert.ToString(ds.Tables[0].Rows[0]["MANUFACTURER"]);
                    model.ITEM_DESC_PO = Convert.ToString(ds.Tables[0].Rows[0]["ITEM_DESC_PO"]);
                    model.CALL_MARK_DT = Convert.ToString(ds.Tables[0].Rows[0]["CALL_MARK_DT"]);
                    model.IE_NAME = Convert.ToString(ds.Tables[0].Rows[0]["IE_NAME"]);
                    model.IE_PHONE_NO = Convert.ToString(ds.Tables[0].Rows[0]["IE_PHONE_NO"]);
                    model.PO_NO = Convert.ToString(ds.Tables[0].Rows[0]["PO_NO"]);
                    model.PO_DATE = Convert.ToString(ds.Tables[0].Rows[0]["PO_DATE"]);
                    model.CASE_NO = Convert.ToString(ds.Tables[0].Rows[0]["CASE_NO"]);
                    model.CALL_STATUS = Convert.ToString(ds.Tables[0].Rows[0]["CALL_STATUS"]);
                    model.HOLOGRAM = Convert.ToString(ds.Tables[0].Rows[0]["HOLOGRAM"]);
                    model.IC_PHOTO = Convert.ToString(ds.Tables[0].Rows[0]["IC_PHOTO"]);
                    model.MFG_PERS = Convert.ToString(ds.Tables[0].Rows[0]["MFG_PERS"]);
                    model.MFG_PHONE = Convert.ToString(ds.Tables[0].Rows[0]["MFG_PHONE"]);
                    model.CALL_SNO = Convert.ToString(ds.Tables[0].Rows[0]["CALL_SNO"]);
                    model.CO_NAME = Convert.ToString(ds.Tables[0].Rows[0]["CO_NAME"]);
                    model.REMARKS = Convert.ToString(ds.Tables[0].Rows[0]["REMARK"]);


                }
                model.lstCallDetails = lstSummary;
            }
            return model;
        }
    }
}
