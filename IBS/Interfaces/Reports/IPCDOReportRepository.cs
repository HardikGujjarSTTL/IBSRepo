using IBS.Models;
using IBS.Models.Reports;

namespace IBS.Interfaces.Reports
{
    public interface IPCDOReportRepository
    {
        List<HighlightReportsModel> GetHighlightData(string p_wYrMth);

        List<FinancialBillingModel> GetFinancialBillingData(int dmonth, string wYrMth_Past, string CumYrPast, string wYrMth, string CumYrMth, int byear);

    }
}
