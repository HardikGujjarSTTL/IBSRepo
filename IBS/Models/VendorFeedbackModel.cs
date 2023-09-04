using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class VendorFeedbackModel
    {
        [Required]
        public int? VendCd { get; set; }
        [Required]
        public string? RegionCode { get; set; }
        [Required]
        public int Field1 { get; set; }
        [Required]
        public int Field2 { get; set; }
        [Required]
        public int Field3 { get; set; }
        [Required]
        public int Field4 { get; set; }
        [Required]
        public int Field5 { get; set; }
        [Required]
        public int Field6 { get; set; }
        [Required]
        public int Field7 { get; set; }
        [Required]
        public int Field8 { get; set; }
        [Required]
        public string? Field9 { get; set; }
        [Required]
        public string? Field10 { get; set; }

        public string? MthyrPeriod { get; set; }

        public string VendEmail { get; set; }
    }
}
