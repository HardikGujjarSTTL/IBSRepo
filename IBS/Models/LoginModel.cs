using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Username is required.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }

    public class ForgotPasswordModel
    {
        [Required(ErrorMessage = "Username or Email-Id is required.")]
        public string UserName { get; set; }
    }

    public class ResetPasswordModel
    {
        [Required]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        public string UserName { get; set; }

        [Required, DataType(DataType.Password), Display(Name = "New password")]
        public string NewPassword { get; set; }

        [Required, DataType(DataType.Password), Display(Name = "Re enter new password")]
        [Compare("NewPassword", ErrorMessage = "Confirm password does not match")]
        public string ConfirmPassword { get; set; }
    }
}
