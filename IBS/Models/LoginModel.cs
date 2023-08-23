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

    public class UserSessionModel
    {
        public string LoginType { get; set; }
        public int UserID { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Region { get; set; }
        public string AuthLevl { get; set; }
        public int IeCd { get; set; }
        public int RoleId { get; set; }
        public string Organisation { get; set; }
        public string OrgnType { get; set; }
        public string RoleName { get; set; }
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
