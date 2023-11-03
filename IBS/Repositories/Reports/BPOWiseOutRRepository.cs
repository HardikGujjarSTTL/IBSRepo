using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces.Reports;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OfficeOpenXml;
using Oracle.ManagedDataAccess.Client;
using PuppeteerSharp;
using System.Data;
using System.Drawing;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace IBS.Repositories.Reports
{
    public class BPOWiseOutRRepository : IBPOWiseOutstandingBillsRepository
    {
        private readonly ModelContext context;

        public BPOWiseOutRRepository(ModelContext context)
        {
            this.context = context;
        }
        //public BPOWiseOutstandingBillsModel GenerateReport(string ReportType, string FromDt, string ToDt, string BpoCd, string BpoType, string BpoRly, string BpoRegion, string Checkbox, string Railway, string PSU, string StateGovt, string ForeignRailways, string PrivateSector, string TypeofOutStandingBills, string Region)
        //{

        //    using (var dbContext = context.Database.GetDbConnection())
        //    {
        //        OracleParameter[] par = new OracleParameter[10];
        //        par[0] = new OracleParameter("p_RegionCode", OracleDbType.NVarchar2, BPOWiseOutstandingBillsModel.Region, ParameterDirection.Input);
        //        par[1] = new OracleParameter("p_StartDate", OracleDbType.NVarchar2, BPOWiseOutstandingBillsModel.FromDt, ParameterDirection.Input);
        //        par[2] = new OracleParameter("p_EndDate", OracleDbType.NVarchar2, BPOWiseOutstandingBillsModel.ToDt, ParameterDirection.Input);
        //        par[3] = new OracleParameter("p_BPOCode", OracleDbType.NVarchar2, BPOWiseOutstandingBillsModel.BpoCd, ParameterDirection.Input);
        //        par[4] = new OracleParameter("p_BPOType", OracleDbType.NVarchar2, BPOWiseOutstandingBillsModel.BpoType, ParameterDirection.Input);
        //        par[5] = new OracleParameter("p_BPORly", OracleDbType.NVarchar2, BPOWiseOutstandingBillsModel.BpoRly, ParameterDirection.Input);
        //        par[6] = new OracleParameter("p_BPORegion", OracleDbType.NVarchar2, BPOWiseOutstandingBillsModel.BpoRegion, ParameterDirection.Input);
        //        par[7] = new OracleParameter("p_BPOTypeFilter", OracleDbType.NVarchar2, "", ParameterDirection.Input);
        //        par[8] = new OracleParameter("p_TypeOutstanding", OracleDbType.NVarchar2, BPOWiseOutstandingBillsModel.TypeofOutStandingBills, ParameterDirection.Input);
        //        par[9] = new OracleParameter("p_ResultCursor", OracleDbType.RefCursor, ParameterDirection.Output);

        //        var ds = DataAccessDB.GetDataSet("BPO_OUTSTANDING_BILLS_REPORT", par, 9);
                
        ////            var columns = new string[]
        ////{
        ////    "BILL_NO", "BILL_DT", "BILL_AMOUNT", "BK_NO", "SET_NO", "TOTAL_TDS",
        ////    "RETENTION_MONEY", "CNOTE_AMOUNT", "WRITE_OFF_AMT", "AMOUNT_POSTED", "AMOUNT_REALISED", "AMOUNT_OUTSTADING",
        ////    "CASE_NO", "PO_NO", "PO_DT", "CONSIGNEE", "BPO", "SAP_CUST_CD_BPO", "VENDOR", "IE", "INVOICE_NO", "LO_REMARKS"
        ////};
        //        //byte[] fileContents;
        //        //return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Outstanding.xlsx");
        //        //List<LABREGISTERModel> modelList = new List<LABREGISTERModel>();
        //        //if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //        //{
        //        //    foreach (DataRow row in ds.Tables[0].Rows)
        //        //    {
        //        //        LABREGISTERModel model = new LABREGISTERModel
        //        //        {
        //        //            CHQ_NO = row["CHQ_NO"] as string,
        //        //            CHQ_DATE = Convert.ToString(row["CHQ_DATE"]),
        //        //            AMOUNT = Convert.ToString(row["AMOUNT"]),
        //        //            SUSPENSE_AMT = Convert.ToString(row["SUSPENSE_AMT"]),
        //        //            BANK_NAME = Convert.ToString(row["BANK_NAME"]),
        //        //            CASE_NO = Convert.ToString(row["CASE_NO"]),
        //        //            BANK_CD = Convert.ToString(row["BANK_CD"]),
        //        //            NARRATION = Convert.ToString(row["NARRATION"]),
        //        //        };

        //        //        modelList.Add(model);
        //        //    }
        //        //}

        //        return ds;
        //    }

        //    //return dTResult;
        //}
        public BPOWiseOutstandingBillsModel GenerateReport(string ReportType, string FromDt, string ToDt, string BpoCd, string BpoType, string BpoRly, string BpoRegion, Boolean Railway, Boolean PSU, Boolean StateGovt, Boolean ForeignRailways, Boolean PrivateSector, string TypeofOutStandingBills, string Region)
        {

            BPOWiseOutstandingBillsModel model = new();
            List<BPOWiseOutstandingBillsModel> lstlab = new();


            OracleParameter[] par = new OracleParameter[14];
            par[0] = new OracleParameter("p_RegionCode", OracleDbType.NVarchar2, Region, ParameterDirection.Input);
            par[1] = new OracleParameter("p_StartDate", OracleDbType.NVarchar2, FromDt, ParameterDirection.Input);
            par[2] = new OracleParameter("p_EndDate", OracleDbType.NVarchar2, ToDt, ParameterDirection.Input);
            par[3] = new OracleParameter("p_BPOCode", OracleDbType.NVarchar2, BpoCd, ParameterDirection.Input);
            par[4] = new OracleParameter("p_BPOType", OracleDbType.NVarchar2, BpoType, ParameterDirection.Input);
            par[5] = new OracleParameter("p_BPORly", OracleDbType.NVarchar2, BpoRly, ParameterDirection.Input);
            par[6] = new OracleParameter("p_BPORegion", OracleDbType.NVarchar2, BpoRegion, ParameterDirection.Input);
            par[7] = new OracleParameter("p_Railway", OracleDbType.Boolean, Railway, ParameterDirection.Input);
            par[8] = new OracleParameter("p_PSU", OracleDbType.Boolean, PSU, ParameterDirection.Input);
            par[9] = new OracleParameter("p_StateGov", OracleDbType.Boolean, StateGovt, ParameterDirection.Input);
            par[10] = new OracleParameter("p_FornRly", OracleDbType.Boolean, ForeignRailways, ParameterDirection.Input);
            par[11] = new OracleParameter("p_Private", OracleDbType.Boolean, PrivateSector, ParameterDirection.Input);
            par[12] = new OracleParameter("p_TypeOutstanding", OracleDbType.NVarchar2, TypeofOutStandingBills, ParameterDirection.Input);
            par[13] = new OracleParameter("p_ResultCursor", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("BPO_OUTSTANDING_BILLS_REPORT", par, 13);

            if (ds.Tables[0].Rows.Count != 0)
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                    lstlab = JsonConvert.DeserializeObject<List<BPOWiseOutstandingBillsModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });


                    model.BILL_NO = Convert.ToString(ds.Tables[0].Rows[0]["BILL_NO"]);
                    model.BILL_DT = Convert.ToString(ds.Tables[0].Rows[0]["BILL_DT"]);
                    model.BILL_AMOUNT = Convert.ToString(ds.Tables[0].Rows[0]["BILL_AMOUNT"]);
                    model.BK_NO = Convert.ToString(ds.Tables[0].Rows[0]["BK_NO"]);
                    model.SET_NO = Convert.ToString(ds.Tables[0].Rows[0]["SET_NO"]);
                    //model.TOTAL_TDS = Convert.ToString(ds.Tables[0].Rows[0]["TOTAL_TDS"]);
                    model.RETENTION_MONEY = Convert.ToString(ds.Tables[0].Rows[0]["RETENTION_MONEY"]);
                    model.WRITE_OFF_AMT = Convert.ToString(ds.Tables[0].Rows[0]["WRITE_OFF_AMT"]);
                    model.AMOUNT_POSTED = Convert.ToString(ds.Tables[0].Rows[0]["AMOUNT_POSTED"]);
                    model.AMOUNT_REALISED = Convert.ToString(ds.Tables[0].Rows[0]["AMOUNT_REALISED"]);
                    model.AMOUNT_OUTSTADING = Convert.ToString(ds.Tables[0].Rows[0]["AMOUNT_OUTSTADING"]);
                    model.CASE_NO = Convert.ToString(ds.Tables[0].Rows[0]["CASE_NO"]);
                    model.PO_NO = Convert.ToString(ds.Tables[0].Rows[0]["PO_NO"]);
                    model.PO_DT = Convert.ToString(ds.Tables[0].Rows[0]["PO_DT"]);
                    model.CONSIGNEE = Convert.ToString(ds.Tables[0].Rows[0]["CONSIGNEE"]);
                    model.BPO = Convert.ToString(ds.Tables[0].Rows[0]["BPO"]);
                    model.SAP_CUST_CD_BPO = Convert.ToString(ds.Tables[0].Rows[0]["SAP_CUST_CD_BPO"]);
                    model.VENDOR = Convert.ToString(ds.Tables[0].Rows[0]["VENDOR"]);
                    model.IE = Convert.ToString(ds.Tables[0].Rows[0]["IE"]);
                    model.INVOICE_NO = Convert.ToString(ds.Tables[0].Rows[0]["INVOICE_NO"]);
                    model.LO_REMARKS = Convert.ToString(ds.Tables[0].Rows[0]["LO_REMARKS"]);
                    model.I_TAX_TDS = Convert.ToString(ds.Tables[0].Rows[0]["I_TAX_TDS"]);
                    model.GST_TDS = Convert.ToString(ds.Tables[0].Rows[0]["GST_TDS"]);
                    model.PO_OR_LETTER = Convert.ToString(ds.Tables[0].Rows[0]["PO_OR_LETTER"]);
                    model.IC_NO = Convert.ToString(ds.Tables[0].Rows[0]["IC_NO"]);
                    model.IC_DT = Convert.ToString(ds.Tables[0].Rows[0]["IC_DT"]);
                    model.RECIPIENT_GSTIN_NO = Convert.ToString(ds.Tables[0].Rows[0]["RECIPIENT_GSTIN_NO"]);
                    model.CHQ_DT = Convert.ToString(ds.Tables[0].Rows[0]["CHQ_DT"]);
                    model.NARRATION = Convert.ToString(ds.Tables[0].Rows[0]["NARRATION"]);
                    model.Amount = Convert.ToString(ds.Tables[0].Rows[0]["CHQ_AMT"]);
                    model.CNOTE_AMOUNT = Convert.ToString(ds.Tables[0].Rows[0]["CNOTE_AMOUNT"]);
                    model.CNOTE_NUMBER = Convert.ToString(ds.Tables[0].Rows[0]["CNOTE_BILL_NO"]);
                    model.AU_DESC = Convert.ToString(ds.Tables[0].Rows[0]["AU_DESC"]);
                }
                model.lstBPOReport = lstlab;
            }
            return model;
        }
    }
}
