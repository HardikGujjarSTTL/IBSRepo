using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class CityMasterModel
    {
        public int CityCd { get; set; }

        public string? Location { get; set; }

        [Required]
        public string? City { get; set; }

        [Display (Name ="State")]
        [Required]
        public byte? StateCd { get; set; }

        public string? State { get; set; }

        public int? CountryCd { get; set; }

        public string? Country { get; set; }

        public string? UserId { get; set; }

        public DateTime? Datetime { get; set; }

        [Display(Name = "Pin Code")]
        [Required]
        public string? PinCode { get; set; }

        public byte? Isdeleted { get; set; }

        public int? Createdby { get; set; }

        public int? Updatedby { get; set; }

        public DateTime? Createddate { get; set; }

        public DateTime? Updateddate { get; set; }
    }
}
