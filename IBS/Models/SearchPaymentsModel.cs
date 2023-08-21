namespace IBS.Models
{
    public class SearchPaymentsModel
    {
        public string VCHR_NO { get; set; }
        public string VCHR_DT { get; set; }
        public int SNO { get; set; }
        public string BANK_CD { get; set; }
        public string BANK_NAME { get; set; }
        public string CHQ_NO { get; set; }
        public string CHQ_DT { get; set; }
        public decimal AMOUNT { get; set; }
        public decimal AMOUNT_ADJUSTED { get; set; }
        public decimal SUSPENSE_AMT { get; set; }
        public decimal AMT_TRANSFERRED { get; set; }
        public string BPO { get; set; }
        public string CASE_NO { get; set; }
        public string NARRATION { get; set; }
        public string ACC_CD { get; set; }
    }
}
