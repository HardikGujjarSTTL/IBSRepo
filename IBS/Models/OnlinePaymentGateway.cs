using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class OnlinePaymentGateway
    {
        [Required]
        [Display(Name = "Case No")]
        public string CaseNo { get; set; }
        [Required]
        [Display(Name = "Call Date")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        //[DataType(DataType.Date)]
        public string CallDate { get; set; }
        [Required]
        [Display(Name = "Call Sno")]
        public int? CallSno { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string? Email { get; set; }
        [Required]
        [Display(Name = "Mobile No")]
        public string? Mobile { get; set; }
        public string? PO_NO { get; set; }
        public DateTime? PO_DT { get; set; }
        public string? VEND_NAME { get; set; }
        public string? AlertMsg { get; set; }
        public string? VendAdd1 { get; set; }
        public string? Location { get; set; }
        public string? City { get; set; }
        public int? VEND_CD { get; set; }
        [Required]
        [Display(Name = "Charges")]
        public decimal? Charges { get; set; }
        public string MER_TXN_REF { get; set; }
        [Required]
        [Display(Name = "Charges Type")]
        public string ChargesType { get; set; }
        public string Product { get; set; }
        public string MERTXNID { get; set; }
        public string LocalURL { get; set; }
        public string BankTXNID { get; set; }
        public string BankName { get; set; }
        public string PaymentStatus { get; set; }
        public string TranDate { get; set; }
        public string Tok_id { get; set; }
        public string MerID { get; set; }
        public string merchTxnDate { get; set; }
        public string AtomTXNID { get; set; }
        public string custAccNo { get; set; }
        public int BankID { get; set; }
        public string SubChannel { get; set; }
        public string Description { get; set; }
        public string StatusCode { get; set; }
        public List<PaymentList> lstPaymentList { get; set; }
    }

    public class PaymentList
    {
        public string MerID { get; set; }
        public string MER_TXN_REF { get; set; }
        public string merchTxnDate { get; set; }
        public string MERTXNID { get; set; }
        public decimal? Charges { get; set; }
        public string AtomTXNID { get; set; }
        public string Status { get; set; }
    }
}
