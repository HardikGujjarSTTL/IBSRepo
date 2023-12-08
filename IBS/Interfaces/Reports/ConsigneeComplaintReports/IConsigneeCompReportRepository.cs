using IBS.Models;
using IBS.Models.Reports;

namespace IBS.Interfaces.Reports.ConsigneeComplaintReports
{
    public interface IConsigneeCompReportRepository
    {
        public ConsigneeCompReports GetCompPeriodData(string FromDate, string ToDate,string InspRegion,string JIInspRegion,string JIInspReqRegion, string actiondrp, string actioncodedrp, string actionjidrp,int IeCd);
        string GetItems(string Clientwise);
        public JIRequiredReport GetJIRequiredList(string FromDate, string ToDate, string AllCM, string AllIEs, string AllVendors, string AllClient, string AllConsignee, string Compact, string AwaitingJI, string JIConclusion, string JIConclusionfollowup,
           string JIconclusionreport, string JIDecidedDT, string All, string ParticularIEs, string IEWise, string CMWise, string VendorWise, string ClientWise, string ConsigneeWise, string FinancialYear, string ParticularCMs, string ParticularClients, string ParticularConsignee,
           string ParticularVendor, string Detailed, string FinancialYears, string ddlsupercm, string ddliename, string Clientwiseddl, string vendor, string Item, string consignee, string Region, string FinancialYearsvalue);
        public ConsigneeComplaints GetComplaintReportDetails(string JISNO, string Region);
        public DefectCodeReport GetDefectCodeWiseData(DateTime FromDate, DateTime ToDate, string Region);
        public JIRequiredReport GetJIComplaintsList(string FinancialYearsText, string FinancialYearsValue);
        public HighValueInspReport GetHighValueInspdata(string month, string year, string valinsp, string FromDate, string ToDate, string ICDate, string BillDate, string formonth, string forperiod, string Region);


    }
}
