namespace IBS.Models.Reports
{
    public class PeriodWiseTechnicalRefReportModel
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string ReportTitle { get; set; }
        public string Regions { get; set; }

        public List<PeriodWiseTechnicalRef> lstPeriodWiseTechnicalRef { get; set; }
    }

    public class PeriodWiseTechnicalRef
    {
        public string cm_name { get; set; }
        public string ie_name { get; set; }
        public string item_des { get; set; }
        public string spec_drg { get; set; }
        public string letter_no { get; set; }
        public string tech_date { get; set; }
        public string ref_made { get; set; }
        public string tech_content { get; set; }
    }
}
