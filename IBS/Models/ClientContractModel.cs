using System.ComponentModel.DataAnnotations;
using System.Composition;

namespace IBS.Models
{
    public class ClientContractModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Date Of Contract is required")]
        public DateTime VisitDt { get; set; }
        [Required(ErrorMessage = "Officer Name Contracted is required")]
        public string? ClientOfficerName { get; set; }
        [Required(ErrorMessage = "Designation is required")]
        public string? Designation { get; set; }

        [Required(ErrorMessage = "Client Type is required")]
        public string? ClientType { get; set; }
        [Required(ErrorMessage = "Client is required")]
        public string Client { get; set; } = null!;
        [Required(ErrorMessage = "Rites Controlling Officer is required")]
        public int RitesOfficerCd { get; set; }

        public string? Highlights { get; set; }
        [Required(ErrorMessage = "Overall OutCome of the Visit is required")]
        public string? OverallOutcome { get; set; }

        public string? RegionCd { get; set; }

        public string? UserId { get; set; }

        public DateTime? Datetime { get; set; }

        public string TypeCb { get; set; } = null!;
        [Required(ErrorMessage = "Outstanding Amount(In Rs.) is required")]
        public decimal? OutAmt { get; set; }

        public byte? Isdeleted { get; set; }

        public DateTime? Createddate { get; set; }

        public int? Createdby { get; set; }

        public DateTime? Updateddate { get; set; }

        public int? Updatedby { get; set; }


    }
}
