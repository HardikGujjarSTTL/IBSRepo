using IBSAPI.Interfaces;
using IBSAPI.Models;
using IBSAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IBSAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VendorController : ControllerBase
    {
        #region Variable
        private readonly IVendorRepository vendorRepository;
        #endregion

        public VendorController(IVendorRepository _vendorRepository)
        {
            vendorRepository = _vendorRepository;
        }

        [HttpGet("GetCaseDetailsforvendor", Name = "GetCaseDetailsforvendor")]
        public IActionResult GetCaseDetailsforvendor(int UserID)
        {
            try
            {
                List<CallRegiModel> callRegiModels = vendorRepository.GetCaseDetailsforvendor(UserID);
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
                Common.AddException(ex.ToString(), ex.Message.ToString(), "CallList", "GetCaseDetailsforvendor", 1, string.Empty);
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                    message = ex.Message.ToString(),
                };
                return Ok(response);
            }
        }


        [HttpGet("GetManufacturerList", Name = "GetManufacturerList")]
        public IActionResult GetManufacturerList(string searchValues)
        {
            try
            {
                bool IsDigit = false;
                if (searchValues != null && searchValues != "0")
                {
                    char characterToCheck = searchValues[3];
                    IsDigit = Char.IsDigit(characterToCheck);
                }

                List<ManufacturerModel> agencyClient = new List<ManufacturerModel>();
                if (searchValues != null && searchValues != "0")
                {
                    if (IsDigit)
                    {
                        agencyClient = Common.GetVendorDigit(Convert.ToInt32(searchValues));
                    }
                    else
                    {
                        agencyClient = Common.GetVendorUsingTextAndValues(searchValues);
                    }
                }
                if (agencyClient.Count() > 0)
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                        message = "Successfully",
                        data = agencyClient
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
                Common.AddException(ex.ToString(), ex.Message.ToString(), "CallList", "GetCaseDetailsforvendor", 1, string.Empty);
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
