using IBS.Models;

namespace IBS.Interfaces.WebsitePages
{
    public interface IOnlinePaymentGatewayRepository
    {
        public OnlinePaymentGateway VerifyByCaseNo(OnlinePaymentGateway model);

        public OnlinePaymentGateway PaymentIntergreationSave(OnlinePaymentGateway model);

        public OnlinePaymentGateway PaymentResponseUpdate(OnlinePaymentGateway model);
    }
}
