using IBS.Models;

namespace IBS.Interfaces.WebsitePages
{
    public interface IOnlinePaymentGatewayRepository
    {
        public OnlinePaymentGateway VerifyByCaseNo(OnlinePaymentGateway model);

    }
}
