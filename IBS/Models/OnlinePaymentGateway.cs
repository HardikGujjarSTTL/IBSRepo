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
        public DateTime CallDate { get; set; }
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
        public decimal? Charges { get; set; }
        public string MER_TXN_REF { get; set; }
        public string ChargesType { get; set; }
        public int? ORDER_INFO { get; set; }
        public string Tok_id { get; set; }
        public string DATETIME { get; set; }
    }
}
