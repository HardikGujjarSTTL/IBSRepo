using IBS.Models;

namespace IBS.Interfaces
{
    public interface IOnlinePaymentReportRepository
    {

        DTResult<OnlinePaymentReportModel> OnlinePaymentReport(DTParameters dtParameters, string Regin);

    }
}
