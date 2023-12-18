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
using System.Dynamic;
using System.Globalization;
using System.Reflection.Emit;
using System.Threading.Tasks;
using static IBS.Helper.Enums;
using static System.Net.Mime.MediaTypeNames;

namespace IBS.Repositories
{
    public class LabReportsRRepository : ILabReportsRepository
    {
        private readonly ModelContext context;

        public LabReportsRRepository(ModelContext context)
        {
            this.context = context;
        }
        public LabReportsModel LabRegisterReport(string ReportType, string wFrmDtO, string wToDt, string rdbIEWise, string rdbPIE, string rdbVendWise, string rdbPVend, string rdbLabWise, string rdbPLab, string rdbPending, string rdbPaid, string rdbDue, string rdbPartlyPaid, string lstTStatus, string lstIE, string ddlVender, string lstLab, string Disciplinewise, string rdbPDis, string Discipline, string Regin)
        {
            string reg = "";
            LabReportsModel model = new();
            List<LabReportsModel> lstlab = new();
            List<LabReportsModel> lstsum = new();

            //model = calculate(wFrmDtO, wToDt, Regin);
            OracleParameter[] par = new OracleParameter[21];
            par[0] = new OracleParameter("p_region", OracleDbType.NVarchar2, Regin, ParameterDirection.Input);
            par[1] = new OracleParameter("p_wFrmDtO", OracleDbType.Date, wFrmDtO, ParameterDirection.Input);
            par[2] = new OracleParameter("p_wToDt", OracleDbType.Date, wToDt, ParameterDirection.Input);
            par[3] = new OracleParameter("p_rdbPending", OracleDbType.Boolean, rdbPending, ParameterDirection.Input);
            par[4] = new OracleParameter("p_rdbPaid", OracleDbType.Boolean, rdbPaid, ParameterDirection.Input);
            par[5] = new OracleParameter("p_rdbDue", OracleDbType.Boolean, rdbDue, ParameterDirection.Input);
            par[6] = new OracleParameter("p_rdbPartlyPaid", OracleDbType.Boolean, rdbPartlyPaid, ParameterDirection.Input);
            par[7] = new OracleParameter("p_lstTStatus", OracleDbType.NVarchar2, lstTStatus, ParameterDirection.Input);
            par[8] = new OracleParameter("p_rdbIEWise", OracleDbType.Boolean, rdbIEWise, ParameterDirection.Input);
            par[9] = new OracleParameter("p_rdbPIE", OracleDbType.Boolean, rdbPIE, ParameterDirection.Input);
            par[10] = new OracleParameter("p_lstIE", OracleDbType.NVarchar2, lstIE, ParameterDirection.Input);
            par[11] = new OracleParameter("p_rdbVendWise", OracleDbType.Boolean, rdbVendWise, ParameterDirection.Input);
            par[12] = new OracleParameter("p_rdbPVend", OracleDbType.Boolean, rdbPVend, ParameterDirection.Input);
            par[13] = new OracleParameter("p_ddlVender", OracleDbType.NVarchar2, ddlVender, ParameterDirection.Input);
            par[14] = new OracleParameter("p_rdbLabWise", OracleDbType.Boolean, rdbLabWise, ParameterDirection.Input);
            par[15] = new OracleParameter("p_rdbPLab", OracleDbType.Boolean, rdbPLab, ParameterDirection.Input);
            par[16] = new OracleParameter("p_lstLab", OracleDbType.NVarchar2, lstLab, ParameterDirection.Input);
            par[17] = new OracleParameter("p_Disciplinewise", OracleDbType.Boolean,Disciplinewise, ParameterDirection.Input);
            par[18] = new OracleParameter("p_rdbPDis", OracleDbType.Boolean, rdbPDis, ParameterDirection.Input);
            par[19] = new OracleParameter("p_Discipline", OracleDbType.NVarchar2, Discipline, ParameterDirection.Input);
            par[20] = new OracleParameter("p_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("LabRegisterReport", par, 20);
            //int totalTestingFee = Convert.ToInt32(par[17].Value);
            DataTable dt = ds.Tables[0];
            
            lstlab = dt.AsEnumerable().Select(row => new LabReportsModel
            {
                SAMPLE_REG_NO = row["SAMPLE_REG_NO"].ToString(),
                SAMPLE_REG_DATE = row["SAMPLE_REG_DATE"].ToString(),
                CASE_NO = row["CASE_NO"].ToString(),
                CALL_RECV_DATE = row["CALL_RECV_DATE"].ToString(),
                CALL_SNO = row["CALL_SNO"].ToString(),
                T_TYPE = row["T_TYPE"].ToString(),
                CODE_NO = row["CODE_NO"].ToString(),
                CODE_DATE = row["CODE_DATE"].ToString(),
                VENDOR = row["VENDOR"].ToString(),
                IE_NAME = row["IE_NAME"].ToString(),
                LAB = row["LAB"].ToString(),
                TEST_REPORT_REC_DATE = row["TEST_REPORT_REC_DATE"].ToString(),
                TEST_STATUS = row["TEST_STATUS"].ToString(),
                TESTING_FEE = row["TESTING_FEE"].ToString(),
                SERVICE_TAX = row["SERVICE_TAX"].ToString(),
                HANDLING_CHARGES = row["HANDLING_CHARGES"].ToString(),
                AMOUNT_RECIEVED = row["AMOUNT_RECIEVED"].ToString(),
                TDS_AMT = row["TDS_AMT"].ToString(),
                TDS_DATE = row["TDS_DATE"].ToString(),
                AMT_DUE = row["AMT_DUE"].ToString(),
                TEST = row["TEST"].ToString(),
                ITEM_DESC = row["ITEM_DESC"].ToString(),
                REMARKS = row["REMARKS"].ToString(),
                SAMPLE_DISPATCH_DATE = row["SAMPLE_DISPATCH_DATE"].ToString(),
                NO_OF_SAMPLES = row["QTY"].ToString(),

            }).ToList();
            foreach (var item in lstlab)
            {
                
                
                if (reg == item.SAMPLE_REG_NO)
                {
                    item.AMOUNT_RECIEVED = "";
                    item.TDS_AMT = "";
                    item.AMT_DUE = "";
                }
                reg = item.SAMPLE_REG_NO;
            }
            //lstlab = lstlab.Where(row => row.AMOUNT_RECIEVED != null).ToList();


            //lstlab = lstlab.GroupBy(row => row.SAMPLE_REG_NO)
            //               .Select(group => group.First())
            //               .ToList();
            //model.lstLabReport = lstlab;
            //var filteredList = lstlab.Where(row => row.AMOUNT_RECIEVED != null);


            //var grouped = filteredList.GroupBy(row => row.SAMPLE_REG_NO)
            //                          .Select(group => group.OrderBy(r => r.AMOUNT_RECIEVED != null ? 0 : 1)
            //                                               .ThenBy(r => r.AMOUNT_RECIEVED)
            //                                               .First())
            //                          .ToList();

            model.lstLabReport = lstlab;

            return model;
        }
        
        public LabReportsModel LabPerformanceReport(string ReportType, string wFrmDtO, string wToDt, string Regin)
        {

            LabReportsModel model = new();
            List<LabReportsModel> lstlab = new();


            OracleParameter[] par = new OracleParameter[3];
            par[0] = new OracleParameter("wFrmDt", OracleDbType.Varchar2, wFrmDtO, ParameterDirection.Input);
            par[1] = new OracleParameter("wToDt", OracleDbType.Varchar2, wToDt, ParameterDirection.Input);
            par[2] = new OracleParameter("result_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("Lab_Performance_Report", par, 2);
            DataTable dt = ds.Tables[0];
            lstlab = dt.AsEnumerable().Select(row => new LabReportsModel
            {
                LAB = row["LAB"].ToString(),
                NO_OF_TEST = row["NO_OF_TEST"].ToString(),
                NO_OF_SAMPLES = row["NO_OF_SAMPLES"].ToString(),
                NO_OF_FAILURE = row["NO_OF_FAILURE"].ToString(),
                NO_OF_FAIL_SAMPLES = row["NO_OF_FAIL_SAMPLES"].ToString(),
                NO_OFNOCOMMENTS = row["NO_OFNOCOMMENTS"].ToString(),
                MAXM_DAYS = row["MAXM_DAYS"].ToString(),
                MIN_DAYS = row["MIN_DAYS"].ToString(),
                AVG_DAYS = row["AVG_DAYS"].ToString(),
                TOTAL_FEE = row["TOTAL_FEE"].ToString(),
            }).ToList();
            model.lstLabReport = lstlab;
            //if (ds.Tables[0].Rows.Count != 0)
            //{
            //    if (ds != null && ds.Tables.Count > 0)
            //    {
            //        string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
            //        lstlab = JsonConvert.DeserializeObject<List<LabReportsModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            //        model.LAB = Convert.ToString(ds.Tables[0].Rows[0]["LAB"]);
            //        model.NO_OF_TEST = Convert.ToString(ds.Tables[0].Rows[0]["NO_OF_TEST"]);
            //        model.NO_OF_SAMPLES = Convert.ToString(ds.Tables[0].Rows[0]["NO_OF_SAMPLES"]);
            //        model.NO_OF_FAILURE = Convert.ToString(ds.Tables[0].Rows[0]["NO_OF_FAILURE"]);
            //        model.NO_OF_FAIL_SAMPLES = Convert.ToString(ds.Tables[0].Rows[0]["NO_OF_FAIL_SAMPLES"]);
            //        model.NO_OFNOCOMMENTS = Convert.ToString(ds.Tables[0].Rows[0]["NO_OFNOCOMMENTS"]);
            //        model.MAXM_DAYS = Convert.ToString(ds.Tables[0].Rows[0]["MAXM_DAYS"]);
            //        model.MIN_DAYS = Convert.ToString(ds.Tables[0].Rows[0]["MIN_DAYS"]);
            //        model.AVG_DAYS = Convert.ToString(ds.Tables[0].Rows[0]["AVG_DAYS"]);
            //        model.TOTAL_FEE = Convert.ToString(ds.Tables[0].Rows[0]["TOTAL_FEE"]);
            //    }
            //    model.lstLabReport = lstlab;
            //}
            return model;
        }
        public LabReportsModel LabPostingReport(string ReportType, string wFrmDtO, string wToDt, string Regin)
        {
            var from = Convert.ToDateTime(wFrmDtO).ToString("MM/dd/yyyy");
            var to = Convert.ToDateTime(wToDt).ToString("MM/dd/yyyy");
            LabReportsModel model = new();
            List<LabReportsModel> lstlab = new();


            OracleParameter[] par = new OracleParameter[4];
            par[0] = new OracleParameter("p_Region", OracleDbType.NVarchar2, Regin, ParameterDirection.Input);
            par[1] = new OracleParameter("p_FromDate", OracleDbType.Date, from, ParameterDirection.Input);
            par[2] = new OracleParameter("p_ToDate", OracleDbType.Date, to, ParameterDirection.Input);
            par[3] = new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("LabRegisterReportPosting", par, 3);
            DataTable dt = ds.Tables[0];
            lstlab = dt.AsEnumerable().Select(row => new LabReportsModel
            {
                SAMPLE_REG_NO = Convert.ToString(row["SAMPLE_REG_NO"]),
                SAMPLE_REG_DATE = Convert.ToString(row["SAMPLE_REG_DATE"]),
                CASE_NO = Convert.ToString(row["CASE_NO"]),
                CALL_RECV_DATE = Convert.ToString(row["CALL_RECV_DATE"]),
                CALL_SNO = Convert.ToString(row["CALL_SNO"]),
                VENDOR = Convert.ToString(row["VENDOR"]),
                IE_NAME = Convert.ToString(row["IE_NAME"]),
                AMOUNT_RECIEVED = Convert.ToString(row["AMOUNT_RECIEVED"]),
                TOTAL_LAB_CHARGES = Convert.ToString(row["TOTAL_LAB_CHARGES"]),
                TDS_AMT = Convert.ToString(row["TDS_AMT"]),
                TDS_DATE = Convert.ToString(row["TDS_DATE"]),
                AMT_DUE = Convert.ToString(row["AMT_DUE"]),
                CHQ_NO = Convert.ToString(row["CHQ_NO"]),
                CHQ_DATE = Convert.ToString(row["CHQ_DATE"]),
                AMOUNT = Convert.ToString(row["AMOUNT"]),
                CHQ_CASE_NO = Convert.ToString(row["CHQ_CASE_NO"]),
                AMOUNT_ADJUSTED = Convert.ToString(row["AMOUNT_ADJUSTED"]),
                SUSPENSE_AMT = Convert.ToString(row["SUSPENSE_AMT"]),
        }).ToList();
            model.lstLabReport = lstlab;
            //if (ds.Tables[0].Rows.Count != 0)
            //{
            //    if (ds != null && ds.Tables.Count > 0)
            //    {
            //        string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
            //        lstlab = JsonConvert.DeserializeObject<List<LabReportsModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });


            //        model.SAMPLE_REG_NO = Convert.ToString(ds.Tables[0].Rows[0]["SAMPLE_REG_NO"]);
            //        model.SAMPLE_REG_DATE = Convert.ToString(ds.Tables[0].Rows[0]["SAMPLE_REG_DATE"]);
            //        model.CASE_NO = Convert.ToString(ds.Tables[0].Rows[0]["CASE_NO"]);
            //        model.CALL_RECV_DATE = Convert.ToString(ds.Tables[0].Rows[0]["CALL_RECV_DATE"]);
            //        model.CALL_SNO = Convert.ToString(ds.Tables[0].Rows[0]["CALL_SNO"]);
            //        model.VENDOR = Convert.ToString(ds.Tables[0].Rows[0]["VENDOR"]);
            //        model.IE_NAME = Convert.ToString(ds.Tables[0].Rows[0]["IE_NAME"]);
            //        model.AMOUNT_RECIEVED = Convert.ToString(ds.Tables[0].Rows[0]["AMOUNT_RECIEVED"]);
            //        model.TOTAL_LAB_CHARGES = Convert.ToString(ds.Tables[0].Rows[0]["TOTAL_LAB_CHARGES"]);
            //        model.TDS_AMT = Convert.ToString(ds.Tables[0].Rows[0]["TDS_AMT"]);
            //        model.TDS_DATE = Convert.ToString(ds.Tables[0].Rows[0]["TDS_DATE"]);
            //        model.AMT_DUE = Convert.ToString(ds.Tables[0].Rows[0]["AMT_DUE"]);
            //        model.CHQ_NO = Convert.ToString(ds.Tables[0].Rows[0]["CHQ_NO"]);
            //        model.CHQ_DATE = Convert.ToString(ds.Tables[0].Rows[0]["CHQ_DATE"]);
            //        model.AMOUNT = Convert.ToString(ds.Tables[0].Rows[0]["AMOUNT"]);
            //        model.CHQ_CASE_NO = Convert.ToString(ds.Tables[0].Rows[0]["CHQ_CASE_NO"]);
            //        model.AMOUNT_ADJUSTED = Convert.ToString(ds.Tables[0].Rows[0]["AMOUNT_ADJUSTED"]);
            //        model.SUSPENSE_AMT = Convert.ToString(ds.Tables[0].Rows[0]["SUSPENSE_AMT"]);
            //    }
            //    model.lstLabReport = lstlab;
            //}
            return model;
        }
        public LabReportsModel OnlinePaymentReport(string ReportType, string wFrmDtO, string wToDt, string Regin)
        {

            LabReportsModel model = new();
            List<LabReportsModel> lstlab = new();


            OracleParameter[] par = new OracleParameter[4];
            par[0] = new OracleParameter("wDtFr", OracleDbType.NVarchar2, wFrmDtO, ParameterDirection.Input);
            par[1] = new OracleParameter("wDtTo", OracleDbType.NVarchar2, wToDt, ParameterDirection.Input);
            par[2] = new OracleParameter("region", OracleDbType.NVarchar2, Regin, ParameterDirection.Input);
            par[3] = new OracleParameter("cur", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GetOnlinePaymentReport", par, 3);
            DataTable dt = ds.Tables[0];
            lstlab = dt.AsEnumerable().Select(row => new LabReportsModel
            {
                MER_TXN_REF = Convert.ToString(row["MER_TXN_REF"]),
                ORDER_INFO = Convert.ToString(row["ORDER_INFO"]),
                TRANSACTION_NO = Convert.ToString(row["TRANSACTION_NO"]),
                RRN_NO = Convert.ToString(row["RRN_NO"]),
                AUTH_CD = Convert.ToString(row["AUTH_CD"]),
                CASE_NO = Convert.ToString(row["CASE_NO"]),
                CALL_DT = Convert.ToString(row["CALL_DT"]),
                CALL_SNO = Convert.ToString(row["CALL_SNO"]),
                VENDOR = Convert.ToString(row["VENDOR"]),
                AMOUNT = Convert.ToString(row["AMOUNT"]),
                TYPE = Convert.ToString(row["TYPE"]),
                STATUS = Convert.ToString(row["STATUS"]),
                DATETIME = Convert.ToString(row["DATETIME"]),
        }).ToList();
            model.lstLabReport = lstlab;
            //if (ds.Tables[0].Rows.Count != 0)
            //{
            //    if (ds != null && ds.Tables.Count > 0)
            //    {
            //        string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
            //        lstlab = JsonConvert.DeserializeObject<List<LabReportsModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });


            //        model.MER_TXN_REF = Convert.ToString(ds.Tables[0].Rows[0]["MER_TXN_REF"]);
            //        model.ORDER_INFO = Convert.ToString(ds.Tables[0].Rows[0]["ORDER_INFO"]);
            //        model.TRANSACTION_NO = Convert.ToString(ds.Tables[0].Rows[0]["TRANSACTION_NO"]);
            //        model.RRN_NO = Convert.ToString(ds.Tables[0].Rows[0]["RRN_NO"]);
            //        model.AUTH_CD = Convert.ToString(ds.Tables[0].Rows[0]["AUTH_CD"]);
            //        model.CASE_NO = Convert.ToString(ds.Tables[0].Rows[0]["CASE_NO"]);
            //        model.CALL_DT = Convert.ToString(ds.Tables[0].Rows[0]["CALL_DT"]);
            //        model.CALL_SNO = Convert.ToString(ds.Tables[0].Rows[0]["CALL_SNO"]);
            //        model.VENDOR = Convert.ToString(ds.Tables[0].Rows[0]["VENDOR"]);
            //        model.AMOUNT = Convert.ToString(ds.Tables[0].Rows[0]["AMOUNT"]);
            //        model.TYPE = Convert.ToString(ds.Tables[0].Rows[0]["TYPE"]);
            //        model.STATUS = Convert.ToString(ds.Tables[0].Rows[0]["STATUS"]);
            //        model.DATETIME = Convert.ToString(ds.Tables[0].Rows[0]["DATETIME"]);
            //    }
            //    model.lstLabReport = lstlab;
            //}
            return model;
        }
        public LabReportsModel LabInvoiceReport(string ReportType, string wFrmDtO, string wToDt, string Regin)
        {

            LabReportsModel model = new();
            List<LabReportsModel> lstlab = new();


            OracleParameter[] par = new OracleParameter[5];
            par[0] = new OracleParameter("wFrmDt", OracleDbType.NVarchar2, wFrmDtO, ParameterDirection.Input);
            par[1] = new OracleParameter("wToDt", OracleDbType.NVarchar2, wToDt, ParameterDirection.Input);
            par[2] = new OracleParameter("region", OracleDbType.NVarchar2, Regin, ParameterDirection.Input);
            par[3] = new OracleParameter("cur", OracleDbType.RefCursor, ParameterDirection.Output);
            par[4] = new OracleParameter("CUR_Two", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GetLabInvoiceReport", par, 4);
            DataTable dt = ds.Tables[0];
            lstlab = dt.AsEnumerable().Select(row => new LabReportsModel
            {
                 BPO_NAME = row["BPO_NAME"].ToString(),
                 recipient_gstin_no = row["recipient_gstin_no"].ToString(),
                 St_cd = row["St_cd"].ToString(),
                 invoice_no = row["invoice_no"].ToString(),
                 invoice_dt = row["invoice_dt"].ToString(),
                 Total_AMT = Convert.ToString(row["Total_AMT"]),
                 INV_TYPE = row["INV_TYPE"].ToString(),
                 HSN_CD = row["HSN_CD"].ToString(),
                 INV_amount = Convert.ToString(row["INV_amount"]),
                 INV_sgst = Convert.ToString(row["INV_sgst"]),
                 INV_cgst = Convert.ToString(row["INV_cgst"]),
                 INV_igst = Convert.ToString(row["INV_igst"]),
                 INVOICE_TYPE = row["INVOICE_TYPE"].ToString(),
                 INC_TYPE = row["INC_TYPE"].ToString(),
                 Total_GST = Convert.ToString(row["Total_GST"]),
                 IRN_NO = row["IRN_NO"].ToString(),
                 BILL_FINALIZE = row["BILL_FINALIZE"].ToString(),
                 BILL_SENT = row["BILL_SENT"].ToString(),
        }).ToList();
            model.lstLabReport = lstlab;
            //if (ds.Tables[0].Rows.Count != 0)
            //{
            //    if (ds != null && ds.Tables.Count > 0)
            //    {
            //        string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
            //        lstlab = JsonConvert.DeserializeObject<List<LabReportsModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            //        model.BPO_NAME = ds.Tables[0].Rows[0]["BPO_NAME"].ToString();
            //        model.recipient_gstin_no = ds.Tables[0].Rows[0]["recipient_gstin_no"].ToString();
            //        model.St_cd = ds.Tables[0].Rows[0]["St_cd"].ToString();
            //        model.invoice_no = ds.Tables[0].Rows[0]["invoice_no"].ToString();
            //        model.invoice_dt = ds.Tables[0].Rows[0]["invoice_dt"].ToString();
            //        model.Total_AMT = Convert.ToString(ds.Tables[0].Rows[0]["Total_AMT"]);
            //        model.INV_TYPE = ds.Tables[0].Rows[0]["INV_TYPE"].ToString();
            //        model.HSN_CD = ds.Tables[0].Rows[0]["HSN_CD"].ToString();
            //        model.INV_amount = Convert.ToString(ds.Tables[0].Rows[0]["INV_amount"]);
            //        model.INV_sgst = Convert.ToString(ds.Tables[0].Rows[0]["INV_sgst"]);
            //        model.INV_cgst = Convert.ToString(ds.Tables[0].Rows[0]["INV_cgst"]);
            //        model.INV_igst = Convert.ToString(ds.Tables[0].Rows[0]["INV_igst"]);
            //        model.INVOICE_TYPE = ds.Tables[0].Rows[0]["INVOICE_TYPE"].ToString();
            //        model.INC_TYPE = ds.Tables[0].Rows[0]["INC_TYPE"].ToString();
            //        model.Total_GST = Convert.ToString(ds.Tables[0].Rows[0]["Total_GST"]);
            //        model.IRN_NO = ds.Tables[0].Rows[0]["IRN_NO"].ToString();
            //        model.BILL_FINALIZE = ds.Tables[0].Rows[0]["BILL_FINALIZE"].ToString();
            //        model.BILL_SENT = ds.Tables[0].Rows[0]["BILL_SENT"].ToString();
            //    }
            //    model.lstLabReport = lstlab;
            //}
            LabReportsModel LabReportsModel = new();
            if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
            {

                DataRow row = ds.Tables[1].Rows[0];
                LabReportsModel = new LabReportsModel
                {
                    SumAMT = Convert.ToString(row["SumAMT"]),
                    Sumamount = Convert.ToString(row["Sumamount"]),
                    Sumsgst = Convert.ToString(row["Sumsgst"]),
                    Sumcgst = Convert.ToString(row["Sumcgst"]),
                    Sumigst = Convert.ToString(row["Sumigst"]),
                    SumGST = Convert.ToString(row["SumGST"])
                };

            }
            model.SumAMT = LabReportsModel.SumAMT;
            model.Sumamount = LabReportsModel.Sumamount;
            model.Sumsgst = LabReportsModel.Sumsgst;
            model.Sumcgst = LabReportsModel.Sumcgst;
            model.Sumigst = LabReportsModel.Sumigst;
            model.SumGST = LabReportsModel.SumGST;
            return model;
        }
        string dateconcate20(string dt)
        {
            string myYear, myMonth, myDay;

            myYear = dt.Substring(0, 4);
            myMonth = dt.Substring(4, 2);
            myDay = dt.Substring(6, 2);
            string dt1 = myYear + myDay + myMonth;
            return (dt1);
        }
        string dateconcate21(string dt)
        {
            string myYear, myMonth, myDay;

            myYear = dt.Substring(0, 4);
            myMonth = dt.Substring(4, 2);
            myDay = dt.Substring(6, 2);
            string dt1 = myYear + myMonth + myDay;
            return (dt1);
        }
        public LabReportsModel LabSamplePaymentReport(string ReportType, string wFrmDtO, string wToDt, string Regin, string lstStatus, string rdbrecvdt)
        {

            LabReportsModel model = new();
            List<LabReportsModel> lstlab = new();


            OracleParameter[] par = new OracleParameter[6];
            par[0] = new OracleParameter("p_lstStatus", OracleDbType.NVarchar2, lstStatus, ParameterDirection.Input);
            par[1] = new OracleParameter("p_rdbrecvdt", OracleDbType.NVarchar2, rdbrecvdt, ParameterDirection.Input);
            par[2] = new OracleParameter("p_frmDt", OracleDbType.NVarchar2, wFrmDtO, ParameterDirection.Input);
            par[3] = new OracleParameter("p_toDt", OracleDbType.NVarchar2, wToDt, ParameterDirection.Input);
            par[4] = new OracleParameter("p_REGION", OracleDbType.NVarchar2, Regin, ParameterDirection.Input);
            par[5] = new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GET_PaymentReport", par, 5);
            DataTable dt = ds.Tables[0];
            foreach (DataRow row in dt.Rows)
            {
                var caseno = "";
                var callsno = "";
                var calldocdt = "";

                caseno = row["case_no"].ToString(); 
                callsno = row["call_sno"].ToString(); 
                calldocdt = row["call_doc_dt"].ToString(); 

                string fn = "", MyFile = "", MyFile2 = "";
                string mdt = dateconcate20(calldocdt.Trim());
                string mdt2 = dateconcate21(calldocdt.Trim());

                MyFile = $"{caseno.Trim()}_{callsno.Trim()}_{mdt}";
                MyFile2 = $"{caseno.Trim()}_{callsno.Trim()}_{mdt2}";

                //fn = Path.GetFileName(filename);
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ReadWriteData", "LAB", "PReciept", $"{MyFile2}.PDF");
                string filePath2 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ReadWriteData", "LAB", $"{MyFile}.PDF");
                bool docExists = File.Exists(filePath);
                bool docExists2 = File.Exists(filePath2);

                LabReportsModel labReport = new LabReportsModel
                {
                    call_recv_dt = Convert.ToString(row["call_recv_dt"]),
                    SAMPLE_REG_NO = Convert.ToString(row["sample_reg_no"]),
                    CASE_NO = Convert.ToString(row["case_no"]),
                    IE_NAME = Convert.ToString(row["ie_name"]),
                    vend_name = Convert.ToString(row["vend_name"]),
                    MFG_NAME = Convert.ToString(row["mfg_name"]),
                    likely_dt_report = Convert.ToString(row["likely_dt_report"]),
                    LAB_STATUS = Convert.ToString(row["LAB_STATUS"]),
                    testing_charges_by_lab = Convert.ToString(row["testing_charges_by_lab"]),
                    testing_charges_by_vendor = Convert.ToString(row["testing_charges_by_vendor"]),
                    tds_charges_by_vendor = Convert.ToString(row["tds_charges_by_vendor"]),
                    Vend_INIT_DT = Convert.ToString(row["Vend_INIT_DT"]),
                    UTR_NO = Convert.ToString(row["UTR_NO"]),
                    UTR_DATE = Convert.ToString(row["UTR_DATE"]),
                    doc_status_fin = Convert.ToString(row["doc_status_fin"]),
                    FIN_INIT_DT = Convert.ToString(row["FIN_INIT_DT"]),
                    REMARKS = Convert.ToString(row["REMARKS"]),
                    DOC_REJ_REMARK = Convert.ToString(row["DOC_REJ_REMARK"]),
                    PDoc = docExists,
                    LABDoc = docExists2,
                    CALL_SNO = Convert.ToString(row["call_sno"]),
                    CallDocDate = Convert.ToString(row["call_doc_dt"]),
                    File = filePath,
                    File2 = filePath2,
                };
                lstlab.Add(labReport);
                model.lstLabReport = lstlab;
            }
            //lstlab = dt.AsEnumerable().Select(row => new LabReportsModel
            //{
            //    call_recv_dt = Convert.ToString(row["call_recv_dt"]),
            //    SAMPLE_REG_NO = Convert.ToString(row["sample_reg_no"]),
            //    CASE_NO = Convert.ToString(row["case_no"]),
            //    IE_NAME = Convert.ToString(row["ie_name"]),
            //    vend_name = Convert.ToString(row["vend_name"]),
            //    MFG_NAME = Convert.ToString(row["mfg_name"]),
            //    likely_dt_report = Convert.ToString(row["likely_dt_report"]),
            //    LAB_STATUS = Convert.ToString(row["LAB_STATUS"]),
            //    testing_charges_by_lab = Convert.ToString(row["testing_charges_by_lab"]),
            //    testing_charges_by_vendor = Convert.ToString(row["testing_charges_by_vendor"]),
            //    tds_charges_by_vendor = Convert.ToString(row["tds_charges_by_vendor"]),
            //    Vend_INIT_DT = Convert.ToString(row["Vend_INIT_DT"]),
            //    UTR_NO = Convert.ToString(row["UTR_NO"]),
            //    UTR_DATE = Convert.ToString(row["UTR_DATE"]),
            //    doc_status_fin = Convert.ToString(row["doc_status_fin"]),
            //    FIN_INIT_DT = Convert.ToString(row["FIN_INIT_DT"]),
            //    REMARKS = Convert.ToString(row["REMARKS"]),
            //    DOC_REJ_REMARK = Convert.ToString(row["DOC_REJ_REMARK"]),
            //}).ToList();
            //model.lstLabReport = lstlab;

            //if (ds.Tables[0].Rows.Count != 0)
            //{
            //    if (ds != null && ds.Tables.Count > 0)
            //    {
            //        string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
            //        lstlab = JsonConvert.DeserializeObject<List<LabReportsModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });


            //        model.call_recv_dt = Convert.ToString(ds.Tables[0].Rows[0]["call_recv_dt"]);
            //        model.SAMPLE_REG_NO = Convert.ToString(ds.Tables[0].Rows[0]["sample_reg_no"]);
            //        model.CASE_NO = Convert.ToString(ds.Tables[0].Rows[0]["case_no"]);
            //        model.IE_NAME = Convert.ToString(ds.Tables[0].Rows[0]["ie_name"]);
            //        model.vend_name = Convert.ToString(ds.Tables[0].Rows[0]["vend_name"]);
            //        model.MFG_NAME = Convert.ToString(ds.Tables[0].Rows[0]["mfg_name"]);
            //        model.likely_dt_report = Convert.ToString(ds.Tables[0].Rows[0]["likely_dt_report"]);
            //        model.LAB_STATUS = Convert.ToString(ds.Tables[0].Rows[0]["LAB_STATUS"]);
            //        model.testing_charges_by_lab = Convert.ToString(ds.Tables[0].Rows[0]["testing_charges_by_lab"]);
            //        model.testing_charges_by_vendor = Convert.ToString(ds.Tables[0].Rows[0]["testing_charges_by_vendor"]);
            //        model.tds_charges_by_vendor = Convert.ToString(ds.Tables[0].Rows[0]["tds_charges_by_vendor"]);
            //        model.Vend_INIT_DT = Convert.ToString(ds.Tables[0].Rows[0]["Vend_INIT_DT"]);
            //        model.UTR_NO = Convert.ToString(ds.Tables[0].Rows[0]["UTR_NO"]);
            //        model.UTR_DATE = Convert.ToString(ds.Tables[0].Rows[0]["UTR_DATE"]);
            //        model.doc_status_fin = Convert.ToString(ds.Tables[0].Rows[0]["doc_status_fin"]);
            //        model.FIN_INIT_DT = Convert.ToString(ds.Tables[0].Rows[0]["FIN_INIT_DT"]);
            //        model.REMARKS = Convert.ToString(ds.Tables[0].Rows[0]["REMARKS"]);
            //        model.DOC_REJ_REMARK = Convert.ToString(ds.Tables[0].Rows[0]["DOC_REJ_REMARK"]);
            //    }
            //    model.lstLabReport = lstlab;
            //}
            return model;
        }
        public LabReportsModel BarcodeReport(string ReportType, string wFrmDtO, string wToDt, string Regin)
        {

            LabReportsModel model = new();
            List<LabReportsModel> lstlab = new();


            OracleParameter[] par = new OracleParameter[3];
            par[0] = new OracleParameter("wDtFr", OracleDbType.NVarchar2, wFrmDtO, ParameterDirection.Input);
            par[1] = new OracleParameter("wDtTo", OracleDbType.NVarchar2, wToDt, ParameterDirection.Input);
            par[2] = new OracleParameter("cur", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("BARCODE_GENERATED_REPORT", par, 2);
            DataTable dt = ds.Tables[0];
            lstlab = dt.AsEnumerable().Select(row => new LabReportsModel
            {
                barcode_no = row["barcode_no"].ToString(),
                total_qty = row["total_qty"].ToString(),
                CASE_NO = row["case_no"].ToString(),
                createddate = row["createddate"].ToString(),
                INSPECTOR_CUSTOMER = row["INSPECTOR_CUSTOMER"].ToString(),
                DESCRIPTION = Convert.ToString(row["DESCRIPTION"]),
                SEALING_TYPE = row["SEALING_TYPE"].ToString(),
                RATE = row["RATE"].ToString(),
                TARGETED_DATE = row["TARGETED_DATE"].ToString(),
                RTAX = row["RTAX"].ToString(),

            }).ToList();
            model.lstBarcodeReport = lstlab;
            
            return model;
        }
    }
}
