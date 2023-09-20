using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class CentralItemMasterModel
    {
        public int Id { get; set; }

        [Required]
        public string? RailCd { get; set; }

        [Required]
        public string? RailDesc { get; set; }

        public string? RailLengthMeter { get; set; }

        public string? UserId { get; set; }

        public int? Createdby { get; set; }

        public DateTime? Createddate { get; set; }

        public int? Updatedby { get; set; }

        public DateTime? Updateddate { get; set; }

        public byte? Isdeleted { get; set; }
    }
}
