using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class Railway
    {
        [Display(Name = "Railway Code")]
        [Required]
        public string RLY_CD { get; set; }
        [Display(Name = "Railway")]
        [Required]
        public string RAILWAY { get; set; }
        public string? HEAD_QUARTER { get; set; }
        public string? USERID { get; set; }
        public string? DATETIME { get; set; }
        public string? IMMS_RLY_CD { get; set; }
        public string? IMMSRLyCD { get; set; }
        public int? Updatedby { get; set; }
        public int? Createdby { get; set; }
    }
}
