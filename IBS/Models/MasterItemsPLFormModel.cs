namespace IBS.Models
{
    public class MasterItemsPLFormModel
    {
        public string? ItemCd { get; set; }

        public string PlNo { get; set; } = null!;

        public string? UserId { get; set; }

        public DateTime? Datetime { get; set; }

        public byte? Isdeleted { get; set; }

        public int? Createdby { get; set; }

        public int? Updatedby { get; set; }

        public DateTime? Createddate { get; set; }

        public DateTime? Updateddate { get; set; }
    }
}