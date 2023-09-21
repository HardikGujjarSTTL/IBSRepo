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

        public string Outstanding { get; set; }

        public string ParticularCM { get; set; }

        public string ParticularSector { get; set; }
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

    public class RWCOModel
    {
        public DateTime FromDate { get; set; }

        public string Display_FromDate { get { return Common.ConvertDateFormat(this.FromDate); } }

        public string Outstanding { get; set; }

        public List<RWCOListModel> lsttRWCOList { get; set; }
    }

    public class RWCOListModel
    {
        public string BPO_TYPE { get; set; }

        public string BPO_RLY { get; set; }

        public string BPO_ORGN { get; set; }

        public string BPO_TYPE_CD { get; set; }

        public decimal NR_OUTSTANDING { get; set; }

        public decimal WR_OUTSTANDING { get; set; }

        public decimal ER_OUTSTANDING { get; set; }

        public decimal SR_OUTSTANDING { get; set; }

        public decimal CR_OUTSTANDING { get; set; }

        public decimal TOT_SUSPENSE_NR { get; set; }

        public decimal TOT_SUSPENSE_WR { get; set; }

        public decimal TOT_SUSPENSE_ER { get; set; }

        public decimal TOT_SUSPENSE_SR { get; set; }

        public decimal TOT_SUSPENSE_CR { get; set; }

        public decimal TOT_ALL_OUTSTANDING { get; set; }

        public decimal TOT_ALL_SUSPENSE { get; set; }

    }

    public class ICSubmissionModel
    {
        public string Region { get; set; }

        public DateTime? FromDate { get; set; }

        public string Display_FromDate { get { return this.FromDate != null ? Common.ConvertDateFormat(this.FromDate.Value) : ""; } }

        public DateTime? ToDate { get; set; }

        public string Display_ToDate { get { return this.ToDate != null ? Common.ConvertDateFormat(this.ToDate.Value) : ""; } }

        public List<ICSubmissionListModel> lstICSubmission { get; set; }

    }

    public class ICSubmissionListModel
    {
        public int ID { get; set; }

        public DateTime? IC_SUBMIT_DATE { get; set; }

        public string Display_IC_SUBMIT_DATE { get { return this.IC_SUBMIT_DATE != null ? Common.ConvertDateFormat(this.IC_SUBMIT_DATE.Value) : ""; } }

        public string IE_NAME { get; set; }

        public string BK_NO { get; set; }

        public string SET_NO { get; set; }

    }

    public class PendingICAgainstCallsModel
    {
        public string Region { get; set; }

        public DateTime? FromDate { get; set; }

        public string Display_FromDate { get { return this.FromDate != null ? Common.ConvertDateFormat(this.FromDate.Value) : ""; } }

        public DateTime? ToDate { get; set; }

        public string Display_ToDate { get { return this.ToDate != null ? Common.ConvertDateFormat(this.ToDate.Value) : ""; } }

        public List<PendingICAgainstCallsListModel> lstPendingICAgainstCalls { get; set; }

    }

    public class PendingICAgainstCallsListModel
    {
        public int ID { get; set; }

        public string CASE_NO { get; set; }

        public DateTime? CALL_RECV_DT { get; set; }

        public string Display_CALL_RECV_DT { get { return this.CALL_RECV_DT != null ? Common.ConvertDateFormat(this.CALL_RECV_DT.Value) : ""; } }

        public int CALL_SNO { get; set; }

        public string STATUS { get; set; }

        public string IE_NAME { get; set; }

        public string IE_STATUS { get; set; }

    }

    public class SuperSurpriseDetailsModel
    {
        public string Region { get; set; }

        public DateTime? FromDate { get; set; }

        public string Display_FromDate { get { return this.FromDate != null ? Common.ConvertDateFormat(this.FromDate.Value) : ""; } }

        public DateTime? ToDate { get; set; }

        public string Display_ToDate { get { return this.ToDate != null ? Common.ConvertDateFormat(this.ToDate.Value) : ""; } }

        public List<SuperSurpriseListModel> lstSuperSurprise { get; set; }
    }

    public class SuperSurpriseListModel
    {
        public int ID { get; set; }

        public string SuperSurpriseNo { get; set; }

        public DateTime? SuperSurpriseDt { get; set; }

        public string Display_SuperSurpriseDt { get { return this.SuperSurpriseDt != null ? Common.ConvertDateFormat(this.SuperSurpriseDt.Value) : ""; } }

        public string CoName { get; set; }

        public string IeName { get; set; }

        public string Vendor { get; set; }

        public string ItemDesc { get; set; }

        public string NameScopeItem { get; set; }

        public string PreIntRej { get; set; }

        public string Discrepancy { get; set; }

        public string Outcome { get; set; }

        public string SbuHeadRemarks { get; set; }

    }
}
