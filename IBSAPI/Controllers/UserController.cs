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
            //string EncryptPassword = Common.EncryptQueryString(loginModel.Password.ToString());
            string DecryptPassword = Common.DecryptQueryString(loginModel.Password);
            loginModel.Password= DecryptPassword;
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

        [HttpPost("ForgotPassword", Name = "ForgotPassword")]
        public IActionResult ForgotPassword([FromBody] ForgotPasswordModel forgotPasswordModel)
        {
            UserModel userMaster = userRepository.FindByUsernameOrEmail(forgotPasswordModel.UserName);
            if (userMaster != null)
            {
                if (userMaster.Email != null && userMaster.Email != "")
                {
                    string rootPath = Configuration["MyAppSettings:ResetPasswordPath"] + Common.EncryptQueryString(Convert.ToString(userMaster.userId));
                    string body = System.IO.File.ReadAllText(System.IO.Path.Combine(_env.WebRootPath, "EmailTemplates", "ForgotPassword.html"), Encoding.UTF8);
                    body = body.Replace("{{USERNAME}}", userMaster.userName).Replace("{{RESETPASSURL}}", rootPath );

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