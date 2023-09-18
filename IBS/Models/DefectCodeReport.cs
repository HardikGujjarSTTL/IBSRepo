namespace IBS.Models
{
    public class DefectCodeReport
    {
        public string ReportType { get; set; }

        public string ReportTitle { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public List<DefectCodeList> lstDefectCodeList { get; set; }
    }

    public class DefectCodeList
    {
        public string Code { get; set; }
        public string Upheld { get; set; }
        public string Sorting { get; set; }
        public string Rectification { get; set; }
        public string PriceReduction { get; set; }
        public string Total { get; set; }
    }
}
