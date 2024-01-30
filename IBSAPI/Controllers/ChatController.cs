using IBSAPI.Interfaces;
using IBSAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IBSAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatRepository _chatRepository;
        private readonly IWebHostEnvironment _env;

        public ChatController(IChatRepository chatRepository, IWebHostEnvironment env)
        {
            _chatRepository = chatRepository;
            _env = env;
        }

        [HttpGet("Get_User_Type", Name = "Get_User_Type")]
        public IActionResult Get_User_Type()
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "DashBoard_API", "Get_IE", 1, string.Empty);
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                    message = ex.Message.ToString(),
                };
                return Ok(response);
            }
        }
    }
}
