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
        public LabReportsModel LabRegisterReport(string ReportType, string wFrmDtO, string wToDt, string rdbIEWise, string rdbPIE, string rdbVendWise, string rdbPVend, string rdbLabWise, string rdbPLab, string rdbPending, string rdbPaid, string rdbDue, string rdbPartlyPaid, string lstTStatus, string lstIE, string ddlVender, string lstLab, string Regin)
        {

            LabReportsModel model = new();
            List<LabReportsModel> lstlab = new();


            OracleParameter[] par = new OracleParameter[18];
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
            par[17] = new OracleParameter("p_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("LabRegisterReport", par, 17);

            if (ds.Tables[0].Rows.Count != 0)
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                    lstlab = JsonConvert.DeserializeObject<List<LabReportsModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });


                    model.SAMPLE_REG_NO = Convert.ToString(ds.Tables[0].Rows[0]["SAMPLE_REG_NO"]);
                    model.SAMPLE_REG_DATE = Convert.ToString(ds.Tables[0].Rows[0]["SAMPLE_REG_DATE"]);
                    model.CASE_NO = Convert.ToString(ds.Tables[0].Rows[0]["CASE_NO"]);
                    model.CALL_RECV_DATE = Convert.ToString(ds.Tables[0].Rows[0]["CALL_RECV_DATE"]);
                    model.CALL_SNO = Convert.ToString(ds.Tables[0].Rows[0]["CALL_SNO"]);
                    model.T_TYPE = Convert.ToString(ds.Tables[0].Rows[0]["T_TYPE"]);
                    model.CODE_NO = Convert.ToString(ds.Tables[0].Rows[0]["CODE_NO"]);
                    model.CODE_DATE = Convert.ToString(ds.Tables[0].Rows[0]["CODE_DATE"]);
                    model.VENDOR = Convert.ToString(ds.Tables[0].Rows[0]["VENDOR"]);
                    model.IE_NAME = Convert.ToString(ds.Tables[0].Rows[0]["IE_NAME"]);
                    model.LAB = Convert.ToString(ds.Tables[0].Rows[0]["LAB"]);
                    model.TEST_REPORT_REC_DATE = Convert.ToString(ds.Tables[0].Rows[0]["TEST_REPORT_REC_DATE"]);
                    model.TEST_STATUS = Convert.ToString(ds.Tables[0].Rows[0]["TEST_STATUS"]);
                    model.TESTING_FEE = Convert.ToString(ds.Tables[0].Rows[0]["TESTING_FEE"]);
                    model.SERVICE_TAX = Convert.ToString(ds.Tables[0].Rows[0]["SERVICE_TAX"]);
                    model.HANDLING_CHARGES = Convert.ToString(ds.Tables[0].Rows[0]["HANDLING_CHARGES"]);
                    model.AMOUNT_RECIEVED = Convert.ToString(ds.Tables[0].Rows[0]["AMOUNT_RECIEVED"]);
                    model.TDS_AMT = Convert.ToString(ds.Tables[0].Rows[0]["TDS_AMT"]);
                    model.TDS_DATE = Convert.ToString(ds.Tables[0].Rows[0]["TDS_DATE"]);
                    model.AMT_DUE = Convert.ToString(ds.Tables[0].Rows[0]["AMT_DUE"]);
                    model.TEST = Convert.ToString(ds.Tables[0].Rows[0]["TEST"]);
                    model.ITEM_DESC = Convert.ToString(ds.Tables[0].Rows[0]["ITEM_DESC"]);
                    model.REMARKS = Convert.ToString(ds.Tables[0].Rows[0]["REMARKS"]);
                    model.SAMPLE_DISPATCH_DATE = Convert.ToString(ds.Tables[0].Rows[0]["SAMPLE_DISPATCH_DATE"]);
                }
                model.lstLabReport = lstlab;
            }
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

            if (ds.Tables[0].Rows.Count != 0)
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                    lstlab = JsonConvert.DeserializeObject<List<LabReportsModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                    model.LAB = Convert.ToString(ds.Tables[0].Rows[0]["LAB"]);
                    model.NO_OF_TEST = Convert.ToString(ds.Tables[0].Rows[0]["NO_OF_TEST"]);
                    model.NO_OF_SAMPLES = Convert.ToString(ds.Tables[0].Rows[0]["NO_OF_SAMPLES"]);
                    model.NO_OF_FAILURE = Convert.ToString(ds.Tables[0].Rows[0]["NO_OF_FAILURE"]);
                    model.NO_OF_FAIL_SAMPLES = Convert.ToString(ds.Tables[0].Rows[0]["NO_OF_FAIL_SAMPLES"]);
                    model.NO_OFNOCOMMENTS = Convert.ToString(ds.Tables[0].Rows[0]["NO_OFNOCOMMENTS"]);
                    model.MAXM_DAYS = Convert.ToString(ds.Tables[0].Rows[0]["MAXM_DAYS"]);
                    model.MIN_DAYS = Convert.ToString(ds.Tables[0].Rows[0]["MIN_DAYS"]);
                    model.AVG_DAYS = Convert.ToString(ds.Tables[0].Rows[0]["AVG_DAYS"]);
                    model.TOTAL_FEE = Convert.ToString(ds.Tables[0].Rows[0]["TOTAL_FEE"]);
                }
                model.lstLabReport = lstlab;
            }
            return model;
        }
        public LabReportsModel LabPostingReport(string ReportType, string wFrmDtO, string wToDt, string Regin)
        {

            LabReportsModel model = new();
            List<LabReportsModel> lstlab = new();


            OracleParameter[] par = new OracleParameter[4];
            par[0] = new OracleParameter("p_Region", OracleDbType.NVarchar2, Regin, ParameterDirection.Input);
            par[1] = new OracleParameter("p_FromDate", OracleDbType.Date, wFrmDtO, ParameterDirection.Input);
            par[2] = new OracleParameter("p_ToDate", OracleDbType.Date, wToDt, ParameterDirection.Input);
            par[3] = new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("LabRegisterReportPosting", par, 3);

            if (ds.Tables[0].Rows.Count != 0)
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                    lstlab = JsonConvert.DeserializeObject<List<LabReportsModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });


                    model.SAMPLE_REG_NO = Convert.ToString(ds.Tables[0].Rows[0]["SAMPLE_REG_NO"]);
                    model.SAMPLE_REG_DATE = Convert.ToString(ds.Tables[0].Rows[0]["SAMPLE_REG_DATE"]);
                    model.CASE_NO = Convert.ToString(ds.Tables[0].Rows[0]["CASE_NO"]);
                    model.CALL_RECV_DATE = Convert.ToString(ds.Tables[0].Rows[0]["CALL_RECV_DATE"]);
                    model.CALL_SNO = Convert.ToString(ds.Tables[0].Rows[0]["CALL_SNO"]);
                    model.VENDOR = Convert.ToString(ds.Tables[0].Rows[0]["VENDOR"]);
                    model.IE_NAME = Convert.ToString(ds.Tables[0].Rows[0]["IE_NAME"]);
                    model.AMOUNT_RECIEVED = Convert.ToString(ds.Tables[0].Rows[0]["AMOUNT_RECIEVED"]);
                    model.TOTAL_LAB_CHARGES = Convert.ToString(ds.Tables[0].Rows[0]["TOTAL_LAB_CHARGES"]);
                    model.TDS_AMT = Convert.ToString(ds.Tables[0].Rows[0]["TDS_AMT"]);
                    model.TDS_DATE = Convert.ToString(ds.Tables[0].Rows[0]["TDS_DATE"]);
                    model.AMT_DUE = Convert.ToString(ds.Tables[0].Rows[0]["AMT_DUE"]);
                    model.CHQ_NO = Convert.ToString(ds.Tables[0].Rows[0]["CHQ_NO"]);
                    model.CHQ_DATE = Convert.ToString(ds.Tables[0].Rows[0]["CHQ_DATE"]);
                    model.AMOUNT = Convert.ToString(ds.Tables[0].Rows[0]["AMOUNT"]);
                    model.CHQ_CASE_NO = Convert.ToString(ds.Tables[0].Rows[0]["CHQ_CASE_NO"]);
                    model.AMOUNT_ADJUSTED = Convert.ToString(ds.Tables[0].Rows[0]["AMOUNT_ADJUSTED"]);
                    model.SUSPENSE_AMT = Convert.ToString(ds.Tables[0].Rows[0]["SUSPENSE_AMT"]);
                }
                model.lstLabReport = lstlab;
            }
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

            if (ds.Tables[0].Rows.Count != 0)
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                    lstlab = JsonConvert.DeserializeObject<List<LabReportsModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });


                    model.MER_TXN_REF = Convert.ToString(ds.Tables[0].Rows[0]["MER_TXN_REF"]);
                    model.ORDER_INFO = Convert.ToString(ds.Tables[0].Rows[0]["ORDER_INFO"]);
                    model.TRANSACTION_NO = Convert.ToString(ds.Tables[0].Rows[0]["TRANSACTION_NO"]);
                    model.RRN_NO = Convert.ToString(ds.Tables[0].Rows[0]["RRN_NO"]);
                    model.AUTH_CD = Convert.ToString(ds.Tables[0].Rows[0]["AUTH_CD"]);
                    model.CASE_NO = Convert.ToString(ds.Tables[0].Rows[0]["CASE_NO"]);
                    model.CALL_DT = Convert.ToString(ds.Tables[0].Rows[0]["CALL_DT"]);
                    model.CALL_SNO = Convert.ToString(ds.Tables[0].Rows[0]["CALL_SNO"]);
                    model.VENDOR = Convert.ToString(ds.Tables[0].Rows[0]["VENDOR"]);
                    model.AMOUNT = Convert.ToString(ds.Tables[0].Rows[0]["AMOUNT"]);
                    model.TYPE = Convert.ToString(ds.Tables[0].Rows[0]["TYPE"]);
                    model.STATUS = Convert.ToString(ds.Tables[0].Rows[0]["STATUS"]);
                    model.DATETIME = Convert.ToString(ds.Tables[0].Rows[0]["DATETIME"]);
                }
                model.lstLabReport = lstlab;
            }
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

            if (ds.Tables[0].Rows.Count != 0)
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                    lstlab = JsonConvert.DeserializeObject<List<LabReportsModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                    model.BPO_NAME = ds.Tables[0].Rows[0]["BPO_NAME"].ToString();
                    model.recipient_gstin_no = ds.Tables[0].Rows[0]["recipient_gstin_no"].ToString();
                    model.St_cd = ds.Tables[0].Rows[0]["St_cd"].ToString();
                    model.invoice_no = ds.Tables[0].Rows[0]["invoice_no"].ToString();
                    model.invoice_dt = ds.Tables[0].Rows[0]["invoice_dt"].ToString();
                    model.Total_AMT = Convert.ToString(ds.Tables[0].Rows[0]["Total_AMT"]);
                    model.INV_TYPE = ds.Tables[0].Rows[0]["INV_TYPE"].ToString();
                    model.HSN_CD = ds.Tables[0].Rows[0]["HSN_CD"].ToString();
                    model.INV_amount = Convert.ToString(ds.Tables[0].Rows[0]["INV_amount"]);
                    model.INV_sgst = Convert.ToString(ds.Tables[0].Rows[0]["INV_sgst"]);
                    model.INV_cgst = Convert.ToString(ds.Tables[0].Rows[0]["INV_cgst"]);
                    model.INV_igst = Convert.ToString(ds.Tables[0].Rows[0]["INV_igst"]);
                    model.INVOICE_TYPE = ds.Tables[0].Rows[0]["INVOICE_TYPE"].ToString();
                    model.INC_TYPE = ds.Tables[0].Rows[0]["INC_TYPE"].ToString();
                    model.Total_GST = Convert.ToString(ds.Tables[0].Rows[0]["Total_GST"]);
                    model.IRN_NO = ds.Tables[0].Rows[0]["IRN_NO"].ToString();
                    model.BILL_FINALIZE = ds.Tables[0].Rows[0]["BILL_FINALIZE"].ToString();
                    model.BILL_SENT = ds.Tables[0].Rows[0]["BILL_SENT"].ToString();
                }
                model.lstLabReport = lstlab;
            }
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
        public LabReportsModel LabSamplePaymentReport(string ReportType, string wFrmDtO, string wToDt, string Regin,string lstStatus,string rdbrecvdt)
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

            if (ds.Tables[0].Rows.Count != 0)
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                    lstlab = JsonConvert.DeserializeObject<List<LabReportsModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });


                    model.call_recv_dt = Convert.ToString(ds.Tables[0].Rows[0]["call_recv_dt"]);
                    model.SAMPLE_REG_NO = Convert.ToString(ds.Tables[0].Rows[0]["sample_reg_no"]);
                    model.CASE_NO = Convert.ToString(ds.Tables[0].Rows[0]["case_no"]);
                    model.IE_NAME = Convert.ToString(ds.Tables[0].Rows[0]["ie_name"]);
                    model.vend_name = Convert.ToString(ds.Tables[0].Rows[0]["vend_name"]);
                    model.MFG_NAME = Convert.ToString(ds.Tables[0].Rows[0]["mfg_name"]);
                    model.likely_dt_report = Convert.ToString(ds.Tables[0].Rows[0]["likely_dt_report"]);
                    model.LAB_STATUS = Convert.ToString(ds.Tables[0].Rows[0]["LAB_STATUS"]);
                    model.testing_charges_by_lab = Convert.ToString(ds.Tables[0].Rows[0]["testing_charges_by_lab"]);
                    model.testing_charges_by_vendor = Convert.ToString(ds.Tables[0].Rows[0]["testing_charges_by_vendor"]);
                    model.tds_charges_by_vendor = Convert.ToString(ds.Tables[0].Rows[0]["tds_charges_by_vendor"]);
                    model.Vend_INIT_DT = Convert.ToString(ds.Tables[0].Rows[0]["Vend_INIT_DT"]);
                    model.UTR_NO = Convert.ToString(ds.Tables[0].Rows[0]["UTR_NO"]);
                    model.UTR_DATE = Convert.ToString(ds.Tables[0].Rows[0]["UTR_DATE"]);
                    model.doc_status_fin = Convert.ToString(ds.Tables[0].Rows[0]["doc_status_fin"]);
                    model.FIN_INIT_DT = Convert.ToString(ds.Tables[0].Rows[0]["FIN_INIT_DT"]);
                    model.REMARKS = Convert.ToString(ds.Tables[0].Rows[0]["REMARKS"]);
                    model.DOC_REJ_REMARK = Convert.ToString(ds.Tables[0].Rows[0]["DOC_REJ_REMARK"]);
                }
                model.lstLabReport = lstlab;
            }
            return model;
        }
    }
}
