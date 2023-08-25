using IBS.DataAccess;
using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class IE_CO_FormModel
    {
        public int CoCd { get; set; }

        [Display (Name = "Controlling Officer")]
        [Required]
        public string? CoName { get; set; }

        [Display(Name = "Designation")]
        [Required]
        public int? CoDesig { get; set; }

        public string? CoDesigName { get; set; }

        public string? CoRegion { get; set; }

        public string? CoPhoneNo { get; set; }

        [Display(Name = "Email")]
        [EmailAddress]
        public string? CoEmail { get; set; }

        public string? CoStatus { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        [RegularExpression(@"(?:(?:(?:0[1-9]|1\d|2[0-8])\/(?:0[1-9]|1[0-2])|(?:29|30)\/(?:0[13-9]|1[0-2])|31\/(?:0[13578]|1[02]))\/[1-9]\d{3}|29\/02(?:\/[1-9]\d(?:0[48]|[2468][048]|[13579][26])|(?:[2468][048]|[13579][26])00))", ErrorMessage = "Invalid date format.")]
        public DateTime? CoStatusDt { get; set; }

        public string? CoType { get; set; }

        public string? CoTypeName { get; set; }

        public byte? Isdeleted { get; set; }

        public int? Createdby { get; set; }

        public int? Updatedby { get; set; }

        public DateTime? Createddate { get; set; }

        public DateTime? Updateddate { get; set; }
    }
}

