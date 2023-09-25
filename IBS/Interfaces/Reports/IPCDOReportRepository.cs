using IBS.Models;
using IBS.Models.Reports;

namespace IBS.Interfaces.Reports
{
    public interface IPCDOReportRepository
    {
        List<HighlightReportsModel> GetHighlightData(string p_wYrMth);
        COHighlightMainModel GetCOHighlightData(string p_CumYrMth, string p_wYrMth, string p_byear, int p_dmonth, string p_lstdate, string p_CumYrPast, string p_wYrMth_Past);
        List<FinancialBillingModel> GetFinancialBillingData(int dmonth, string wYrMth_Past, string CumYrPast, string wYrMth, string CumYrMth, int byear);
        FinancialExpenditureRealizationMainModel GetFinancialExpenditureRealizationData(string wYrMth_Past, string CumYrPast, string wYrMth, string CumYrMth, int byear);
        FinancialOutstandingMainModel GetFinancialOutstandingData(string wYrMth_Past, string CumYrPast, string wYrMth, string CumYrMth, string bakdate, string lstdate);
        List<EOIPricedOfferSentModel> GetEOIPricedOfferSentData(string wYrMth);
        List<BDEffortsModel> GetBDEffortsData(string wYrMth);
        List<EOIPricedOfferSentModel> GetPreviousOfferSentData(string wYrMth);
    }
}
