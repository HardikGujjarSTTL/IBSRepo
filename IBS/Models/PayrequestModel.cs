namespace IBS.Models
{
    public class PayrequestModel
    {
        public class HeadDetails
        {
            public string version { get; set; }
            public string api { get; set; }
            public string platform { get; set; }
        }

        public class MerchDetails
        {
            public string merchId { get; set; }
            public string userId { get; set; }
            public string password { get; set; }
            public string merchTxnDate { get; set; }
            public string merchTxnId { get; set; }
        }

        public class PayDetails
        {
            public string amount { get; set; }
            public string product { get; set; }
            public string custAccNo { get; set; }
            public string txnCurrency { get; set; }
        }

        public class CustDetails
        {
            public string custEmail { get; set; }
            public string custMobile { get; set; }
        }

        public class Extras
        {
            public string udf1 { get; set; }
            public string udf2 { get; set; }
            public string udf3 { get; set; }
            public string udf4 { get; set; }
            public string udf5 { get; set; }
        }

        public class MsgBdy
        {
            public HeadDetails headDetails { get; set; }
            public MerchDetails merchDetails { get; set; }
            public PayDetails payDetails { get; set; }
            public CustDetails custDetails { get; set; }
            public Extras extras { get; set; }
        }

        public class Payrequest
        {
            public HeadDetails headDetails { get; set; }
            public MerchDetails merchDetails { get; set; }
            public PayDetails payDetails { get; set; }
            public CustDetails custDetails { get; set; }
            public Extras extras { get; set; }
        }

        public class RootObject
        {
            public Payrequest payInstrument { get; set; }
        }
    }

    public class TransactionTrackingRequestModel
    {
        public class MerchDetails
        {
            public int merchId { get; set; }
            public string merchTxnId { get; set; }
            public string merchTxnDate { get; set; }
        }

        public class PayDetails
        {
            public decimal amount { get; set; }
            public string txnCurrency { get; set; }
            public string signature { get; set; }
        }

        public class PayInstrument
        {
            public MerchDetails merchDetails { get; set; }
            public PayDetails payDetails { get; set; }
        }

        public class Rootobject
        {
            public PayInstrument payInstrument { get; set; }
        }
    }

    public class TransactionTrackingResponseModel
    {
        public class Rootobject
        {
            public PayInstrument payInstrument { get; set; }
        }

        public class PayInstrument
        {
            public SettlementDetails settlementDetails { get; set; }
            public MerchDetails merchDetails { get; set; }
            public PayDetails payDetails { get; set; }
            public PayModeSpecificData payModeSpecificData { get; set; }
            public ResponseDetails responseDetails { get; set; }
        }

        public class SettlementDetails
        {
            public string reconStatus { get; set; }
        }

        public class MerchDetails
        {
            public string merchId { get; set; }
            public string merchTxnId { get; set; }
            public string merchTxnDate { get; set; }
            public string clientCode { get; set; }
        }

        public class PayDetails
        {
            public string atomTxnId { get; set; }
            public string product { get; set; }
            public double amount { get; set; }
            public double surchargeAmount { get; set; }
            public double totalAmount { get; set; }
        }

        public class PayModeSpecificData
        {
            public string subChannel { get; set; }
            public BankDetails bankDetails { get; set; }
        }

        public class ResponseDetails
        {
            public string statusCode { get; set; }
            public string message { get; set; }
            public string description { get; set; }
        }

        public class BankDetails
        {
            public string bankTxnId { get; set; }
            public string otsBankName { get; set; }
            public string bankAuthId { get; set; }
        }

    }
}
