using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class CentralItemDetailModel
    {
        public int Id { get; set; }
        public int RailId { get; set; }
        [Required(ErrorMessage = "Rail Price Per Mt is required")]
        public string? RailPricePerMt { get; set; }
        [Required(ErrorMessage = "Packing Charge is required")]
        public string? PackingCharge { get; set; }
        [Required(ErrorMessage = "Price Date From is required")]
        public DateTime? PriceDateFr { get; set; }
        [Required(ErrorMessage = "Price Date To is required")]
        public DateTime? PriceDateTo { get; set; }
        public bool Isactive { get; set; }

        public int? Createdby { get; set; }

        public DateTime? Createddate { get; set; }

        public int? Updatedby { get; set; }

        public DateTime? Updateddate { get; set; }

        public byte? Isdeleted { get; set; }
    }
}
