using IBSAPI.Interfaces;
using IBSAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IBSAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]    
    public class InspectionController : ControllerBase
    {
        #region Varible
        private readonly IInspectionRepository inspectionRepository;
        #endregion
        public InspectionController(IInspectionRepository _inspectionRepository)
        {
            inspectionRepository = _inspectionRepository;
        }

        [HttpGet("GetTodayInspection", Name = "GetTodayInspection")]
        public IActionResult GetTodayInspection(int IeCd)
        {
            try
            {
                var detail = inspectionRepository.GetToDayInspection(IeCd);
                if (detail.Count() > 0)
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                        message = "Data get successfully",
                        data = detail
                    };
                    return Ok(response);
                }
                else
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                        message = "No Data Found"
                    };
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "GetTodayInspection", "Inspection", 1, string.Empty);
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                    message = ex.Message.ToString(),
                };
                return Ok(response);
            }            
        }

        [HttpGet("GetTomorrowInspection", Name = "GetTomorrowInspection")]
        public IActionResult GetTomorrowInspection(int IeCd)
        {
            try
            {
                var detail = inspectionRepository.GetTomorrowInspection(IeCd);
                if (detail.Count() > 0)
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                        message = "Data get successfully",
                        data = detail
                    };
                    return Ok(response);
                }
                else
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                        message = "No Data Found"
                    };
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "GetTomorrowInspection", "Inspection", 1, string.Empty);
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
