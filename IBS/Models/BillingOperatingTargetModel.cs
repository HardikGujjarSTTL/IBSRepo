using System.ComponentModel.DataAnnotations;
using System.Composition;

namespace IBS.Models
{
    public class BillingOperatingTargetModel
    {
        [Required]
        public string Be_Per { get; set; } = null!;
        [Required]
        public decimal? B_Target { get; set; }

        [Required]
        public decimal? E_Target { get; set; }

        public string Region_Code { get; set; } = null!;

        public string? User_Id { get; set; }

        public DateTime? Datetime { get; set; }
        [Required]
        public decimal? Ex_Target { get; set; }

        public byte? Isdeleted { get; set; }

        public DateTime? Createddate { get; set; }

        public string? Createdby { get; set; }

        public DateTime? Updateddate { get; set; }

        public string? Updatedby { get; set; }

    }
}
