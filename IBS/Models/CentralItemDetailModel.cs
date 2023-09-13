using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class CentralItemDetailModel
    {
        public int Id { get; set; }
        public int RailId { get; set; }
        [Required]
        public string? RailPricePerMt { get; set; }
        [Required]
        public string? PackingCharge { get; set; }
        [Required]
        public DateTime? PriceDateFr { get; set; }
        [Required]
        public DateTime? PriceDateTo { get; set; }
        [Required]
        public bool Isactive { get; set; }

        public int? Createdby { get; set; }

        public DateTime? Createddate { get; set; }

        public int? Updatedby { get; set; }

        public DateTime? Updateddate { get; set; }

        public byte? Isdeleted { get; set; }
    }
}
