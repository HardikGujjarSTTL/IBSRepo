namespace IBS.Models
{
    public class Allow_Old_Bill_DateModel
    {
        public string Region { get; set; } = null!;

        public string? AllowOldBillDt { get; set; }

        public int? GraceDays { get; set; }

        public string? Createdby { get; set; }

        public string? Updatedby { get; set; }

        public DateTimeOffset? Createddate { get; set; }

        public DateTimeOffset? Updateddate { get; set; }

        public decimal? Isdeleted { get; set; }
    }
}
