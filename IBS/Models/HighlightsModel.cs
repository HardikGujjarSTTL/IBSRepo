using System.ComponentModel.DataAnnotations;
using System.Composition;

namespace IBS.Models
{
    public class HighlightsModel
    {
        public string Region_Code { get; set; } = null!;

        public string High_Dt { get; set; } = null!;
        [Required]
        public string HighDtMon { get; set; } = null!;
        [Required]
        public string HighDtYear { get; set; } = null!;
        [Required]
        public string? Hight_Text { get; set; }

        public string? User_Id { get; set; }

        public DateTime? Datetime { get; set; }

        public byte? Isdeleted { get; set; }

        public DateTime? Createddate { get; set; }

        public int? Createdby { get; set; }

        public DateTime? Updateddate { get; set; }

        public int? Updatedby { get; set; }

    }
}
