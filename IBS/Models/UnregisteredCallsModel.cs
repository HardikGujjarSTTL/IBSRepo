using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class UnregisteredCallsModel
    {
        [Display(Name = "IE Name")]
        [Required]
        public int IeCd { get; set; }

        public string? YrMth { get; set; }

        public byte? UnregCalls { get; set; }

        public string? Region { get; set; }

        public string? IeName { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public bool IsNew { get; set; } = true;
    }
}
