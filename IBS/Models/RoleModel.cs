namespace IBS.Models
{
    public class RoleModel
    {
        public decimal RoleId { get; set; }

        public string? Rolename { get; set; }

        public string? Roledescription { get; set; }

        public byte? Issysadmin { get; set; }

        public byte? Isactive { get; set; }

        public byte? Isdeleted { get; set; }

        public DateTime? Createddate { get; set; }

        public string? Createdby { get; set; }

        public DateTime? Updateddate { get; set; }

        public string? Updatedby { get; set; }
    }
}
