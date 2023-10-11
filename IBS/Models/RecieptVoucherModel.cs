namespace IBS.Models
{
    public class RecieptVoucherModel
    {
        public string? VCHR_NO { get; set; }

        public string VCHR_DT { get; set; }

        public int? BANK_CD { get; set; }

        public string BANK_NAME { get; set; }

        public string VCHR_TYPE { get; set; }

        public int SNO { get; set; }

        public string CHQ_NO { get; set; }

        public string CHQ_DT { get; set; }

        public double AMOUNT { get; set; }

        public string ACC_CD { get; set; }

        public double AMOUNT_ADJUSTED { get; set; }

        public double SUSPENSE_AMT { get; set; }

        public string NARRATION { get; set; }

        public string SAMPLE_NO { get; set; }

        public DateTime POST_DT { get; set; }

        public string STATUS { get; set; }

        public string BPO_CD { get; set; }

        public string BPO_TYPE { get; set; }

        public string CASE_NO { get; set; }

        public double AMT_TRANSFERRED { get; set; }

        public string USER_ID { get; set; }

        public DateTime DATETIME { get; set; }

        public List<BPOlist> BPOList { get; set; }

        public List<VoucherDetailsModel> lstVoucherDetails { get; set; }
    }

    public class VoucherDetailsModel
    {
        public int ID { get; set; }

        public string CHQ_NO { get; set; }

        public int? BANK_CD { get; set; }

        public string BANK_NAME { get; set; }

        public DateTime? CHQ_DT { get; set; }

        public string Display_CHQ_DT { get { return this.CHQ_DT != null ? Common.ConvertDateFormat(this.CHQ_DT.Value) : ""; } }

        public decimal? AMOUNT { get; set; }

        public string SAMPLE_NO { get; set; }

        public int? ACC_CD { get; set; }

        public string ACC_NAME { get; set; }

        public string BPO_CD { get; set; }

        public string BPO_NAME { get; set; }

        public string BPO_TYPE { get; set; }

        public string CASE_NO { get; set; }

        public string NARRATION { get; set; }
    }

    public class ReceiptVoucherImportExcelModel
    {
        public string ChequeNo { get; set; }

        public DateTime? ChequeDate { get; set; }

        public decimal? Amount { get; set; }
    }

    public class BPODetailsModel
    {
        public string CASE_NO { get; set; }

        public string BPO_CD { get; set; }

        public string VEND_NAME { get; set; }

    }
}
