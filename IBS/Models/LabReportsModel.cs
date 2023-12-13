using IBS.DataAccess;
using IBS.Models.Reports;

namespace IBS.Models
{
    public class LabReportsModel
    {
        public string ReportType { get; set; }
        public string ReportTitle { get; set; }
        public string SumGST { get; set; }
        public string SumAMT { get; set; }
        public string Sumamount { get; set; }
        public string Sumsgst { get; set; }
        public string Sumcgst { get; set; }
        public string Sumigst { get; set; }
        public string lstStatus { get; set; }
        public string rdbrecvdt { get; set; }
        public string SAMPLE_REG_DATE { get; set; }
        public string AMOUNT_RECIEVED { get; set; }
        public string TOTAL_LAB_CHARGES { get; set; }
        public string TDS_AMT { get; set; }
        public string TDS_DATE { get; set; }
        public string AMT_DUE { get; set; }
        public string CHQ_NO { get; set; }
        public string CHQ_DATE { get; set; }
        public string CHQ_CASE_NO { get; set; }
        public string AMOUNT_ADJUSTED { get; set; }
        public string SUSPENSE_AMT { get; set; }
        public string T_TYPE { get; set; }
        public string CODE_NO { get; set; }
        public string CODE_DATE { get; set; }
        public string TEST_REPORT_REC_DATE { get; set; }
        public string TEST_STATUS { get; set; }
        public string TESTING_FEE { get; set; }
        public string SERVICE_TAX { get; set; }
        public string HANDLING_CHARGES { get; set; }
        public string TEST { get; set; }
        public string ITEM_DESC { get; set; }
        public string REMARKS { get; set; }
        public string SAMPLE_DISPATCH_DATE { get; set; }
        public string Region { get; set; }
        public int SumTesting { get; set; }
        public int SumService { get; set; }
        public int SumHandling { get; set; }
        public int SumAmtR { get; set; }
        public int SumTDS { get; set; }
        public int SumAMtD { get; set; }
        public int SumT { get; set; }
        public int SumS { get; set; }
        public int SumH { get; set; }
        public int SumAR { get; set; }
        public int SumTds { get; set; }
        public int SumAD { get; set; }

        public string LAB { get; set; }
        public string NO_OF_TEST { get; set; }
        public string NO_OF_SAMPLES { get; set; }
        public string NO_OF_FAILURE { get; set; }
        public string NO_OF_FAIL_SAMPLES { get; set; }
        public string NO_OFNOCOMMENTS { get; set; }
        public string MAXM_DAYS { get; set; }
        public string MIN_DAYS { get; set; }
        public string AVG_DAYS { get; set; }
        public string TOTAL_FEE { get; set; }

        public string MER_TXN_REF { get; set; }
        public string ORDER_INFO { get; set; }
        public string TRANSACTION_NO { get; set; }
        public string RRN_NO { get; set; }
        public string AUTH_CD { get; set; }
        public string CASE_NO { get; set; }
        public string CALL_DT { get; set; }
        public string CALL_SNO { get; set; }
        public string VENDOR { get; set; }
        public string AMOUNT { get; set; }
        public string TYPE { get; set; }
        public string STATUS { get; set; }
        public string DATETIME { get; set; }

        public string BPO_NAME { get; set; }
        public string recipient_gstin_no { get; set; }
        public string St_cd { get; set; }
        public string invoice_no { get; set; }
        public string invoice_dt { get; set; }
        public string Total_AMT { get; set; }
        public string INV_TYPE { get; set; }
        public string HSN_CD { get; set; }
        public string INV_amount { get; set; }
        public string INV_sgst { get; set; }
        public string INV_cgst { get; set; }
        public string INV_igst { get; set; }
        public string INVOICE_TYPE { get; set; }
        public string INC_TYPE { get; set; }
        public string Total_GST { get; set; }
        public string IRN_NO { get; set; }
        public string BILL_FINALIZE { get; set; }
        public string BILL_SENT { get; set; }

        public string ReportStatus { get; set; }
        public string CALL_RECV_DATE { get; set; }
        public string SAMPLE_REG_NO { get; set; }
        public string vend_name { get; set; }
        public string MFG_NAME { get; set; }
        public string IE_NAME { get; set; }
        public string LAB_STATUS { get; set; }
        public string DOC_REJ_REMARK { get; set; }
        public string likely_dt_report { get; set; }
        public string testing_charges_by_lab { get; set; }
        public string testing_charges_by_vendor { get; set; }
        public string tds_charges_by_vendor { get; set; }
        public string CallDocDate { get; set; }
        public string File { get; set; }
        public string File2 { get; set; }
        public string doc_status_fin { get; set; }
        public string Vend_INIT_DT { get; set; }
        public string FIN_INIT_DT { get; set; }
        public string call_recv_dt { get; set; }
        public string UTR_NO { get; set; }
        public string UTR_DATE { get; set; }
        public bool PDoc { get; set; }
        public bool LABDoc { get; set; }

        public string wFrmDtO { get; set; }
        public string wToDt { get; set; }
        public string rdbIEWise { get; set; }
        public string rdbPIE { get; set; }
        public string rdbVendWise { get; set; }
        public string rdbPVend { get; set; }
        public string rdbLabWise { get; set; }
        public string rdbPLab { get; set; }
        public string rdbPending { get; set; }
        public string rdbPaid { get; set; }
        public string rdbDue { get; set; }
        public string rdbPartlyPaid { get; set; }
        public string lstTStatus { get; set; }
        public string lstIE { get; set; }
        public string ddlVender { get; set; }
        public string lstLab { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string Disciplinewise { get; set; }
        public string rdbPDis { get; set; }
        public string Discipline { get; set; }
        public List<LabReportsModel> lstLabReport { get; set; }
        public List<LabReportsModel> lstsum { get; set; }

        public string barcode_no { get; set; }
        public string total_qty { get; set; }
        public string createddate { get; set; }
        public string INSPECTOR_CUSTOMER { get; set; }
        public string DESCRIPTION { get; set; }
        public string SEALING_TYPE { get; set; }
        public string RATE { get; set; }
        public string TARGETED_DATE { get; set; }
        public string RTAX { get; set; }
        public List<LabReportsModel> lstBarcodeReport { get; set; }
    }
}
