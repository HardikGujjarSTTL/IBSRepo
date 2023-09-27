namespace IBS.Models.Reports
{
    public class MonthlyReportModel
    {
        public string ReportType { get; set; }

        public string ReportTitle { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string Ie_Cd { get; set; }
    }

    public class AllICStatusModel
    {
        public DateTime? FromDate { get; set; }
        public string Display_FromDate { get { return this.FromDate != null ? Common.ConvertDateFormat(this.FromDate.Value) : ""; } }
        public DateTime? ToDate { get; set; }
        public string Display_ToDate { get { return this.ToDate != null ? Common.ConvertDateFormat(this.ToDate.Value) : ""; } }
        public string IECD { get; set; }
        public string Region { get; set; }
        public List<AllICStatusListModel> lstAllICStatus { get; set; }
    }
    public class AllICStatusListModel
    {
        public string IE_NAME { get; set; }
        public string BK_NO { get; set; }
        public string SET_NO { get; set; }
        public string STATUS { get; set; }
    }

    public class ReInspectionICsModel
    {
        public DateTime? FromDate { get; set; }
        public string Display_FromDate { get { return this.FromDate != null ? Common.ConvertDateFormat(this.FromDate.Value) : ""; } }
        public DateTime? ToDate { get; set; }
        public string Display_ToDate { get { return this.ToDate != null ? Common.ConvertDateFormat(this.ToDate.Value) : ""; } }
        public string Region { get; set; }
        public List<ReInspectionICsListModel> lstReInspectionIC { get; set; }
    }

    public class ReInspectionICsListModel
    {
        public string BPO { get; set; }
        public string BPO_CD { get; set; }
        public string BILL_NO { get; set; }
        public string BILL_DT { get; set; }
        public string PO_NO { get; set; }
        public string PO_DT { get; set; }
        public string CASE_NO { get; set; }
        public string RLY_CD { get; set; }
        public string BK_NO { get; set; }
        public string SET_NO { get; set; }
        public string IE_SNAME { get; set; }
        public string Vendor { get; set; }
        public string IC_NO { get; set; }
        public string IC_DT { get; set; }
        public string BILL_AMOUNT { get; set; }
        public string INSP_FEE { get; set; }
        public string SERVICE_TAX { get; set; }
        public string TAX { get; set; }
    }

}
