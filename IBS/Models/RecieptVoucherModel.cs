using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class RecieptVoucherModel
    {
        [Display(Name = "Voucher No")]
        [Required]
        public string? VCHR_NO { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        [Required]
        public DateTime? VCHR_DT { get; set; }

        public int? BANK_CD { get; set; }

        public string BANK_NAME { get; set; }
        public string VoucherDate { get; set; }
        public string AccountNo { get; set; }

        public string VCHR_TYPE { get; set; }

        public string Region { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string CHQ_NO { get; set; }

        public string CHQ_DT { get; set; }
        public string AMOUNT { get; set; }

        public string UserName { get; set; }

        public int? Createdby { get; set; }

        public DateTime? Createddate { get; set; }

        public int? Updatedby { get; set; }

        public DateTime? Updateddate { get; set; }

        public bool Isdeleted { get; set; }

        public bool IsNew { get; set; } = true;

        public List<VoucherDetailsModel> lstVoucherDetails { get; set; }
        public List<RecieptVoucherModel> lstBankStatement { get; set; }
    }

    public class VoucherDetailsModel
    {
        public int ID { get; set; }

        public string CHQ_NO { get; set; }

        public int BANK_CD { get; set; }

        public string BANK_NAME { get; set; }

        public DateTime CHQ_DT { get; set; }

        public string Display_CHQ_DT { get { return Common.ConvertDateFormat(this.CHQ_DT); } }

        public decimal? AMOUNT { get; set; }

        public string SAMPLE_NO { get; set; }

        public int? ACC_CD { get; set; }

        public string ACC_NAME { get; set; }

        public string BPO_CD { get; set; }

        public string BPO_NAME { get; set; }

        public string BPO_TYPE { get; set; }

        public string CASE_NO { get; set; }

        public string NARRATION { get; set; }

        public bool IsNew { get; set; } = true;
    }

    public class BPODetailsModel
    {
        public string CASE_NO { get; set; }

        public string BPO_CD { get; set; }

        public string VEND_NAME { get; set; }

    }
}
