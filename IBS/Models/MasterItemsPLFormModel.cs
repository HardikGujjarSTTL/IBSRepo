using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class MasterItemsPLFormModel
    {
        [Display(Name = "Item")]
        [Required]
        public string? ItemCd { get; set; }

        public string? ItemDesc { get; set; }

        [Display(Name = "PL No")]
        [Required]
        public string PlNo { get; set; } = null!;

        public string? UserId { get; set; }

        public DateTime? Datetime { get; set; }

        public byte? Isdeleted { get; set; }

        public int? Createdby { get; set; }

        public int? Updatedby { get; set; }

        public DateTime? Createddate { get; set; }

        public DateTime? Updateddate { get; set; }

        public bool IsNew { get; set; } = true;
    }
}