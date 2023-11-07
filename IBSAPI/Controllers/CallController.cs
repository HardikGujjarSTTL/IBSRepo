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
    public class CallController : ControllerBase
    {
        #region Variable
        private readonly ICallRepository callRepository;
        #endregion

        public CallController(ICallRepository _callRepository)
        {
            callRepository = _callRepository;
        }

        [HttpGet("GetCallList", Name = "GetCallList")]
        public IActionResult GetCallList()
        {
            try
            {
                List<CallListModel> callList = callRepository.GetCallList();
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
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Call_API", "GetCallList", 1, string.Empty);
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                    message = ex.Message.ToString(),
                };
                return Ok(response);
            }
        }

        [HttpPost("SheduleInspection", Name = "SheduleInspection")]
        public IActionResult SheduleInspection([FromBody] SheduleInspectionRequestModel sheduleInspectionRequestModel)
        {
            try
            {
                int id = callRepository.SheduleInspection(sheduleInspectionRequestModel);
                if (id > 0)
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                        message = "Successfully"
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
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Call_API", "SheduleInspection", 1, string.Empty);
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                    message = ex.Message.ToString(),
                };
                return Ok(response);
            }
        }
        [HttpGet("Get_Call_Status_List", Name = "Get_Call_Status_List")]
        public IActionResult Get_Call_Status_List()
        {
            try
            {
                var statusList = callRepository.Get_Call_Status_List();
                if (statusList.Count() > 0)
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                        message = "Data get successfully",
                        data = statusList
                    };
                    return Ok(response);
                }
                else
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                        message = "No Data Found",
                        data = statusList
                    };
                    return Ok(response);

                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Call_API", "Get_Call_Status_List", 1, string.Empty);
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
