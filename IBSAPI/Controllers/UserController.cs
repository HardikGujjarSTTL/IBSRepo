﻿using IBSAPI.Helper;
using IBSAPI.Interfaces;
using IBSAPI.Models;
using IBSAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.VisualBasic;
using System.Collections;
using System.Configuration;
using System.Net;
using System.Text;
using static IBSAPI.Helper.Enums;

namespace IBSAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        #region Variables
        private readonly IUserRepository userRepository;
        private readonly IWebHostEnvironment _env;
        private readonly ITokenServices tokenServices;
        private readonly ISendMailRepository pSendMailRepository;
        public IConfiguration Configuration { get; }
        #endregion
        public UserController(IUserRepository _userRepository, IWebHostEnvironment env, ITokenServices _tokenServices, IConfiguration configuration, ISendMailRepository _pSendMailRepository)
        {
            userRepository = _userRepository;
            _env = env;
            tokenServices = _tokenServices;
            Configuration = configuration;
            pSendMailRepository = _pSendMailRepository;
        }

        [HttpPost("SignIn", Name = "SignIn")]
        public IActionResult SignIn([FromBody] LoginModel loginModel)
        {
            try
            {
                //string encryptedUserName = Common.getEncryptedText("adminnr", "301ae92bb2bc7599");
                //string encryptedPassword = Common.getEncryptedText("Rites123", "301ae92bb2bc7599");

                //string DecryptUserName = Common.getDecryptedText(loginModel.UserName, loginModel.UniqueId);
                //string DecryptPassword = Common.getDecryptedText(loginModel.Password, loginModel.UniqueId);
                //loginModel.UserName = DecryptUserName;
                //loginModel.Password = DecryptPassword;
                UserModel userModel = userRepository.FindByLoginDetail(loginModel);
                if (userModel != null)
                {
                    var token = tokenServices.GenerateToken(userModel.userId);
                    tokenServices.InActiveOldActiveTokens(userModel.userId, token.AuthToken);
                    userModel.token = token.AuthToken;
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                        message = "Data get successfully",
                        data = userModel
                    };
                    return Ok(response);
                }
                else
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                        message = "Invalid Username or Password."
                    };
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "User_API", "SignIn", 1, string.Empty);
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                    message = ex.Message.ToString(),
                };
                return Ok(response);
            }
        }

        [HttpPost("SignInNew", Name = "SignInNew")]
        public IActionResult SignInNew([FromBody] LoginModel loginModel)
        {
            try
            {
                //string encryptedUserName = Common.getEncryptedText("adminnr", "301ae92bb2bc7599");
                //string encryptedPassword = Common.getEncryptedText("Rites123", "301ae92bb2bc7599");

                string DecryptUserName = Common.getDecryptedText(loginModel.UserName, loginModel.UniqueId);
                string DecryptPassword = Common.getDecryptedText(loginModel.Password, loginModel.UniqueId);
                loginModel.UserName = DecryptUserName;
                loginModel.Password = DecryptPassword;
                UserModel userModel = userRepository.FindByLoginDetail(loginModel);
                if (userModel != null)
                {
                    var token = tokenServices.GenerateToken(userModel.userId);
                    tokenServices.InActiveOldActiveTokens(userModel.userId, token.AuthToken);
                    userModel.token = token.AuthToken;
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                        message = "Data get successfully",
                        data = userModel
                    };
                    return Ok(response);
                }
                else
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                        message = "Invalid Username or Password."
                    };
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "User_API", "SignIn", 1, string.Empty);
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                    message = ex.Message.ToString(),
                };
                return Ok(response);
            }
        }

        [HttpPost("ForgotPassword", Name = "ForgotPassword")]
        public IActionResult ForgotPassword([FromBody] ForgotPasswordModel forgotPasswordModel)
        {
            try
            {
                UserSessionModel userMaster = userRepository.FindByUsernameOrEmail(forgotPasswordModel.UserName, forgotPasswordModel.UserType);
                if (userMaster != null)
                {
                    if (userMaster.FPUserID.Trim() == "84997")
                    {
                        userMaster.Email = "urvesh.modi@silvertouch.com";
                    }
                    if (userMaster.Email != null && userMaster.Email != "")
                    {
                        string RootHostName = HttpContext.Request.Host.Value;
                        string WebRootPath = "https://"+ RootHostName + "/IBS2/Home/ResetPassword?id=";
                        string rootPath = /*Configuration["MyAppSettings:ResetPasswordPath"]*/ WebRootPath + Common.EncryptQueryString(Convert.ToString(userMaster.FPUserID)) + "&UserType=" + Convert.ToString(forgotPasswordModel.UserType);
                        string body = System.IO.File.ReadAllText(System.IO.Path.Combine(_env.WebRootPath, "EmailTemplates", "ForgotPassword.html"), Encoding.UTF8);
                        body = body.Replace("{{USERNAME}}", userMaster.UserName).Replace("{{RESETPASSURL}}", rootPath);

                        SendMailModel sendMailModel = new SendMailModel();
                        sendMailModel.To = userMaster.Email;
                        sendMailModel.Subject = "Reset Password on IBS";
                        sendMailModel.Message = body;
                        bool isSend = pSendMailRepository.SendMail(sendMailModel, null);

                        if (isSend)
                        {
                            var response = new
                            {
                                resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                                message = "Reset Password link has been sent to registered email id"
                            };
                            return Ok(response);

                        }
                        else
                        {
                            var response = new
                            {
                                resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                                message = "Email not send"
                            };
                            return Ok(response);
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                            message = "Email ID Not Found"
                        };
                        return Ok(response);
                    }
                }
                else
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                        message = "Invalid Username"
                    };
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "User_API", "ForgotPassword", 1, string.Empty);
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                    message = ex.Message.ToString(),
                };
                return Ok(response);
            }
        }

        [HttpGet("GetUserType", Name = "GetUserType")]
        public IActionResult GetUserType()
        {
            try
            {
                List<TextValueDropDownDTO> model = Common.GetUserTypeLogin();
                var response1 = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                    data = model
                };
                return Ok(response1);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "User_API", "GetUserType", 1, string.Empty);
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                    message = ex.Message.ToString(),
                };
                return Ok(response);
            }
        }

        private string GetErrorList(ModelStateDictionary modelState)
        {
            var errors = modelState.Values
                .SelectMany(x => x.Errors)
                .Select(x => x.ErrorMessage)
                .ToList();

            return string.Join(", ", errors);
        }
    }
}
