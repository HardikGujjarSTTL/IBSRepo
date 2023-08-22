namespace IBS.Models
{
    public class AccountCodesDirectoryModel
    {
        public int AccCd { get; set; }

        public string? AccDesc { get; set; }

        public byte? Isdeleted { get; set; }

        public int? Createdby { get; set; }

        public int? Updatedby { get; set; }

        public DateTime? Createddate { get; set; }

        public DateTime? Updateddate { get; set; }
    }
}
