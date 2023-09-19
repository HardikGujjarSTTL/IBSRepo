namespace IBS.Models
{
    public class ConsigneeCompPeriodReport
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Allregion { get; set; }
        public string regionorth { get; set; }
        public string regionsouth { get; set; }
        public string regioneast { get; set; }
        public string regionwest { get; set; }
        public string jiallregion { get; set; }
        public string jinorth { get; set; }
        public string jisourth { get; set; }
        public string jieast { get; set; }
        public string jiwest { get; set; }
        public string compallregion { get; set; }
        public string compyes { get; set; }
        public string compno { get; set; }
        public string cancelled { get; set; }
        public string underconsider { get; set; }
        public string allaction { get; set; }
        public string particilaraction { get; set; }
        public string actiondrp { get; set; }
        public string ReportTitle { get; set; }
        public string ReportType { get; set; }
        public string particilarcode { get; set; }
        public string particilarjicode { get; set; }
        public string actioncodedrp { get; set; }
        public string actionjidrp { get; set; }

        public List<ConsigneeComplaintsReportModel> lstConsigneeComplaints { get; set; }
    }
}
