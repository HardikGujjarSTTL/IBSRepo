﻿using IBS.Models;

namespace IBS.Interfaces.WebsitePages
{
    public interface IOnlinePaymentGatewayRepository
    {
        public OnlinePaymentGateway VerifyByCaseNo(OnlinePaymentGateway model);

        public OnlinePaymentGateway PaymentIntergreationSave(OnlinePaymentGateway model);

        public OnlinePaymentGateway PaymentResponseUpdate(OnlinePaymentGateway model,string id);

        public OnlinePaymentGateway PaymentCallBackUpdate(OnlinePaymentGateway model);

        public OnlinePaymentGateway PaymentTrackingResponse(OnlinePaymentGateway model);

        OnlinePaymentGateway BindPaymentList();

        string GetMerTrnRef(string CaseNo,string ChargesType);
    }
}
