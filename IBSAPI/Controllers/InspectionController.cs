using IBSAPI.Interfaces;
using IBSAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;

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

        [HttpGet("Get_IE_TodayInspection", Name = "GetTodayInspection")]
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
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Inspection_API", "GetTodayInspection", 1, string.Empty);
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                    message = ex.Message.ToString(),
                };
                return Ok(response);
            }
        }

        [HttpGet("Get_IE_TomorrowInspection", Name = "GetTomorrowInspection")]
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
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Inspection_API", "GetTomorrowInspection", 1, string.Empty);
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                    message = ex.Message.ToString(),
                };
                return Ok(response);
            }
        }

        [HttpGet("Get_Case_Detail_For_IE", Name = "GetCaseDetailForIE")]
        public IActionResult GetCaseDetailForIE(string Case_No, DateTime CallRecvDt, int CallSNo, int IeCd)
        {
            try
            {
                var result = inspectionRepository.GetCaseDetailForIE(Case_No, CallRecvDt, CallSNo, IeCd);
                if(result != null)
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                        message = "Data get successfully",
                        data = result,
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
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Inspection_API", "GetCaseDetailForIE", 1, string.Empty);
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                    message = ex.Message.ToString(),
                };
                return Ok(response);
            }
        }

        [HttpGet("Get_IE_PendingInspection", Name = "GetPendingInspection")]
        public IActionResult GetPendingInspection(int IeCd, string Region, string Date)
        {
            try
            {
                var result = inspectionRepository.GetPendingInspection(IeCd, Region, Date);
                if (result != null)
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                        message = "Data get successfully",
                        data = result,
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
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Inspection_API", "GetPendingInspection", 1, string.Empty);
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                    message = ex.Message.ToString(),
                };
                return Ok(response);
            }
        }

        [HttpGet("Get_Date_Wise_Recent_Inspection", Name = "GetDateWiseRecentInspection")]
        public IActionResult GetDateWiseRecentInspection(int IeCd, string FromDate, string ToDate)
        {
            try
            {
                var result = inspectionRepository.GetDateWiseRecentInspection(IeCd, FromDate, ToDate);
                if (result != null)
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                        message = "Data get successfully",
                        data = result,
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
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Inspection_API", "GetDateWiseRecentInspection", 1, string.Empty);
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                    message = ex.Message.ToString(),
                };
                return Ok(response);
            }
        }

        [HttpGet("Get_IE_CompleteInspection", Name = "GetCompleteInspection")]
        public IActionResult GetCompleteInspection(int IeCd)
        {
            try
            {
                var result = inspectionRepository.GetCompleteInspection(IeCd);
                if (result != null)
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                        message = "Data get successfully",
                        data = result,
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
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Inspection_API", "GetCompleteInspection", 1, string.Empty);
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
