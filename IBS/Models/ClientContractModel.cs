using System.ComponentModel.DataAnnotations;
using System.Composition;

namespace IBS.Models
{
    public class ClientContractModel
    {
        public int Id { get; set; }
        [Required]
        public DateTime VisitDt { get; set; }
        [Required]
        public string? ClientOfficerName { get; set; }
        [Required]
        public string? Designation { get; set; }

        [Required]
        public string? ClientType { get; set; }
        [Required]
        public string Client { get; set; } = null!;
        [Required]
        public int RitesOfficerCd { get; set; }

        public string? Highlights { get; set; }
        [Required]
        public string? OverallOutcome { get; set; }

        public string? RegionCd { get; set; }

        public string? UserId { get; set; }

        public DateTime? Datetime { get; set; }

        public string TypeCb { get; set; } = null!;
        [Required]
        public decimal? OutAmt { get; set; }

        public byte? Isdeleted { get; set; }

        public DateTime? Createddate { get; set; }

        public int? Createdby { get; set; }

        public DateTime? Updateddate { get; set; }

        public int? Updatedby { get; set; }


    }
}
