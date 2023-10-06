namespace IBS.Models.Reports
{
    public class HighValueInspReport
    {
        public string month { get; set; }
        public string year { get; set; }
        public string valinsp { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string ICDate { get; set; }
        public string BillDate { get; set; }
        public string formonth { get; set; }
        public string monthChar { get; set; }
        public string forperiod { get; set; }
        public string ReportTitle { get; set; }
        public string Regions { get; set; }

        public List<ValueInspList> lstValueInspList { get; set; }
    }

    public class ValueInspList
    {
        public string BillNo { get; set; }
        public DateTime? BillDate { get; set; }
        public DateTime? ICDate { get; set; }
        public string CaseNo { get; set; }
        public string EngName { get; set; }
        public string VENDOR { get; set; }
        public string CONSIGNEE { get; set; }
        public string ITEMDESC { get; set; }
        public string PLNO { get; set; }
        public string MATERIALVALUE { get; set; }
        public string INSPFEE { get; set; }
    }
}
