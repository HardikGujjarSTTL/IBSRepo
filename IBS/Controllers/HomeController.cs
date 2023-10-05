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
using IBS.Repositories;
using Microsoft.AspNetCore.Hosting;

namespace IBS.Controllers
{
    public class HomeController : BaseController
    {
        #region Variables
        private readonly IUserRepository userRepository;
        private readonly IWebHostEnvironment _env;
        private readonly ILabInvoiceDownloadRepository LabInvoiceDownloadRepository;
        public IConfiguration Configuration { get; }
        #endregion
        public HomeController(IUserRepository _userRepository, IWebHostEnvironment env, IConfiguration configuration, ILabInvoiceDownloadRepository _LabInvoiceDownloadRepository)
        {
            userRepository = _userRepository;
            _env = env;
            Configuration = configuration;
            LabInvoiceDownloadRepository = _LabInvoiceDownloadRepository;

        }

        public IActionResult Index()
        {
            //HttpContext.Session.SetString("LoginType", type);
            return View();
        }
        //public IActionResult Download()
        //{
        //    //string Regin = GetRegionCode;
        //    //DataSet dTResult = LabInvoiceDownloadRepository.Download(CaseNo, RegNo, InvoiceNo, TranNo);
        //    string CaseNo= "N22050400", RegNo= "N/22/0780", InvoiceNo= "R0608L22/00001",  TranNo="";
        //    string Srno = LabInvoiceDownloadRepository.GetSrNo(InvoiceNo);
        //    LabInvoiceDownloadModel dtreg = LabInvoiceDownloadRepository.Getdtreg(InvoiceNo);
        //    ReportDocument rd = new ReportDocument();
        //    string reportPath = "";
        //    if (Convert.ToInt32(Srno) > 3)
        //    {
        //        //reportPath = Server.MapPath("~/Reports/LAB_INVOICE_GEN_NEW.rpt");
        //        reportPath = Path.Combine(_env.WebRootPath, "Reports", "LAB_INVOICE_GEN_NEW.rpt");

        //    }
        //    else if ((Convert.ToInt32(dtreg.INVOICE_DT) >= 202207) && (dtreg.Resign == "N"))
        //    {
        //        // reportPath = Server.MapPath("~/Reports/LAB_INVOICE_GEN_HR.rpt");
        //        reportPath = Path.Combine(_env.WebRootPath, "Reports", "LAB_INVOICE_GEN_HR.rpt");
        //    }
        //    else
        //    {
        //        //reportPath = Server.MapPath("~/Reports/LAB_INVOICE_GEN.rpt");
        //        reportPath = Path.Combine(_env.WebRootPath, "Reports", "LAB_INVOICE_GEN.rpt");
        //    }
        //    rd.Load(reportPath);

        //    // Replace with your data retrieval logic
        //    DataSet dsCustom = LabInvoiceDownloadRepository.Getdata(CaseNo, RegNo, InvoiceNo, TranNo);
        //    rd.SetDataSource(dsCustom);

        //    Stream stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
        //    stream.Seek(0, SeekOrigin.Begin);

        //    return File(stream, "application/pdf", "CustomerList.pdf");
        //}

        [HttpPost]
        //[ValidateDNTCaptcha(ErrorMessage = "Invalid security code.", CaptchaGeneratorLanguage = Language.English, CaptchaGeneratorDisplayMode = DisplayMode.ShowDigits)]
        public ActionResult Login(LoginModel loginModel)
        {
            UserSessionModel userMaster = userRepository.LoginByUserPass(loginModel);
            if (userMaster != null)
            {
                //// temporary Commited - for local
                //if (userMaster.MOBILE != null && userMaster.MOBILE != "")
                if (1==1)
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
                    string EncryptUserName = Common.EncryptQueryString(loginModel.UserName);
                    return RedirectToAction("OTPVerification", "Home", new { UserName = EncryptUserName });
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

        public ActionResult OTPVerification(string UserName)
        {
            string DecryptUserName = Common.DecryptQueryString(UserName);
            LoginModel loginModel1 =new LoginModel();
            loginModel1.DecryptUserName = DecryptUserName;
            return View(loginModel1);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OTPProceed(LoginModel loginModel)
        {
            loginModel.UserName = loginModel.DecryptUserName;
            if (userRepository.VerifyOTP(loginModel))
            {
                UserSessionModel userMaster = userRepository.FindByLoginDetail(loginModel);
                if (userMaster != null)
                {
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
                    };
                    var userIdentity = new ClaimsIdentity(userClaims, "User Identity");
                    var userPrincipal = new ClaimsPrincipal(new[] { userIdentity });
                    HttpContext.SignInAsync(userPrincipal);

                    SessionHelper.MenuModelDTO = userRepository.GenerateMenuListByRoleId(userMaster.RoleId);
                    return Json(new { status = true, responseText = "" });
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

        public IActionResult RegenerateOTP(string UserName)
        {
            LoginModel loginModel = new LoginModel();
            loginModel.UserName = UserName;
            UserSessionModel userMaster = userRepository.LoginByUserName(loginModel);
            loginModel.MOBILE =userMaster.MOBILE;

            string sender = "RITES/QA";
            Random random = new Random();
            string otp = Convert.ToString(random.Next(1000, 9999));
            string message = otp + " is the One Time Password for verification of your login with RITES LTD- QA Division. Valid for 10 minutes. Please do not share with anyone." + "-" + sender;
            //// temporary Commited - for local 
            //string responce = Common.SendOTP(loginModel.MOBILE, message);
            //loginModel.OTP = otp;
            loginModel.OTP = "123";
            userRepository.SaveOTPDetails(loginModel);
            string EncryptUserName = Common.EncryptQueryString(loginModel.UserName);
            return RedirectToAction("OTPVerification","Home", new { UserName = EncryptUserName });
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
                    body = body.Replace("{{USERNAME}}", userMaster.UserName).Replace("{{RESETPASSURL}}", Configuration.GetSection("BaseURL").Value + Url.Action("ResetPassword", "Home", new { id = Common.EncryptQueryString(Convert.ToString(userMaster.UserId)) }));
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
            UserModel user = userRepository.FindByID(id);
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

                UserModel user = userRepository.FindByID(Convert.ToString(resetPassword.UserId));
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
                string SaltedToken = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz012345678912";
                string strOldPassword = formCollection["OldPassword"];
                string strNewPassword = formCollection["NewPassword"];

                user.Password = strNewPassword;

                UserModel userMaster = userRepository.FindByID(user.UserId);

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

        [HttpGet]
        public ActionResult UserAccessDenied()
        {
            return View();
        }
    }
}
