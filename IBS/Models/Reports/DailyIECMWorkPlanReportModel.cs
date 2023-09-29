namespace IBS.Models.Reports
{
    public class DailyIECMWorkPlanReportModel
    {
        public string ReportType { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string lstIE { get; set; }
        public string lstCM { get; set; }
        public string AllIEs { get; set; }
        public string ParticularIEs { get; set; }
        public string AllCM { get; set; }
        public string ParticularCMs { get; set; }
        public string ReportTitle { get; set; }
        public string IEWise { get; set; }
        public string CMWise { get; set; }
        public string SortedIE { get; set; }
        public string visitdate { get; set; }

        public List<DailyIECMWorkPlanReporttbl1> lstDailyIECMWorkPlanReporttbl1 { get; set; }
        public List<DailyIECMWorkPlanReporttbl2> lstDailyIECMWorkPlanReporttbl2 { get; set; }
        public List<DailyIECMWorkPlanReporttbl3> lstDailyIECMWorkPlanReporttbl3 { get; set; }
        public List<DailyIEWorklanExcepReport> lstDailyIEWorklanExcepReport { get; set; }
    }

    public class DailyIECMWorkPlanReporttbl1
    {
        public string CO_NAME { get; set;}
        public string IE_NAME { get; set;}
        public string VISIT_DATE { get; set;}
        public string LOGIN_TIME { get; set;}
        public string CASE_NO { get; set;}
        public string CALL_RECV_DATE { get; set;}
        public string DESIRE_DT { get; set;}
        public string CALL_SNO { get; set;}
        public string CHK_COUNT { get; set;}
        public string MFG_NAME { get; set;}
        public string MFG_PLACE { get; set;}
        public string MFG_CITY { get; set;}
        public string ITEM_DESC_PO { get; set;}
        public string VALUE { get; set;}
        public string CALL_STATUS { get; set;}
    }
    
    public class DailyIECMWorkPlanReporttbl2
    {
        public string CO_NAME { get; set;}
        public string IE_NAME { get; set;}
        public string LOGIN_TIME { get; set;}
        public string WORK_DATE { get; set;}
        public string NI_WORK_PLAN_CD { get; set;}
    }

    public class DailyIECMWorkPlanReporttbl3
    {
        public string CO_NAME { get; set; }
        public string IE_NAME { get; set; }
        public string Date { get; set; }
        public string Reason { get; set; }
    }

    public class DailyIEWorklanExcepReport
    {
        public string CASE_NO { get; set; }
        public string CALL_RECV_DATE { get; set; }
        public string CALL_SNO { get; set; }
        public string IE_NAME { get; set; }
        public string BK_NO { get; set; }
        public string SET_NO { get; set; }
        public string NO_OF_INSP { get; set; }
        public string IC_DATE { get; set; }
        public string FIRST_INSP_DATE { get; set; }
        public string LAST_INSP_DATE { get; set; }
        public string VISIT_DATE { get; set; }
    }
}
