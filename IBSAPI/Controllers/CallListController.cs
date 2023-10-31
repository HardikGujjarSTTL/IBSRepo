using IBSAPI.Interfaces;
using IBSAPI.Models;
using IBSAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Configuration;

namespace IBSAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CallListController : ControllerBase
    {
        #region Variable
        private readonly ICallListRepository callListRepository;
        #endregion

        public CallListController(ICallListRepository _callListRepository)
        {
            callListRepository = _callListRepository;
        }

        [HttpGet("GetCallList", Name = "GetCallList")]
        public IActionResult GetCallList()
        {
            List<CallListModel> callList = callListRepository.GetCallList();
            if (callList.Count() > 0)
            {
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                    message = "Data get successfully",
                    data = callList
                };
                return Ok(response);
            }
            else
            {
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                    message = "No Data Found",
                };
                return Ok(response);
            }
        }
        
    }
}
