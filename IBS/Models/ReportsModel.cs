namespace IBS.Models
{
    public class ReportsModel
    {
        public string ReportType { get; set; }

        public string ReportTitle { get; set; }

        public DateTime? FromDate { get; set; }
        public string Display_FromDate { get { return this.FromDate != null ? Common.ConvertDateFormat(this.FromDate.Value) : ""; } }
        public DateTime? ToDate { get; set; }
        public string Display_ToDate { get { return this.ToDate != null ? Common.ConvertDateFormat(this.ToDate.Value) : ""; } }

        public string Region { get; set; }
        public string Ie_Cd { get; set; }
        public string IE_Name { get; set; }
        public string Type { get; set; }
        public string Bk_No { get; set; }
        public string Set_No { get; set; }
        public string OrderByVisit { get; set; }
    }

    public class ICUnbilledModel
    {
        public string Region { get; set; }

        public DateTime? FromDate { get; set; }

        public string Display_FromDate { get { return this.FromDate != null ? Common.ConvertDateFormat(this.FromDate.Value) : ""; } }

        public DateTime? ToDate { get; set; }

        public string Display_ToDate { get { return this.ToDate != null ? Common.ConvertDateFormat(this.ToDate.Value) : ""; } }
        public List<IC_Unbilled_List_Model> lstICUnBilledList { get; set; }
    }
    public class IC_Unbilled_List_Model
    {
        public string SUBMIT_DATE { get; set; }
        public string IC_SUBMIT_DATE { get; set; }
        public string IE_NAME { get; set; }
        public string BK_NO { get; set; }
        public string SET_NO { get; set; }
        public string CLIENT_TYPE { get; set; }
        public string REMARKS { get; set; }
        public string REMARKS_DATE { get; set; }
        public string IC_DATE { get; set; }
    }
    public class ICIssuedNotReceivedReportModel
    {
        public string ReportType { get; set; }
        public string ReportTitle { get; set; }
        public string Region { get; set; }
        public DateTime? FromDate { get; set; }
        public string Display_FromDate { get { return this.FromDate != null ? Common.ConvertDateFormat(this.FromDate.Value) : ""; } }
        public DateTime? ToDate { get; set; }
        public string Display_ToDate { get { return this.ToDate != null ? Common.ConvertDateFormat(this.ToDate.Value) : ""; } }
        public string Type { get; set; }
        public List<ICIssueNotReceiveModel> ICIssuedNotReceivedList { get; set; }
    }

    public class IE7thCopyListModel
    {
        public string Bk_No { get; set; }
        public string Set_No_Fr { get; set; }
        public string Set_No_To { get; set; }
        public DateTime? Issue_Dt { get; set; }
        public string Issue_To_Iecd { get; set; }
        public string Bk_Submited { get; set; }
        public DateTime? Bk_Submit_Dt { get; set; }
        public string Bk_Issue_To_Region { get; set; }

        public string UserName { get; set; }
        public string UserID { get; set; }
        public List<IE7thCopyReportModel> lstIE7thCopyList { get; set; }
    }

    public class IE7thCopyReportModel
    {
        public string Case_No { get; set; }
        public string Bk_No { get; set; }
        public string Set_No { get; set; }
    }
    public class PendingJICasesReportModel
    {
        public DateTime? FromDate { get; set; }
        public string Display_FromDate { get { return this.FromDate != null ? Common.ConvertDateFormat(this.FromDate.Value) : ""; } }
        public DateTime? ToDate { get; set; }
        public string Display_ToDate { get { return this.ToDate != null ? Common.ConvertDateFormat(this.ToDate.Value) : ""; } }
        public List<PendingJICasesReportListModel> lstPendingJI { get; set; }
    }
    public class PendingJICasesReportListModel
    {
        public string COMPLAINT_ID { get; set; }
        public string COMPLAINT_DATE { get; set; }
        public string REJ_MEMO { get; set; }
        public string CASE_NO { get; set; }
        public string BK_NO { get; set; }
        public string SET_NO { get; set; }
        public string IE_NAME { get; set; }
        public string IE_CO_NAME { get; set; }
        public string COMP_RECV_REGION { get; set; }
        public string CONSIGNEE { get; set; }
        public string VENDOR { get; set; }
        public string ITEM_DESC { get; set; }
        public string QTY_OFF { get; set; }
        public string QTY_REJECTED { get; set; }
        public string REJECTION_VALUE { get; set; }
        public string REJECTION_REASON { get; set; }
        public string STATUS { get; set; }
        public string JI_REQUIRED { get; set; }
        public string JI_SNO { get; set; }
        public string JI_DATE { get; set; }
        public string DEFECT_DESC { get; set; }
        public string JI_STATUS_DESC { get; set; }
        public string ACTION { get; set; }
        public string PO_NO { get; set; }
        public string PO_DATE { get; set; }
        public string IC_DATE { get; set; }
        public string JI_IE_NAME { get; set; }
        public string ACTION_PROPOSED { get; set; }
        public string ACTION_PROPOSED_DATE { get; set; }
        public string CO_NAME { get; set; }

        public bool IsCaseTIF { get; set; }
        public bool IsCasePDF { get; set; }
        public bool IsReportTIF { get; set; }
        public bool IsReportPDF { get; set; }
        //public string CASE_NO { get; set; }
        //public string BK_NO { get; set; }
        //public string SET_NO { get; set; }
    }

    public class IEDairyModel
    {
        public string ReportType { get; set; }
        public string ReportTitle { get; set; }
        public DateTime? FromDate { get; set; }
        public string Display_FromDate { get { return this.FromDate != null ? Common.ConvertDateFormat(this.FromDate.Value) : ""; } }
        public DateTime? ToDate { get; set; }
        public string Display_ToDate { get { return this.ToDate != null ? Common.ConvertDateFormat(this.ToDate.Value) : ""; } }
        public string DpIE { get; set; }
        public string OrderByVisit { get; set; }
        public string IsAllIE { get; set; }
        public string Region { get; set; }
        public List<IEDairyListModel> lstIEDairy { get; set; }
    }
    public class IEDairyListModel
    {
        public string IE_NAME { get; set; }
        public string PO { get; set; }
        public string CASE_NO { get; set; }
        public string CALL { get; set; }
        public string VEND { get; set; }
        public string DT_OF_VISIT { get; set; }
        public string ITEM_DESC_PO { get; set; }
        public string QTY_TO_INSP { get; set; }
        public string CALL_INSTALL_NO { get; set; }
        public string BK_NO { get; set; }
        public string SET_NO { get; set; }
        public string IC_ISSUE_DT { get; set; }
        public string CONSIGNEE { get; set; }
        public string MATERIAL_VALUE { get; set; }
        public string SUBMIT_DT { get; set; }
        public string INSP_FEE { get; set; }
        //public string MyProperty { get; set; }
    }

    #region Status of IC Report
    public class ICStatusModel
    {
        public DateTime? FromDate { get; set; }
        public string Display_FromDate { get { return this.FromDate != null ? Common.ConvertDateFormat(this.FromDate.Value) : ""; } }
        public DateTime? ToDate { get; set; }
        public string Display_ToDate { get { return this.ToDate != null ? Common.ConvertDateFormat(this.ToDate.Value) : ""; } }

        public string Region { get; set; }
        public string Type { get; set; }
        public string IE_Name { get; set; }
        public List<ICStatusListModel> lstICStatus { get; set; }

    }
    public class ICStatusListModel
    {
        public string IC_SUBMIT_DT { get; set; }
        public string IE_NAME { get; set; }
        public string BK_NO { get; set; }
        public string SET_NO { get; set; }
        public string BILL_NO { get; set; }

    }
    #endregion

    public class IEWorkPlanModel
    {
        public DateTime? FromDate { get; set; }
        public string Display_FromDate { get { return this.FromDate != null ? Common.ConvertDateFormat(this.FromDate.Value) : ""; } }
        public DateTime? ToDate { get; set; }
        public string Display_ToDate { get { return this.ToDate != null ? Common.ConvertDateFormat(this.ToDate.Value) : ""; } }
        public string Region { get; set; }

        public List<IEWorkPlanList1Model> lstWorkPlan1 { get; set; }
        public List<IEWorkPlanList2Model> lstWorkPlan2 { get; set; }
        public List<IEWorkPlanList3Model> lstWorkPlan3 { get; set; }
    }

    public class IEWorkPlanList1Model
    {
        public string IE_NAME { get; set; }
        public string CO_NAME { get; set; }
        public string VISIT_DATE { get; set; }
        public string LOGIN_TIME { get; set; }
        public string CASE_NO { get; set; }
        public string CALL_RECV_DATE { get; set; }
        public string DESIRE_DT { get; set; }
        public string CALL_SNO { get; set; }
        public string CHK_COUNT { get; set; }
        public string MFG_NAME { get; set; }
        public string MFG_PLACE { get; set; }
        public string MFG_CITY { get; set; }
        public string ITEM_DESC_PO { get; set; }
        public string VALUE { get; set; }
        public string CALL_STATUS { get; set; }
    }

    public class IEWorkPlanList2Model
    {
        public string IE_NAME { get; set; }
        public string CO_NAME { get; set; }
        public string NI_WORK_PLAN_CD { get; set; }
        public string WORK_DATE { get; set; }
        public string LOGIN_TIME { get; set; }
    }

    public class IEWorkPlanList3Model
    {
        public int SrNo { get; set; }
        public string IE_NAME { get; set; }
        public string CO_NAME { get; set; }
        public string IE_CD { get; set; }
        public string Date { get; set; }
        public string Reason { get; set; }

    }
}
