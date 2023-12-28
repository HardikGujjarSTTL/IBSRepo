using IBS.Models;
using IBS.Models.Reports;

namespace IBS.Interfaces.Reports.OtherReports
{
    public interface IOtherReportsRepository
    {
        ControllingOfficerIEModel GetControllingOfficerWiseIE(string Region);
        DTResult<CoIeWiseCallsListModel> GetCoIeWiseCalls(DTParameters dtParameters);//string CO, string Status, string IE,bool IsAllIE, bool IsCallDate);
        CoIeWiseCallsModel GetCoIeWiseCallsReport(string Case_No, string Call_Recv_Date, string Call_SNo);
        public NCRReport GetNCRIECOWiseData(string month, string year, string FromDate, string ToDate, string AllCM, string forCM, string All, string Outstanding, string formonth, string forperiod, string Region, string iecmname, string reporttype);
        public IEWiseTrainingReportModel GetIEWiseTrainingDetails(string IENAME, string TrainingArea, string Mechanical, string Electrical, string Civil, string Regular, string Deputaion, string Particularie, string ParticularArea, string Region);
        public OngoingContrcatsReportModel Getongoingcontractdetails(string StatusOffer, string Region, string StatusOffertxt, string Regiontxt, string rdoregionwise);
        public ContractReportModel GetContractDetails(string FromDate, string ToDate, string Region, string clientname);
        public VendorClusterReportModel GetVendorClusterReport(string department, string Region);
        public IEAlterMappingReportModel GetIEAlterMappingReport(string Region);
        public VendorPerformanceReportModel GetVendorperformanceReport(string FromDate, string ToDate, string formonth, string forperiod, string month, string year, string vendcd, string Region);
        public PeriodWiseChecksheetReportModel Getperiodwisechecksheetdetails(string FromDate, string ToDate, string Region);
        public PeriodWiseTechnicalRefReportModel Getperiodwisetechrefdetails(string FromDate, string ToDate, string Region);
        public DailyIECMWorkPlanReportModel GetDailyWorkData(string FromDate, string ToDate, string lstIE, string lstCM, string AllIEs, string ParticularIEs, string AllCM, string ParticularCMs, string ReportType, string IEWise, string CMWise, string Region, string SortedIE, string visitdate);
        public VendorFeedbackReportModel GetVendorFeedbackReport(string Region);
        DTResult<IEICPhotoEnclosedModelReport> GetDataList(DTParameters dtParameters, string Region);
        IEICPhotoEnclosedModelReport GetDataListReport(string CaseNo, string CallRecDT, string CallSno, string BKNO, string SETNO, string Region);
        OtherReportsModel GetDSCExpReport(string DSCMonth, string DSCYear, string DSCToMonth, string DSCToYear, string DSC_Monthrdo, string Region);
    }
}
