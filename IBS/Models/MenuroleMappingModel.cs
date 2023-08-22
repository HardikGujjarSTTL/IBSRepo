namespace IBS.Models
{
    public class MenuroleMappingModel
    {
        public int Id { get; set; }

        public int? Roleid { get; set; }
        public string? rolename { get; set; }
        public string? titles { get; set; }

        public int? Menuid { get; set; }

        public bool? Isactive { get; set; }

        public int? Createdby { get; set; }

        public DateTimeOffset? Createddate { get; set; }

        public int? Updatedby { get; set; }

        public DateTimeOffset? Updateddate { get; set; }

        public byte? Isdeleted { get; set; }
    }
    public class MenuroleMappingListModel
    {
        public int? Roleid { get; set; }
        public string? rolename { get; set; }
        public string? titles { get; set; }
    }
}
