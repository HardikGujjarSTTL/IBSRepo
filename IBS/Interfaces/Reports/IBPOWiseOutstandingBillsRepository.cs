using IBS.Models;
using System.Data;

namespace IBS.Interfaces.Reports
{
    public interface IBPOWiseOutstandingBillsRepository
    {
        BPOWiseOutstandingBillsModel GenerateReport(string ReportType, string FromDt, string ToDt, string BpoCd, string BpoType, string BpoRly, string BpoRegion, string Checkbox, string Railway, string PSU, string StateGovt, string ForeignRailways, string PrivateSector, string TypeofOutStandingBills,string Region);
    }
}
