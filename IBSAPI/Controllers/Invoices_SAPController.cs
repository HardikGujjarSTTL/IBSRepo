using Microsoft.AspNetCore.Mvc;
using IBSAPI.Interfaces;
using IBSAPI.Models;
using IBSAPI.Repositories;

namespace IBSAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class Invoices_SAPController : ControllerBase
    {

        #region Variable
        private readonly IInvoices_SAPRepository saprepository;
        #endregion

        public Invoices_SAPController(IInvoices_SAPRepository _saprepository)
        {
            saprepository = _saprepository;
        }

        #region
        [HttpGet("getinvoicelist", Name = "getinvoicelist")]
        public IActionResult getinvoicelist(DateTime from, DateTime to)
        {
            try
            {
                var statusList = saprepository.FindInvoiceList(from, to);
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
                        message = "Invoice Not Generated",
                        data = statusList
                    };
                    return Ok(response);
                }

            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Invoices_SAP", "saprepository", 1, string.Empty);
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
