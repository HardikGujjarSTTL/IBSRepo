using System.Composition;

namespace IBS.Models
{
    public class LastYearOutstandingModel    
    {
        public string Ly_Per { get; set; } = null!;

        public decimal? Ly_Outs { get; set; }

        public string LyPerMon { get; set; }

        public string LyPerYear { get; set; }

        public string Region_Code { get; set; } = null!;

        public string? User_Id { get; set; }

        public DateTime? Datetime { get; set; }

        public byte? Isdeleted { get; set; }

        public DateTime? Createddate { get; set; }

        public string? Createdby { get; set; }

        public DateTime? Updateddate { get; set; }

        public string? Updatedby { get; set; }

    }
}
