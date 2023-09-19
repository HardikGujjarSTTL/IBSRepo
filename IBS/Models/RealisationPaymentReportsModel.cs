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
}
