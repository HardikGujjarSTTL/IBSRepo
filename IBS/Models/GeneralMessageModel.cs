namespace IBS.Models
{
    public class GeneralMessageModel
    {
        public decimal MESSAGE_ID { get; set; }

        public string? MESSAGE { get; set; }

        public string? USER_ID { get; set; }

        public DateTimeOffset? DATETIME { get; set; }

        public decimal? Isdeleted { get; set; }

        public DateTimeOffset? Createddate { get; set; }

        public string? Createdby { get; set; }

        public DateTimeOffset? Updateddate { get; set; }

        public string? Updatedby { get; set; }
    }
}
