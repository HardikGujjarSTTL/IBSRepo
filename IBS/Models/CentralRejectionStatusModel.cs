using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class CentralRejectionStatusModel
    {
        public string? RejDt { get; set; }
        [Required]
        public string? CaseNo { get; set; }
        [Required]
        public string? Consignee { get; set; }
        [Required]
        public string? DesCom { get; set; }
        [Required]
        public string? Conclusion { get; set; }

        public string? Region { get; set; }

        public string? UserId { get; set; }

        public DateTime? Datetime { get; set; }

        public int Id { get; set; }
        [Required]
        public string Month { get; set; } = null!;
        [Required]
        public string Year { get; set; } = null!;

        public int? Createdby { get; set; }

        public DateTimeOffset? Createddate { get; set; }

        public int? Updatedby { get; set; }

        public DateTimeOffset? Updateddate { get; set; }

        public byte? Isdeleted { get; set; }
    }
}
