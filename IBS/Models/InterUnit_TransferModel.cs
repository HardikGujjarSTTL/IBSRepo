namespace IBS.Models
{
    public class InterUnit_TransferModel
    {
        public string VCHR_NO { get; set; }
        public string ACC_CD { get; set; }
        public decimal AMOUNT { get; set; }
        public string NARRATION { get; set; }
        public string CHQ_NO { get; set; }
        public DateTime CHQ_DT { get; set; }
        public string JV_NO { get; set; }
        public DateTime JV_DT { get; set; }
        public DateTime VCHR_DT { get; set; }
        public decimal CHQ_AMOUNT { get; set; }
        public decimal AMT_TRANSFERRED { get; set; }
        public decimal AVL_AMOUNTS { get; set; }
        public string PAYING_AUTHORITY { get; set; }
        public string REGION_TRANSFERRED { get; set; }
        public string BANK_NAME { get; set; }
        public int BANK_CD { get; set; }
        public string BPO { get; set; }
        public decimal SUSPENSE_AMT { get; set; }

        public int SNO { get; set; }

        public string ACC_DESC { get; set; }

        public string IU_ADV_NO { get; set; }
        public DateTime IU_ADV_DT { get; set; }
    }

    public class InterUnit_Transfer_JVModel
    {
        public string JV_NO { get; set; }
        public DateTime JV_DT { get; set; }
        public string VCHR_NO { get; set; }

        public DateTime VCHR_DT { get; set; }

    }
}
