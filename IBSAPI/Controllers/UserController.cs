using IBSAPI.Interfaces;
using IBSAPI.Models;
using IBSAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.VisualBasic;
using System.Collections;
using System.Configuration;
using System.Net;

namespace IBSAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        #region Variables
        private readonly IUserRepository userRepository;
        private readonly ITokenServices tokenServices;
        #endregion
        public UserController(IUserRepository _userRepository, ITokenServices _tokenServices)
        {
            userRepository = _userRepository;
            tokenServices = _tokenServices;
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
