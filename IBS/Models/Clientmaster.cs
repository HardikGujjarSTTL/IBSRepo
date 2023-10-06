using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class Clientmaster
    {
        public int Id { get; set; }
        public string ClientName { get; set; }
        public string ORGANISATION { get; set; }
        public string Client_DESIGNATION { get; set; }
        public string MOBILE { get; set; }
        [EmailAddress]
        public string EMAIL { get; set; }
        public string UNIT { get; set; }
        public string Orgn_Type { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public DateTime? Updateddate { get; set; }

        public DateTime? Createddate { get; set; }

        public int? Updatedby { get; set; }

        public int? Createdby { get; set; }

        public byte? Isdeleted { get; set; }

        public string ShortCode { get; set; }
        public string ContactName { get; set; }
    }
}
