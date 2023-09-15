namespace IBS.Models.Reports
{
    public class ManagementReportsModel
    {
        public string ReportType { get; set; }

        public string ReportTitle { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string FromYearMonth { get; set; }

        public string ToYearMonth { get; set; }
    }

    public class IEPerformanceModel
    {
        public int RejectionsIssued { get; set; }

        public int TotalICs { get; set; }

        public int CallsAttendedWithin7Days { get; set; }

        public int CallsAttendedBeyond7Days { get; set; }

        public string Region { get; set; }

        public DateTime? FromDate { get; set; }

        public string Display_FromDate { get { return this.FromDate != null ? Common.ConvertDateFormat(this.FromDate.Value) : ""; } }

        public DateTime? ToDate { get; set; }

        public string Display_ToDate { get { return this.ToDate != null ? Common.ConvertDateFormat(this.ToDate.Value) : ""; } }

        public List<IEPerformanceListModel> lstPerformance { get; set; }

        public List<IEPerformanceSummaryListModel> lstPerformanceSummaryList { get; set; }

    }

    public class IEPerformanceListModel
    {
        public string IE_NAME { get; set; }

        public string DEPT { get; set; }

        public decimal C3 { get; set; }

        public decimal C7 { get; set; }

        public decimal CM7 { get; set; }

        public decimal C10 { get; set; }

        public decimal C0 { get; set; }

        public decimal INSP_FEE { get; set; }

        public decimal MATERIAL_VALUE { get; set; }

        public decimal AVERAGE_FEE { get; set; }

        public decimal CALLS { get; set; }

        public decimal CALL_CANCEL { get; set; }

        public decimal REJECTIONS { get; set; }
    }

    public class IEPerformanceSummaryListModel
    {
        public string RLY_NONRLY { get; set; }

        public decimal IC_COUNT { get; set; }

        public decimal MATERIAL_VALUE { get; set; }
    }

    public class ClusterPerformanceModel
    {
        public string Region { get; set; }

        public DateTime? FromDate { get; set; }

        public string Display_FromDate { get { return this.FromDate != null ? Common.ConvertDateFormat(this.FromDate.Value) : ""; } }

        public DateTime? ToDate { get; set; }

        public string Display_ToDate { get { return this.ToDate != null ? Common.ConvertDateFormat(this.ToDate.Value) : ""; } }

        public List<ClusterPerformanceListModel> lstPerformance { get; set; }
    }

    public class ClusterPerformanceListModel
    {
        public string CLUSTER_NAME { get; set; }

        public string DEPT { get; set; }

        public decimal C3 { get; set; }

        public decimal C7 { get; set; }

        public decimal CM7 { get; set; }

        public decimal C10 { get; set; }

        public decimal C0 { get; set; }

        public decimal INSP_FEE { get; set; }

        public decimal MATERIAL_VALUE { get; set; }

        public decimal AVERAGE_FEE { get; set; }

        public decimal CALLS { get; set; }

        public decimal CALL_CANCEL { get; set; }

        public decimal REJECTIONS { get; set; }
    }

    public class RWBSummaryModel
    {
        public string FromYearMonth { get; set; }

        public string ToYearMonth { get; set; }

        public string FilterTitle { get; set; }

        public List<RWBSummaryListModel> lstRWBSummaryList { get; set; }

        public List<RBWSectorListModel> lstRBWSectorList { get; set; }
    }

    public class RWBSummaryListModel
    {
        public string SECTOR { get; set; }

        public decimal NR_FEE { get; set; }

        public decimal NR_TAX { get; set; }

        public decimal NR_BILL_AMT { get; set; }

        public decimal NR_BILLLS { get; set; }

        public decimal WR_FEE { get; set; }

        public decimal WR_TAX { get; set; }

        public decimal WR_BILL_AMT { get; set; }

        public decimal WR_BILLLS { get; set; }

        public decimal ER_FEE { get; set; }

        public decimal ER_TAX { get; set; }

        public decimal ER_BILL_AMT { get; set; }

        public decimal ER_BILLLS { get; set; }

        public decimal SR_FEE { get; set; }

        public decimal SR_TAX { get; set; }

        public decimal SR_BILL_AMT { get; set; }

        public decimal SR_BILLLS { get; set; }

        public decimal CR_FEE { get; set; }

        public decimal CR_TAX { get; set; }

        public decimal CR_BILL_AMT { get; set; }

        public decimal CR_BILLLS { get; set; }
    }

    public class RBWSectorListModel
    {
        public string SECTOR { get; set; }

        public decimal INSP_FEE { get; set; }

        public decimal BILL_AMOUNT { get; set; }

        public decimal NO_OF_BILLLS { get; set; }
    }

}
