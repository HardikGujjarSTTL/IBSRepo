namespace IBS.Models
{
    public class RealisationPaymentReportsModel
    {
        public string ReportType { get; set; }

        public string ReportTitle { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }

    public class SummaryOnlinePaymentModel
    {
        public DateTime? FromDate { get; set; }
        public string Display_FromDate { get { return this.FromDate != null ? Common.ConvertDateFormat(this.FromDate.Value) : ""; } }

        public DateTime? ToDate { get; set; }
        public string Display_ToDate { get { return this.ToDate != null ? Common.ConvertDateFormat(this.ToDate.Value) : ""; } }
        public string Region { get; set; }
        public List<SummaryOnlinePaymentListModel> lstOnlinePayment { get; set; }
    }

    public class SummaryOnlinePaymentListModel
    {
        public string MER_TXN_REF { get; set; }
        public Int32? ORDER_INFO { get; set; }
        public string TRANSACTION_NO { get; set; }
        public string RRN_NO { get; set; }
        public string AUTH_CD { get; set; }
        public string CASE_NO { get; set; }
        public string CALL_DT { get; set; }        
        public Int32? CALL_SNO { get; set; }
        public string VENDOR { get; set; }
        public decimal? AMOUNT { get; set; }
        public string TYPE { get; set; }
        public string STATUS { get; set; }
        public string DATETIME { get; set; }
    }

    public class SummaryCrisRlyPaymentModel
    {
        public DateTime? FromDate { get; set; }
        public string Display_FromDate { get { return this.FromDate != null ? Common.ConvertDateFormat(this.FromDate.Value) : ""; } }

        public DateTime? ToDate { get; set; }
        public string Display_ToDate { get { return this.ToDate != null ? Common.ConvertDateFormat(this.ToDate.Value) : ""; } }
        public string Region { get; set; }
        public List<SummaryCrisRlyPaymentDetailedModel> lstCrisRlyDetailed { get; set; }
    }

    public class SummaryCrisRlyPaymentDetailedModel
    {
        public string BILL_NO { get; set; }
        public string BPO_RLY { get; set; }
        public string INVOICENO { get; set; }
        public string INVOICEDATE { get; set; }
        public string RECV_DATE { get; set; }
        public string IC_NO { get; set; }
        public string IC_DT { get; set; }
        public string CO6_NO { get; set; }
        public string CO6_DATE { get; set; }
        public int AMOUNT { get; set; }
        public string PASSED_AMT { get; set; }
        public string DEDUCTED_AMT { get; set; }
        public string NET_AMT { get; set; }
        public string BOOKDATE { get; set; }
        public string RETURN_REASON { get; set; }
        public string RETURN_DATE { get; set; }
        public string CO7_NO { get; set; }
        public string CO7_DATE { get; set; }
        public string PAYMENT_DT { get; set; }
        public string AU_DESC { get; set; }
        public string IBS_AMT_CLEARED { get; set; }
    }
}
