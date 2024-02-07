namespace IBS.Models
{
    public class PayCallBackModel
    {
        public class BankDetails
        {
            public int otsBankId { get; set; }
            public string otsBankName { get; set; }
            public string bankTxnId { get; set; }
        }

        public class CardDetails
        {
            public string cardScheme { get; set; }
            public string cardMaskNumber { get; set; }
        }

        public class MerchDetails
        {
            public string merchId { get; set; }
            public string merchTxnId { get; set; }
            public string merchTxnDate { get; set; }
        }

        public class PayDetails
        {
            public double amount { get; set; }
            public double surchargeAmount { get; set; }
            public string atomTxnId { get; set; }
            public double totalAmount { get; set; }
            public string custAccNo { get; set; }
            public string clientCode { get; set; }
            public string txnInitDate { get; set; }
            public string txnCompleteDate { get; set; }
        }
        public class PayInstrument
        {
            public MerchDetails merchDetails { get; set; }
            public PayDetails payDetails { get; set; }
            public ResponseDetails responseDetails { get; set; }
            public PayModeSpecificData payModeSpecificData { get; set; }
            public List<ProdDetail> prodDetails { get; set; }
            public PayInstrument()
            {
                //
                // TODO: Add constructor logic here
                //
            }
        }
        //public class PayInstrument
        //{
        //    public MerchDetails merchDetails { get; set; }
        //    public PayDetails payDetails { get; set; }
        //    public PayModeSpecificData payModeSpecificData { get; set; }
        //    public ResponseDetails responseDetails { get; set; }
        //}

        public class PayModeSpecificData
        {
            public List<string> subChannel { get; set; }
            public BankDetails bankDetails { get; set; }
            public CardDetails cardDetails { get; set; }
        }

        public class ProdDetail
        {
            public string prodName { get; set; }
            public double prodAmount { get; set; }
        }

        public class ResponseDetails
        {
            public string statusCode { get; set; }
            public string message { get; set; }
            public string description { get; set; }
        }

        public class Parent
        {
            public PayInstrument payInstrument { get; set; }
        }

        public class Rootobject
        {
            public PayInstrument payInstrument { get; set; }
        }
    }
}
