using System.ComponentModel.DataAnnotations;
using System.Composition;

namespace IBS.Models
{
    public class ExpenditureModel
    {
        public string RegionCode { get; set; } = null!;

        public string ExpPer { get; set; } = null!;
        [Required]
        public string ExpPerMonth { get; set; } = null!;
        [Required]
        public string ExpPerYear { get; set; } = null!;
        [Required]
        public decimal? ExpAmt { get; set; }
        [Required]
        public decimal? TaxAmt { get; set; }

        public string? UserId { get; set; }

        public DateTime? Datetime { get; set; }

        public byte? Isdeleted { get; set; }

        public DateTime? Createddate { get; set; }

        public int? Createdby { get; set; }

        public DateTime? Updateddate { get; set; }

        public int? Updatedby { get; set; }

    }
}
