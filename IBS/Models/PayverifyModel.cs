namespace IBS.Models
{
    public class PayverifyModel
    {
        public class Payverify
        {
            public string atomTokenId { get; set; }

            public ResponseDetails responseDetails { get; set; }

            public Payverify()
            {
                //
                // TODO: Add constructor logic here
                //
            }
        }
        
        public class ResponseDetails
        {
            public string txnStatusCode { get; set; }
            public string txnMessage { get; set; }
            public string txnDescription { get; set; }
        }
    }
}
