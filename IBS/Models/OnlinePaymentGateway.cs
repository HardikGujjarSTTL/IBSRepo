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
        [Required]
        [Display(Name = "Charges")]
        public decimal? Charges { get; set; }
        public string MER_TXN_REF { get; set; }
        [Required]
        [Display(Name = "Charges Type")]
        public string ChargesType { get; set; }
        public int? ORDER_INFO { get; set; }
        public string Tok_id { get; set; }
        public string DATETIME { get; set; }
        public string MerchantID { get; set; }
        public string OrderInfo { get; set; }
        public string TransactionAmount { get; set; }
        public string TranRespCode { get; set; }
        public string TranRespCodeDesc { get; set; }
        public string PaymentServerMsg { get; set; }
        public string AcquirerRespCode { get; set; }
        public string ShoppingTranNO { get; set; }
        public string ReceiptNo { get; set; }
        public string AuthorizationID { get; set; }
        public string BatchNo { get; set; }
        public string CardType { get; set; }
        public string CSCResultCode { get; set; }
        public string CSCResultDescrip { get; set; }
        public string ECI { get; set; }
        public string XID { get; set; }
        public string Enrolled { get; set; }
        public string Status { get; set; }
        public string VerToken { get; set; }
        public string VerType { get; set; }
        public string VerSecurityLevel { get; set; }
        public string VerStatus { get; set; }
        public string RiskOverallResult { get; set; }
        public string TranReversalResult { get; set; }
    }
}
