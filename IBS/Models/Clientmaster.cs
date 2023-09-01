namespace IBS.Models
{
    public class Clientmaster
    {
        public int Id { get; set; }
        public string USER_NAME { get; set; }
        public string ORGANISATION { get; set; }
        public string DESIGNATION { get; set; }
        public string MOBILE { get; set; }
        public string EMAIL { get; set; }
        public string UNIT { get; set; }
        public string Orgn_Type { get; set; }
        public string Pwd { get; set; }
        public string ConfirmPassword { get; set; }
        public DateTime? Updateddate { get; set; }

        public DateTime? Createddate { get; set; }

        public int? Updatedby { get; set; }

        public int? Createdby { get; set; }

        public byte? Isdeleted { get; set; }
    }
}
