using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class CentralQOIIModel
    {
        [Required(ErrorMessage = "Client is required")]
        public string Clients { get; set; }
        [Required(ErrorMessage = "Section is required")]
        public string Weights { get; set; }
        [Required(ErrorMessage = "Length is required")]
        public string QoiLengths { get; set; }
        [Required(ErrorMessage = "Accepted Quantity(mt) is required")]
        public long Accepted { get; set; }
        [Required(ErrorMessage = "Rejected Quantity(mt) is required")]
        public long Rejected { get; set; }

        public string QoiDate { get; set; }
        [Required(ErrorMessage = "For The Period Month is required")]
        public string Month { get; set; }
        [Required(ErrorMessage = "For The Period year is required")]
        public string Year { get; set; }

        public string? Region { get; set; }

        public string? UserId { get; set; }

        public DateTime? Datetime { get; set; }

        public int? Createdby { get; set; }

        public DateTimeOffset? Createddate { get; set; }

        public int? Updatedby { get; set; }

        public DateTimeOffset? Updateddate { get; set; }

        public byte? Isdeleted { get; set; }
        public bool IsEdited { get; set; }

        //[Required(ErrorMessage = "Grade is required")]
        public string? Grade { get; set; }
    }
}
