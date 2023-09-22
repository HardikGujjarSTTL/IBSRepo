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
            if (ds.Tables.Count != 0)
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
            if (ds.Tables.Count != 0)
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

            if (ds.Tables.Count != 0)
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
    }
}
