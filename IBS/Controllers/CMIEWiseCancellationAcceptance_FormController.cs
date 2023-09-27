using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IBS.Controllers
{
    public class CMIEWiseCancellationAcceptance_FormController : BaseController
    {

        private readonly ICMIEWiseCancellationAcceptance_FormRepository cmiewisecancellationacceptanceformrepository;
        public CMIEWiseCancellationAcceptance_FormController(ICMIEWiseCancellationAcceptance_FormRepository _cmiewisecancellationacceptanceformrepository)
        {
            cmiewisecancellationacceptanceformrepository = _cmiewisecancellationacceptanceformrepository;
        }

        public IActionResult Ie_dropdown(string selectedValue)
        {
            string Region = GetRegionCode;
            CMIEWiseCancellationAcceptance_FormModel dTResult = cmiewisecancellationacceptanceformrepository.GetIEsByRegionAndCO(selectedValue, Region);

            

            return Json(dTResult);
        }

        public IActionResult CMIEWTable([FromBody] DTParameters dtParameters)
        {
            string Region = GetRegionCode;
            int selectedOption = Convert.ToInt32(dtParameters.AdditionalValues?.GetValueOrDefault("selectedOption"));
          if(selectedOption == 1)
          {
                DTResult<CMIEWiseCancellationAcceptance_FormModel> dTResult = cmiewisecancellationacceptanceformrepository.CMIEWTable(dtParameters, Region);
                return Json(dTResult);
          }
          else
          {
                DTResult<CMIEWiseCancellationAcceptance_FormModel> dTResult = cmiewisecancellationacceptanceformrepository.CMIEWTable1(dtParameters, Region);
                return Json(dTResult);
          }


            
        }

        [HttpPost]
        public IActionResult update(string selectedCASENO, string selectedCALL_RECV_DATE, string selectedCALL_SNO)
        {
            
            CMIEWiseCancellationAcceptance_FormModel model = new CMIEWiseCancellationAcceptance_FormModel();
            model.CASE_NO = Request.Form["CASE_NO"];
            model.CALL_RECV_DATE = Request.Form["CALL_RECV_DATE"];
            model.CALL_SNO = Convert.ToInt32(Request.Form["CALL_SNO"]);
            string Uname = Convert.ToString(UserId);
            string i = cmiewisecancellationacceptanceformrepository.update(model, Uname);
            if (i == "UPDATED")
            {
                return Json(i);
            }
            else
            {

                return null;

            }
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
