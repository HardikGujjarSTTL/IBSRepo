using IBS.Models;

namespace IBS.Interfaces.Reports.RealisationPayment
{
    public interface IRealisationPaymentRepository
    {
        SummaryOnlinePaymentModel GetSummaryOnlinePayment(DateTime FromDate, DateTime ToDate, string Region);
    }
}
