namespace IBS.Models
{
    public class IECliamFormModel
    {
        public string CLAIM_NO { get; set; }

        public string CLAIM_DT { get; set; }

        public int IE { get; set; }

        public int AMOUNT_CLAIMED { get; set; }

        public string CLAIM_RECIEVE_DT { get; set; }
        public int AMOUNT_ADMITTED  { get; set; }

        public int AMOUNT_DISALLOWED { get; set; }

        public string REMARKS { get; set; }

        public string IE_NAME { get; set; }

        public string PERIOD_FROM_YEAR{ get; set;}

        public string PERIOD_FROM_MONTH { get; set; }
        public string PERIOD_TO_MONTH { get; set; }
        public string PERIOD_TO_YEAR { get; set; }

        public int PERIOD_FROM { get; set; }
        public decimal ID { get; set; }

        public string PAYMENT_VOUCHER_NUMBER { get; set; }

        public string PAYMENT_VOUCHER_DATE { get; set; }


        public int PERIOD_TO { get; set; }

        public string CLAIM_HEAD { get; set;}

        public string CLAIM_HEAD_NAME { get; set; }




    }
}
