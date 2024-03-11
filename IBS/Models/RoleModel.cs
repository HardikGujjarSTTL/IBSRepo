namespace IBS.Models
{
    public class RoleModel
    {
        public decimal RoleId { get; set; }
        public string EncryptedRoleId { get; set; }

        public string? Rolename { get; set; }

        public string? Roledescription { get; set; }

        public bool Issysadmin { get; set; }

        public bool Isactive { get; set; }

        public byte? Isdeleted { get; set; }

        public DateTime? Createddate { get; set; }

        public decimal? Createdby { get; set; }

        public DateTime? Updateddate { get; set; }

        public decimal? Updatedby { get; set; }

        public string? User_ID { get; set; }
        public string? UserName { get; set; }
        public int Id { get; set; }
        public string UserType { get; set; }
        public string? MUser_ID { get; set; }
    }
}
