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
        [HttpGet("GetBillDetails", Name = "GetBillDetails")]
        public IActionResult GetBillDetails(string BillNo)
        {
            try
            {
                CRISModel model = crisrepository.FindBillDetails(BillNo);
                if(model != null)
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

    }
}
