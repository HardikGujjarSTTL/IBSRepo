using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class Rly_Designation_FormModel
    {
        public int ID { get; set; }

        [Display (Name = "Designation Code")]
        [Required]
        public string RlyDesigCd { get; set; } = null!;

        public string RlyDesigDesc { get; set; }

        public string? UserId { get; set; }

        public DateTime? Datetime { get; set; }

        public byte? Isdeleted { get; set; }

        public int? Createdby { get; set; }

        public int? Updatedby { get; set; }

        public DateTime? Createddate { get; set; }

        public DateTime? Updateddate { get; set; }
    }
}
