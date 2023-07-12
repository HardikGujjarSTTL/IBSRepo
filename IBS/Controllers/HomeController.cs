using IBS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using IBS.Interfaces;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using System.Text;
using IBS.Helper;
using IBS.DataAccess;

namespace IBS.Controllers
{
    public class HomeController : BaseController
    {
        #region Variables
        private readonly IUserRepository userRepository;
        private readonly IWebHostEnvironment _env;
        public IConfiguration Configuration { get; }
        #endregion
        public HomeController(IUserRepository _userRepository, IWebHostEnvironment env, IConfiguration configuration)
        {
            userRepository = _userRepository;
            _env = env;
            Configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        //[ValidateDNTCaptcha(ErrorMessage = "Invalid security code.", CaptchaGeneratorLanguage = Language.English, CaptchaGeneratorDisplayMode = DisplayMode.ShowDigits)]
        public ActionResult Index(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                // username = anet
                T02User userMaster = userRepository.FindByLoginDetail(loginModel);
                if (userMaster != null)
                {
                    SetUserInfo = userMaster;
                    var userClaims = new List<Claim>()
                    {
                        new Claim("UserId", userMaster.UserId.ToString()),
                        new Claim("UserName", userMaster.UserName),
                        //new Claim("Email", userMaster.Email),
                        //new Claim("Mname", userMaster.Mname),
                        //new Claim("Lname", userMaster.Lname),
                        //new Claim(ClaimTypes.Email, userMaster.Email),
                        //new Claim(ClaimTypes.Role, userMaster.UserType.ToString()),
                     };
                    var userIdentity = new ClaimsIdentity(userClaims, "User Identity");
                    var userPrincipal = new ClaimsPrincipal(new[] { userIdentity });
                    HttpContext.SignInAsync(userPrincipal);
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    AlertDanger("Invalid Username or Password");
                }
            }
            return View(loginModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult ForgotPassword(ForgotPasswordModel loginModel)
        {
            if (ModelState.IsValid)
            {
                T02User userMaster = userRepository.FindByUsernameOrEmail(loginModel.UserName);
                if (userMaster != null)
                {

                    string body = System.IO.File.ReadAllText(System.IO.Path.Combine(_env.WebRootPath, "EmailTemplates", "ForgotPassword.html"), Encoding.UTF8);
                    body = body.Replace("{{USERNAME}}", userMaster.UserName ).Replace("{{RESETPASSURL}}", Configuration.GetSection("BaseURL").Value + Url.Action("ResetPassword", "Home", new { id = Common.EncryptQueryString(Convert.ToString(userMaster.UserId)) }));
                    EmailUtility emailUtility = new(Configuration);
                    string error = emailUtility.SendEmail(new EmailDetails
                    {
                        Body = body,
                        Subject = "Reset Password on IBS",
                        //ToEmailID = userMaster.Email,
                    });
                    if (string.IsNullOrEmpty(error))
                    {
                        AlertAddSuccess("Reset Password link has been sent to registered email id");
                        return RedirectToAction("Index", "Home");
                    }
                    else
                        AlertDanger(error);
                }
                else
                    AlertDanger("Invalid Username or Email-Id");
            }

            return View(loginModel);
        }

        public IActionResult ResetPassword(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                id = Common.DecryptQueryString(id.ToString());
            }
            UserModel user = userRepository.FindByID(Convert.ToInt32(id));
            ResetPasswordModel resetPassword = new() { UserId = Convert.ToInt32(id) };
            return View(resetPassword);
        }

        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordModel resetPassword, IFormCollection formCollection)
        {
            if (ModelState.IsValid)
            {
                var SaltedToken = Common.CreateRandomText(16);
                resetPassword.NewPassword = resetPassword.NewPassword;
                resetPassword.ConfirmPassword = resetPassword.ConfirmPassword;

                UserModel user = userRepository.FindByID(resetPassword.UserId);
                string UserId = formCollection["UserId"];
                if (Convert.ToInt32(user.UserId) != Convert.ToInt32(UserId))
                {
                    AlertDanger("User Name does not match.");
                    return View(user);
                }
                else if (user.UserName != resetPassword.UserName)
                {
                    AlertDanger("User Name does not match.");
                    return View(resetPassword);
                }
                else if (user.Password == resetPassword.NewPassword)
                {
                    AlertDanger("New password must be different from your old password.");
                    return View(resetPassword);
                }

                userRepository.ChangePassword(resetPassword);
                AlertUpdateSuccess("Password reset successfully.");
                return RedirectToAction("Index", "Home");
            }
            return View(resetPassword);
        }

        public IActionResult ChangePassword(string id)
        {
            //UserModel user = new() { IsActive = true };
            UserModel user = new UserModel();
            if (!string.IsNullOrEmpty(id))
            {
                int userid = Convert.ToInt32(Common.DecryptQueryString(id));
                if (userid > 0)
                {
                    user = userRepository.FindByID(userid);
                }
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword(UserModel user, IFormCollection formCollection)
        {
            if (Convert.ToInt32(user.UserId) > 0)
            {
                string SaltedToken = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz012345678912";
                string strOldPassword = formCollection["OldPassword"];
                string strNewPassword = formCollection["NewPassword"];

                user.Password = strNewPassword;

                UserModel userMaster = userRepository.FindByID(Convert.ToInt32(user.UserId));

                if (userMaster.Password != strOldPassword)
                { 
                    AlertDanger("Old Password does not match.");
                    return View(user);
                }
                else if (userMaster.Password == strNewPassword)
                {
                    AlertDanger("New password must be different from your old password.");
                    return View(user);
                }
                userRepository.ChangePassword(Convert.ToInt32(user.UserId), user.Password);
                AlertUpdateSuccess("Password has been changed successfully.");
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public IActionResult Logout()
        {
            SetUserInfo = null;
            HttpContext.Session.Clear();
            HttpContext.SignOutAsync("CookieAuthentication");
            return RedirectToAction("Index");
        }
    }
}
