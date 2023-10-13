namespace IBS.Models.Reports
{
    public class VendorPerformanceReportModel
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string formonth { get; set; }
        public string forperiod { get; set; }
        public string month { get; set; }
        public string year { get; set; }
        public string vendcd { get; set; }
        public string vendor { get; set; }
        public string monthtxt { get; set; }
        public string Region { get; set; }
        public string todaydate { get; set; }
        public string ReportTitle { get; set; }

        public List<VendorPerformance> lstVendorPerformance { get; set; }
    }

    public class VendorPerformance
    {
        public string ITEM_DESC { get; set; }
        public string PO_NO { get; set; }
        public string PO_DATE { get; set; }
        public string CONSIGNEE { get; set; }
        public string QTY_TO_INSP { get; set; }
        public string UOM { get; set; }
        public string QTY_PASSED { get; set; }
        public string QTY_REJECTED { get; set; }
        public string REASON_REJECT { get; set; }
        public string CALL_DATE { get; set; }
        public string FIRST_INSP_DATE { get; set; }
        public string LAST_INSP_DATE { get; set; }
        public string CALL_STATUS_DESC { get; set; }
        public string IC_NO { get; set; }
        public string IC_DATE { get; set; }
    }
}
