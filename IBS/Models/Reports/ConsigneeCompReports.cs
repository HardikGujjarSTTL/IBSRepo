namespace IBS.Models.Reports
{
    public class ConsigneeCompReports
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Allregion { get; set; }
        public string JIInspRegion { get; set; }
        public string InspRegion { get; set; }
        public string JIInspReqRegion { get; set; }
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
        public string AllCM { get; set; }
        public string AllIEs { get; set; }
        public string AllVendors { get; set; }
        public string AllClient { get; set; }
        public string AllConsignee { get; set; }
        public string Compact { get; set; }
        public string AwaitingJI { get; set; }
        public string JIConclusion { get; set; }
        public string JIConclusionfollowup { get; set; }
        public string JIconclusionreport { get; set; }
        public string JIDecidedDT { get; set; }
        public string All { get; set; }
        public string ParticularIEs { get; set; }
        public string IEWise { get; set; }
        public string CMWise { get; set; }
        public string VendorWise { get; set; }
        public string ClientWise { get; set; }
        public string ConsigneeWise { get; set; }
        public string FinancialYear { get; set; }
        public string ParticularCMs { get; set; }
        public string ParticularClients { get; set; }
        public string ParticularConsignee { get; set; }
        public string ParticularVendor { get; set; }
        public string Detailed { get; set; }
        public string FinancialYears { get; set; }
        public string ddlsupercm { get; set; }
        public string ddliename { get; set; }
        public string Clientwiseddl { get; set; }
        public string vendor { get; set; }
        public string Item { get; set; }
        public string consignee { get; set; }
        public string Region { get; set; }
        public string FinancialYearsText { get; set; }
        public string FinancialYearsValue { get; set; }
        public string JiSno { get; set; }
        public string month { get; set; }
        public string year { get; set; }
        public string valinsp { get; set; }
        public string ICDate { get; set; }
        public string BillDate { get; set; }
        public string formonth { get; set; }
        public string monthChar { get; set; }
        public string forperiod { get; set; }
        public string jirequired { get; set; }
        public string Regions { get; set; }

        public List<DefectCodeList> lstDefectCodeList { get; set; }

        public List<ConsigneeComplaintsReportModel> lstConsigneeComplaints { get; set; }
    }

    public class JIRequiredReport
    {
        public string AllCM { get; set; }
        public string AllIEs { get; set; }
        public string AllVendors { get; set; }
        public string AllClient { get; set; }
        public string AllConsignee { get; set; }
        public string Compact { get; set; }
        public string AwaitingJI { get; set; }
        public string JIConclusion { get; set; }
        public string JIConclusionfollowup { get; set; }
        public string JIconclusionreport { get; set; }
        public string JIDecidedDT { get; set; }
        public string All { get; set; }
        public string ParticularIEs { get; set; }
        public string IEWise { get; set; }
        public string CMWise { get; set; }
        public string VendorWise { get; set; }
        public string ClientWise { get; set; }
        public string ConsigneeWise { get; set; }
        public string FinancialYear { get; set; }
        public string ParticularCMs { get; set; }
        public string ParticularClients { get; set; }
        public string ParticularConsignee { get; set; }
        public string ParticularVendor { get; set; }
        public string Detailed { get; set; }
        public string FinancialYears { get; set; }
        public string ddlsupercm { get; set; }
        public string ddliename { get; set; }
        public string Clientwiseddl { get; set; }
        public string vendor { get; set; }
        public string Item { get; set; }
        public string consignee { get; set; }
        public string Region { get; set; }
        public string ReportTitle { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string FinancialYearsText { get; set; }
        public string FinancialYearsValue { get; set; }
        public string Regions { get; set; }
        public List<JIRequiredList> lstJIRequiredList { get; set; }
        public List<JIRequiredList> lstCompList { get; set; }
        public List<ComplaintJIIE> lstComplaintJIIE { get; set; }

        public List<ConsigneeComplaintsReportModel> lstConsigneeComplaints { get; set; }

    }

    public class ComplaintJIIE
    {
        public string Region { get; set; }
        public string S_Code { get; set; }
        public string IE { get; set; }
        public int NO_OF_INSPECTION { get; set; }
        public int MATERIAL_VALUE { get; set; }
        public int RECD { get; set; }
        public int FINALISED { get; set; }
        public int PENDING { get; set; }
        public int ACCEPTED { get; set; }
        public int UPHELD { get; set; }
        public int SORTING { get; set; }
        public int RECTIFICATION { get; set; }
        public int PRICE_REDUCTION { get; set; }
        public int LIFTED_BEFORE_JI { get; set; }
        public int TRANSIT_DEMAGE { get; set; }
        public int UNSTAMPED { get; set; }
        public int NOT_ON_RITES_AC { get; set; }
        public int Total { get; set; }
    }
}
