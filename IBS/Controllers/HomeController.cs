using DocumentFormat.OpenXml.Office2010.Excel;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using static IBS.Helper.Enums;

namespace IBS.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IUserRepository userRepository;
        private readonly IWebHostEnvironment _env;
        public IConfiguration Configuration { get; }
        private readonly IConfiguration config;
        private readonly ISendMailRepository pSendMailRepository;

        public HomeController(IUserRepository _userRepository, IWebHostEnvironment env, IConfiguration configuration, ISendMailRepository _pSendMailRepository, IConfiguration _config)
        {
            userRepository = _userRepository;
            _env = env;
            Configuration = configuration;
            pSendMailRepository = _pSendMailRepository;
            this.config = _config;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel loginModel)
        {
            UserModel model = userRepository.CheckPasswordIsBlank(loginModel.UserName, loginModel.UserType);
            if (model != null)
            {
                if (model.Password == null || model.Password == "")
                {
                    string id = Common.EncryptQueryString(Convert.ToString(loginModel.UserName.Trim()));
                    string UserType = Common.EncryptQueryString(Convert.ToString(loginModel.UserType));
                    return RedirectToAction("ResetPassword", "Home", new { id = id, UserType = UserType });
                }
                else
                {
                    if (model.Password != null && loginModel.Password == null)
                    {
                        AlertDanger("Password is required.");
                        return RedirectToAction("Index");
                    }
                }
            }
            if (loginModel.Password != null)
            {
                //AlertDanger("Password is required.");
                //return RedirectToAction("Index");
            }
            string encryptedPassword = Common.getEncryptedText(loginModel.Password, "301ae92bb2bc7599");
            //string DecryptPassword = Common.getDecryptedText(loginModel.Password, "301ae92bb2bc7599");

            loginModel.Password = encryptedPassword;
            UserSessionModel userMaster = userRepository.LoginByUserPass(loginModel);
            if (userMaster != null)
            {
                //// temporary Commited - for local
                //if (userMaster.MOBILE != null && userMaster.MOBILE != "")
                if (1 == 1)
                {
                    string sender = "RITES/QA";
                    Random random = new Random();
                    string otp = Convert.ToString(random.Next(1000, 9999));
                    string message = otp + " is the One Time Password for verification of your login with RITES LTD- QA Division. Valid for 10 minutes. Please do not share with anyone." + "-" + sender;
                    //// temporary Commited - for local 
                    //string responce = Models.Common.SendOTP(userMaster.MOBILE, message);
                    //loginModel.OTP = otp;
                    loginModel.OTP = "123";
                    loginModel.MOBILE = userMaster.MOBILE;
                    userRepository.SaveOTPDetails(loginModel);
                    if (!string.IsNullOrEmpty(userMaster.Email))
                    {
                        userRepository.send_Vendor_Email(loginModel, userMaster.Email);
                    }
                    string EncryptUserName = Common.EncryptQueryString(loginModel.UserName);
                    string EncryptUserType = Common.EncryptQueryString(loginModel.UserType);
                    return RedirectToAction("OTPVerification", "Home", new { UserName = EncryptUserName, UserType = EncryptUserType });
                }
                else
                {
                    AlertDanger("Mobile no. does not exist");
                }
            }
            else
            {
                AlertDanger("Invalid Username or Password.");
            }
            return RedirectToAction("Index");
        }

        public ActionResult OTPVerification(string UserName, string UserType)
        {
            string DecryptUserName = Common.DecryptQueryString(UserName);
            string DecryptUserType = Common.DecryptQueryString(UserType);
            LoginModel loginModel1 = new LoginModel();
            loginModel1.DecryptUserName = DecryptUserName;
            loginModel1.DecryptUserType = DecryptUserType;
            return View(loginModel1);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OTPProceed(LoginModel loginModel)
        {
            loginModel.UserName = loginModel.DecryptUserName;
            loginModel.UserType = loginModel.DecryptUserType;
            if (userRepository.VerifyOTP(loginModel))
            {
                UserSessionModel userMaster = userRepository.FindByLoginDetail(loginModel);
                if (userMaster != null)
                {
                    if (userMaster.LoginType == "IE")
                    {
                        X509Certificate2 cert = DigitalSigner.getCertificate("minesh vinodchandra doshi");

                        //X509Certificate2 cert = DigitalSigner.getCertificate("bikey3383@gmail.com");
                        //X509Certificate2 cert = DigitalSigner.getCertificate("dhiren.parmar@silvertouch.com");

                        if (cert == null)
                        {
                            return Json(new { status = false, responseText = "Kindly Attached Certificate!!" });
                        }
                        else
                        {
                            DateTime? DSC_Exp_DT = cert.NotAfter;

                            if(DSC_Exp_DT.Value.Date < DateTime.Now.Date)
                            {
                                return Json(new { status = false, responseText = "DSC Expiry date cannot be earlier then current date." });
                            }
                            else
                            {
                                DateTime? DSC_Exp_DT1 = userRepository.GetDSC_Exp_DT(userMaster.IeCd);

                                if(DSC_Exp_DT1 == null || (DSC_Exp_DT.Value.Date != DSC_Exp_DT1.Value.Date))
                                {
                                   string DSCUpdate = userRepository.UpdateDSCDate(userMaster.IeCd, DSC_Exp_DT.Value);
                                }
                            }
                        }
                    }
                    SetUserInfo = userMaster;
                    var userClaims = new List<Claim>()
                    {
                        new Claim("Name", Convert.ToString(userMaster.Name)),
                        new Claim("UserName", Convert.ToString(userMaster.UserName)),
                        new Claim("UserID", userMaster.UserID.ToString()),
                        //new Claim("LoginType", userMaster.LoginType.ToString()),
                        new Claim("Region", userMaster.Region != null ? userMaster.Region.ToString() : ""),
                        new Claim("AuthLevl", userMaster.AuthLevl != null ? userMaster.AuthLevl.ToString() : ""),
                        new Claim("RoleId", Convert.ToString(userMaster.RoleId)),
                        new Claim("RoleName", userMaster.RoleName != null ? Convert.ToString(userMaster.RoleName) : ""),
                        new Claim("OrgnTypeL", (userMaster.OrgnTypeL != null && userMaster.OrgnTypeL != "") ? Convert.ToString(userMaster.OrgnTypeL) : ""),
                        new Claim("OrganisationL", (userMaster.OrganisationL != null && userMaster.OrganisationL != "") ? Convert.ToString(userMaster.OrganisationL) : ""),
                        new Claim("OrgnType", (userMaster.OrgnType != null && userMaster.OrgnType != "") ? Convert.ToString(userMaster.OrgnType) : ""),
                        new Claim("Organisation", (userMaster.Organisation != null && userMaster.Organisation != "") ? Convert.ToString(userMaster.Organisation) : ""),
                        new Claim("UserMasterID",Convert.ToString(userMaster.MasterID)),
                    };
                    var userIdentity = new ClaimsIdentity(userClaims, "User Identity");
                    var userPrincipal = new ClaimsPrincipal(new[] { userIdentity });
                    HttpContext.SignInAsync(userPrincipal);

                    SessionHelper.MenuModelDTO = userRepository.GenerateMenuListByRoleId(userMaster.RoleId);
                    return Json(new { status = true, responseText = "", RoleName = Convert.ToString(IBS.Helper.SessionHelper.UserModelDTO.RoleName) });
                    //return RedirectToAction("Index", "Dashboard");
                }
            }
            else
            {
                //AlertDanger("Invalid OTP/OTP Expired");
                //string EncryptUserName = Common.EncryptQueryString(loginModel.UserName);
                //return RedirectToAction("OTPVerification", new { UserName = EncryptUserName });
                return Json(new { status = false, responseText = "Invalid OTP/OTP Expired" });
            }
            return View(loginModel);
        }

        public IActionResult RegenerateOTP(string UserName, string UserType)
        {
            LoginModel loginModel = new LoginModel();
            loginModel.UserName = UserName;
            loginModel.UserType = UserType;
            UserSessionModel userMaster = userRepository.LoginByUserName(loginModel);
            loginModel.MOBILE = userMaster.MOBILE;

            string sender = "RITES/QA";
            Random random = new Random();
            string otp = Convert.ToString(random.Next(1000, 9999));
            string message = otp + " is the One Time Password for verification of your login with RITES LTD- QA Division. Valid for 10 minutes. Please do not share with anyone." + "-" + sender;
            //// temporary Commited - for local 
            //string responce = Common.SendOTP(loginModel.MOBILE, message);
            //loginModel.OTP = otp;
            loginModel.OTP = "123";
            userRepository.SaveOTPDetails(loginModel);

            if (!string.IsNullOrEmpty(userMaster.Email))
            {
                userRepository.send_Vendor_Email(loginModel, userMaster.Email);
            }
            string EncryptUserName = Common.EncryptQueryString(loginModel.UserName);
            return RedirectToAction("OTPVerification", "Home", new { UserName = EncryptUserName });
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

        public IActionResult ForgotPassword(string usertype)
        {
            ForgotPasswordModel forgotPasswordModel = new ForgotPasswordModel();
            forgotPasswordModel.UserType = usertype;
            return View(forgotPasswordModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult ForgotPassword(ForgotPasswordModel loginModel)
        {
            if (ModelState.IsValid)
            {
                UserSessionModel userMaster = userRepository.FindByUsernameOrEmail(loginModel.UserName, loginModel.UserType);
                if (userMaster != null)
                {

                    string body = System.IO.File.ReadAllText(System.IO.Path.Combine(_env.WebRootPath, "EmailTemplates", "ForgotPassword.html"), Encoding.UTF8);
                    body = body.Replace("{{USERNAME}}", userMaster.UserName).Replace("{{RESETPASSURL}}", Configuration.GetSection("BaseURL").Value + Url.Action("ResetPassword", "Home", new { id = Common.EncryptQueryString(Convert.ToString(userMaster.FPUserID)), UserType = Common.EncryptQueryString(Convert.ToString(loginModel.UserType)) }));
                    EmailUtility emailUtility = new(Configuration);
                    //string error = emailUtility.SendEmail(new EmailDetails
                    //{
                    //    Body = body,
                    //    Subject = "Reset Password on IBS",
                    //    //ToEmailID = userMaster.Email,
                    //});

                    bool isSend = false;
                    if (Convert.ToString(config.GetSection("MailConfig")["SendMail"]) == "1")
                    {
                        SendMailModel sendMailModel = new SendMailModel();
                        sendMailModel.To = userMaster.Email;
                        sendMailModel.Subject = "Reset Password on IBS";
                        sendMailModel.Message = body;
                        isSend = pSendMailRepository.SendMail(sendMailModel, null);
                    }

                    if (isSend)
                    {
                        AlertAddSuccess("Reset Password link has been sent to registered email id");
                        return RedirectToAction("Index", "Home");

                    }
                    else
                    {
                        AlertAddSuccess("Error");
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                    AlertDanger("Invalid Username or Email-Id");
            }

            return View(loginModel);
        }

        public IActionResult ResetPassword(string id, string UserType)
        {
            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(UserType))
            {
                id = Common.DecryptQueryString(id.ToString());
                UserType = Common.DecryptQueryString(UserType.ToString());
            }
            ResetPasswordModel resetPassword = new() { UserId = id, UserType = UserType, UserName = id };
            return View(resetPassword);
        }

        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordModel resetPassword, IFormCollection formCollection)
        {
            if (ModelState.IsValid)
            {
                var SaltedToken = Common.CreateRandomText(16);
                resetPassword.NewPassword = Common.getEncryptedText(resetPassword.NewPassword, "301ae92bb2bc7599");
                resetPassword.ConfirmPassword = Common.getEncryptedText(resetPassword.ConfirmPassword, "301ae92bb2bc7599");
                resetPassword.UserType = Common.DecryptQueryString(resetPassword.UserType);
                UserModel user = userRepository.FindByIDForResetPass(Convert.ToString(resetPassword.UserId), resetPassword.UserType);
                string UserId = formCollection["UserId"];

                if (user.FPUserID.Trim() != resetPassword.UserName.Trim())
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
                string userid = Common.DecryptQueryString(id);
                if (userid != null)
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
                //string SaltedToken = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz012345678912";
                string strOldPassword = formCollection["OldPassword"];
                string strNewPassword = formCollection["NewPassword"];

                user.Password = strNewPassword;

                string LoginType = IBS.Helper.SessionHelper.UserModelDTO.LoginType;
                UserModel userMaster = userRepository.FindByUserID(user.UserId, LoginType);

                string encryptedOldPassword = Common.getEncryptedText(strOldPassword, "301ae92bb2bc7599");
                if (userMaster.Password != encryptedOldPassword)
                {
                    AlertDanger("Old Password does not match.");
                    return View(user);
                }
                else if (userMaster.Password == strNewPassword)
                {
                    AlertDanger("New password must be different from your old password.");
                    return View(user);
                }
                string encryptedPassword = Common.getEncryptedText(user.Password, "301ae92bb2bc7599");
                userRepository.ChangePassword(user.UserId, encryptedPassword, LoginType);
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

        [HttpGet]
        public ActionResult UserAccessDenied()
        {
            return View();
        }


        [HttpGet("CreateQueryForEncryptPasswords")]
        public ActionResult<string> CreateQueryForEncryptPasswords(string UserType)
        {
            string result = "";
            List<UserModel> obj = userRepository.FindByUserType(UserType);

            if (obj != null)
            {
                StringBuilder queryBuilder = new StringBuilder();

                foreach (var item in obj)
                {
                    string encryptedPassword = Common.getEncryptedText(item.Password, "301ae92bb2bc7599");

                    switch (UserType)
                    {
                        case "USERS":
                            queryBuilder.AppendLine($"UPDATE t02_users SET PASSWORD = '{encryptedPassword}' WHERE PASSWORD = '{item.Password}';");
                            break;
                        case "VENDOR":
                            queryBuilder.AppendLine($"UPDATE t05_vendor SET VEND_PWD = '{encryptedPassword}' WHERE VEND_PWD = '{item.Password}';");
                            break;
                        case "IE":
                            queryBuilder.AppendLine($"UPDATE t09_ie SET IE_PWD = '{encryptedPassword}' WHERE IE_PWD = '{item.Password}';");
                            break;
                        case "CLIENT_LOGIN":
                            queryBuilder.AppendLine($"UPDATE t32_client_login SET PWD = '{encryptedPassword}' WHERE PWD = '{item.Password}';");
                            break;
                        case "LO_LOGIN":
                            queryBuilder.AppendLine($"UPDATE t105_lo_login SET PWD = '{encryptedPassword}' WHERE PWD = '{item.Password}';");
                            break;
                            // Add additional cases as needed
                    }
                }

                result = queryBuilder.ToString();
            }

            return Ok(result);
        }

        //[HttpGet("Encrypted")]
        //public ActionResult<string> Encrypted(string Text)
        //{
        //    string encryptedPassword = Common.getEncryptedText(Text, "301ae92bb2bc7599");
        //    return Ok(encryptedPassword);
        //}

        //[HttpGet("Decrypted")]
        //public ActionResult<string> Decrypted(string Text)
        //{
        //    string DecryptedPassword = Common.getDecryptedText(Text, "301ae92bb2bc7599");
        //    return Ok(DecryptedPassword);
        //}
    }
}
