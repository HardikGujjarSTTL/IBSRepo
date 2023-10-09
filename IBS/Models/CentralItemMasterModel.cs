using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class CentralItemMasterModel
    {
        public int Id { get; set; }

        
        [Required(ErrorMessage = "Rail Cd is required")]
        public string? RailCd { get; set; }

        [Required(ErrorMessage = "Rail Description is required")]
        public string? RailDesc { get; set; }

        [Required(ErrorMessage = "Rail Length Meter is required")]
        public string? RailLengthMeter { get; set; }

        public string? UserId { get; set; }

        public int? Createdby { get; set; }

        public DateTime? Createddate { get; set; }

        public int? Updatedby { get; set; }

        public DateTime? Updateddate { get; set; }

        public byte? Isdeleted { get; set; }
    }
}
