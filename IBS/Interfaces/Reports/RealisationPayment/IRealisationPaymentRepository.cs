using IBS.Models;

namespace IBS.Interfaces.Reports.RealisationPayment
{
    public interface IRealisationPaymentRepository
    {
        SummaryOnlinePaymentModel GetSummaryOnlinePayment(DateTime FromDate, DateTime ToDate, string Region);
        SummaryCrisRlyPaymentModel GetSummaryCrisRlyPaymentDetailed(DateTime FromDate, DateTime ToDate, string IsRly, string Rly, string IsAU, string AU, string IsAllRegion, string Status, string Region);
        SummaryCrisRlyPaymentModel GetSummaryCrisRlyPaymentSummary(DateTime FromDate, DateTime ToDate, string IsRlyWise, string Status, string Region);
    }
}
