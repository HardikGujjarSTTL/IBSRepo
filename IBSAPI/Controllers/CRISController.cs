using Microsoft.AspNetCore.Mvc;
using IBSAPI.Interfaces;
using IBSAPI.Models;
using IBSAPI.Repositories;

namespace IBSAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CRISController : ControllerBase
    {
        #region Variable
        private readonly ICRISRepository crisrepository;
        #endregion

        public CRISController(ICRISRepository _crisrepository)
        {
            crisrepository = _crisrepository;
        }

        #region Bill Details
        [HttpGet("getbilldetails", Name = "getbilldetails")]
        public IActionResult getbilldetails(string billno)
        {
            try
            {
                CRISModel model = crisrepository.FindBillDetails(billno);
                if (model != null)
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                        message = "Successfully",
                        data = model
                    };
                    return Ok(response);
                }
                else
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                        message = "Bill Not Generated"
                    };
                    return Ok(response);
                }

            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "CRIS", "GetBillDetails", 1, string.Empty);
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                    message = ex.Message.ToString(),
                };
                return Ok(response);
            }
        }
        #endregion

        #region
        [HttpGet("getlistbillno", Name = "getlistbillno")]
        public IActionResult getlistbillno(DateTime from, DateTime to)
        {
            try
            {
                var statusList = crisrepository.FindListBillNo(from, to);
                if (statusList.Count() > 0)
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                        message = "Successfully",
                        data = statusList
                    };
                    return Ok(response);
                }
                else
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                        message = "Bill Not Generated",
                        data = statusList
                    };
                    return Ok(response);
                }

            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "CRIS", "getlistbillno", 1, string.Empty);
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                    message = ex.Message.ToString(),
                };
                return Ok(response);
            }
        }
        #endregion

        #region
        [HttpGet("getbill", Name = "getbill")]
        public IActionResult getbill(DateTime from, DateTime to, string? billno)
        {
            try
            {
                var statusList = crisrepository.Findgetbill(from, to, billno);
                if (statusList.Count() > 0)
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                        message = "Successfully",
                        data = statusList
                    };
                    return Ok(response);
                }
                else
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                        message = "Bill Not Generated",
                        data = statusList
                    };
                    return Ok(response);
                }

            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "CRIS", "getbill", 1, string.Empty);
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                    message = ex.Message.ToString(),
                };
                return Ok(response);
            }
        }
        #endregion

    }
}
