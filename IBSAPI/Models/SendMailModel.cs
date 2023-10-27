using System.ComponentModel.DataAnnotations;

namespace IBSAPI.Models
{
    public class SendMailModel
    {
        [Required]
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string? Message { get; set; }
        public string? CC { get; set; }
        public string? Bcc { get; set;}
        public string? BodyFormat { get; set;}
    }
}
