namespace IBS.Models
{
    public class IE_MaximumCallLimitFormModel
    {
        public string RegionCode { get; set; } = null!;

        public byte? MaximumCall { get; set; }

        public string? UserId { get; set; }

        public DateTime? Datetime { get; set; }
        public byte? Isdeleted { get; set; }

        public int? Createdby { get; set; }

        public int? Updatedby { get; set; }

        public DateTime? Createddate { get; set; }

        public DateTime? Updateddate { get; set; }
    }
}
