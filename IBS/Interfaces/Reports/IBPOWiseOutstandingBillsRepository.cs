using IBS.Models;
using System.Data;

namespace IBS.Interfaces.Reports
{
    public interface IBPOWiseOutstandingBillsRepository
    {
        BPOWiseOutstandingBillsModel GenerateReport(string ReportType, string FromDt, string ToDt, string BpoCd, string BpoType, string BpoRly, string BpoRegion, Boolean Railway, Boolean PSU, Boolean StateGovt, Boolean ForeignRailways, Boolean PrivateSector, string TypeofOutStandingBills,string Region);
    }
}
