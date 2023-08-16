using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class CentralQOIIModel
    {
        [Required]
        public string Client { get; set; } = null!;
        [Required]
        public string Weight { get; set; } = null!;
        [Required]
        public string QoiLength { get; set; } = null!;
        [Required]
        public long Accepted { get; set; }
        [Required]
        public long Rejected { get; set; }

        public string QoiDate { get; set; } = null!;
        [Required]
        public string Month { get; set; } = null!;
        [Required]
        public string Year { get; set; } = null!;

        public string? Region { get; set; }

        public string? UserId { get; set; }

        public DateTime? Datetime { get; set; }

        public int? Createdby { get; set; }

        public DateTimeOffset? Createddate { get; set; }

        public int? Updatedby { get; set; }

        public DateTimeOffset? Updateddate { get; set; }

        public byte? Isdeleted { get; set; }
        public bool IsEdited { get; set; }
    }
}
