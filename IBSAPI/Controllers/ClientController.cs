using IBSAPI.Interfaces;
using IBSAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBSAPI.Controllers
{
    public class ClientController : Controller
    {
        #region Variable
        private readonly IVendorRepository vendorRepository;
        #endregion

        public ClientController(IVendorRepository _vendorRepository)
        {
            vendorRepository = _vendorRepository;
        }

        [HttpGet("GetCaseDetailsforClient", Name = "GetCaseDetailsforClient")]
        public IActionResult GetCaseDetailsforClient(string UserID, string Organisation, string OrgnType)
        {
            try
            {
                List<CallRegiModel> callRegiModels = vendorRepository.GetCaseDetailsforClient(UserID, Organisation, OrgnType);
                if (callRegiModels.Count() > 0)
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                        message = "Successfully",
                        data = callRegiModels
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
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Vendor_API", "GetCaseDetailsforvendor", 1, string.Empty);
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
