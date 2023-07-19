using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class IE_Instructions_AdminModel
    {
        public int MessageId { get; set; }

        public string? LetterNo { get; set; }

        public DateTime? LetterDt { get; set; }

        public string? Message { get; set; }

        public string? UserId { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? Datetime { get; set; }

        public string RegionCode { get; set; } = null!;

        public DateTime? MessageDt { get; set; }

        public string? Createdby { get; set; }

        public string? Updatedby { get; set; }

        public DateTimeOffset? Createddate { get; set; }

        public DateTimeOffset? Updateddate { get; set; }

        public decimal? Isdeleted { get; set; }
    }
}
