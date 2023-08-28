using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class AccountCodesDirectoryModel
    {
        [Display(Name = "Account Code")]
        [Required]
        public int? AccCd { get; set; }

        public string? AccountCode { get; set; }

        public string? AccDesc { get; set; }

        public byte? Isdeleted { get; set; }

        public int? Createdby { get; set; }

        public int? Updatedby { get; set; }

        public DateTime? Createddate { get; set; }

        public DateTime? Updateddate { get; set; }

        public bool IsNew { get; set; } = true;
    }
}
