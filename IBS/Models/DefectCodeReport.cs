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
        public decimal Upheld { get; set; }
        public decimal Sorting { get; set; }
        public decimal Rectification { get; set; }
        public decimal PriceReduction { get; set; }
        public decimal Total { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}
