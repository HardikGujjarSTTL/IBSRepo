using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class VendorMasterModel
    {
        public int VendCd { get; set; }

        [Display(Name = "Vendor Name")]
        [Required]
        public string? VendName { get; set; }

        [Display(Name = "Vendor Address")]
        [Required]
        public string? VendAdd1 { get; set; }

        public string? VendAdd2 { get; set; }

        [Display(Name = "City")]
        [Required]
        public int? VendCityCd { get; set; }

        public string? VendContactPer1 { get; set; }

        public string? VendContactTel1 { get; set; }

        public string? VendContactPer2 { get; set; }

        public string? VendContactTel2 { get; set; }

        public string? VendApproval { get; set; }

        [Display(Name = "Approval Period From")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        [RegularExpression(Common.RegularExpressionForDT, ErrorMessage = "Invalid date format.")]
        public DateTime? VendApprovalFr { get; set; }

        [Display(Name = "Approval Period To")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        [RegularExpression(Common.RegularExpressionForDT, ErrorMessage = "Invalid date format.")]
        public DateTime? VendApprovalTo { get; set; }

        public string? VendStatus { get; set; }

        [Display(Name = "Status Date From")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        [RegularExpression(Common.RegularExpressionForDT, ErrorMessage = "Invalid date format.")]
        public DateTime? VendStatusDtFr { get; set; }

        [Display(Name = "Status Date To")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        [RegularExpression(Common.RegularExpressionForDT, ErrorMessage = "Invalid date format.")]
        public DateTime? VendStatusDtTo { get; set; }

        public string? VendStatusDtFrST { get; set; }

        public string? VendStatusDtToST { get; set; }

        public string? VendRemarks { get; set; }

        public string? VendCdAlpha { get; set; }

        public string? UserId { get; set; }

        public DateTime? Datetime { get; set; }

        [Display(Name = "Email")]
        [EmailAddress]
        public string? VendEmail { get; set; }

        public string? VendInspStopped { get; set; }

        public string? VendPwd { get; set; }

        public string? OnlineCallStatus { get; set; }

        public string? OfflineCallStatus { get; set; }

        public byte? Isdeleted { get; set; }

        public int? Createdby { get; set; }

        public int? Updatedby { get; set; }

        public DateTime? Createddate { get; set; }

        public DateTime? Updateddate { get; set; }
        [Display(Name = "GSTIN No")]
        [RegularExpression(@"^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}Z[0-9A-Z]{1}$", ErrorMessage = "Invalid GSTIN No.")]
        [Required]
        public string? GSTNO { get; set; }

        public string? TANNO { get; set; }
        [Display(Name = "PAN No")]
        [RegularExpression(@"[A-Za-z]{5}[0-9]{4}[A-Za-z]{1}", ErrorMessage = "Invalid PAN number.")]
        [Required]
        public string? PANNO { get; set; }
    }
}
