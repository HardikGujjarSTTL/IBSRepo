using System.ComponentModel.DataAnnotations;

namespace IBSAPI.Models
{
    public class LoginModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public string UniqueId { get; set; }
    }

    public class ForgotPasswordModel
    {
        [Required(ErrorMessage = "Username or Email-Id is required.")]
        public string UserName { get; set; }
    }
}
