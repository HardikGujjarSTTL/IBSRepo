using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class ClientFeedbackModel
    {
        [Required]
        public string? Client { get; set; }
        [Required]
        public string? OffName { get; set; }
        [Required]
        public string? Mobile { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? RegionCode { get; set; }
        [Required]
        public int? Field1 { get; set; }
        [Required]
        public int? Field2 { get; set; }
        [Required]
        public int? Field3 { get; set; }
        [Required]
        public int? Field4 { get; set; }
        [Required]
        public int? Field5 { get; set; }
        [Required]
        public int? Field6 { get; set; }
        [Required]
        public int? Field7 { get; set; }
        [Required]
        public int? Field8 { get; set; }
        [Required]
        public int? Field9 { get; set; }
        [Required]
        public int? Field10 { get; set; }
        [Required]
        public int? Field11 { get; set; }
        [Required]
        public string? Field12 { get; set; }

        public int Id { get; set; }
    }
}
