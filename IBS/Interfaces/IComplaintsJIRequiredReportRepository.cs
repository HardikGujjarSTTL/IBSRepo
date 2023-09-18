using IBS.Models;
using IBS.Models.Reports;

namespace IBS.Interfaces
{
    public interface IComplaintsJIRequiredReportRepository
    {
        string GetItems(string Clientwise);
        public JIRequiredReport GetJIRequiredList(string FromDate, string ToDate, string AllCM, string AllIEs, string AllVendors, string AllClient, string AllConsignee, string Compact, string AwaitingJI, string JIConclusion, string JIConclusionfollowup,
            string JIconclusionreport, string JIDecidedDT, string All, string ParticularIEs, string IEWise, string CMWise, string VendorWise, string ClientWise, string ConsigneeWise, string FinancialYear, string ParticularCMs, string ParticularClients, string ParticularConsignee,
            string ParticularVendor, string Detailed, string FinancialYears, string ddlsupercm, string ddliename, string Clientwiseddl, string vendor, string Item, string consignee,string Region);
    }
}
