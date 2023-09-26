namespace IBS.Models.Reports
{
    public class PeriodWiseChecksheetReportModel
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string ReportTitle { get; set; }

        public List<PeriodWiseChecksheet> lstPeriodWiseChecksheet { get; set; }
    }

    public class PeriodWiseChecksheet
    {
        public string ITEM_DESC { get; set; }
        public string IE { get; set; }
        public string CO_NAME { get; set; }
        public string CREATION_REV_DT { get; set; }
    }
}
