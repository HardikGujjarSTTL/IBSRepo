namespace IBS.Models.Reports
{
    public class VendorFeedbackReportModel
    {
        public List<VendorFeedbackReport> lstVendorFeedbackReport { get; set; }
    }

    public class VendorFeedbackReport
    {
        public string  Vendor { get; set; }
        public string Region { get; set; }
        public int FIELD_1 { get; set; }
        public int FIELD_2 { get; set; }
        public int FIELD_3 { get; set; }
        public int FIELD_4 { get; set; }
        public int FIELD_5 { get; set; }
        public int FIELD_6 { get; set; }
        public int FIELD_7 { get; set; }
        public int FIELD_8 { get; set; }
        public string FIELD_9 { get; set; }
        public string FIELD_10 { get; set; }
        public int MTHYR_PERIOD { get; set; }
    }
}
