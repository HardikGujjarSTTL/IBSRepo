using System.ComponentModel.DataAnnotations;

namespace IBSAPI.Models
{
    public class LoginModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        //[Required]
        //public string UniqueId { get; set; }
    }
}
