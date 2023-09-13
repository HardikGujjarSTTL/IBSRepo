using IBS.Filters;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Interfaces.Reports;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers.Reports
{
    [Authorization]
    public class InspectionBillingDelayController : BaseController
    {
        #region Variables
        private readonly IInspectionBillingDelayRepository inspectionBillingDelayRepository;
        #endregion
        public InspectionBillingDelayController(IInspectionBillingDelayRepository _inspectionBillingDelayRepository)
        {
            inspectionBillingDelayRepository = _inspectionBillingDelayRepository;
        }
        [Authorization("InspectionBillingDelay", "Index", "view")]
        public IActionResult Index()
        {
            string Region = SessionHelper.UserModelDTO.Region;
            var Action = Convert.ToString(Request.Query["Action"].FirstOrDefault());
            string wRegion = "";
            if (Region == "N") { wRegion = "RITES LTD. (NORTHERN REGION)"; }
            else if (Region == "S") { wRegion = "RITES LTD. (SOUTHERN REGION)"; }
            else if (Region == "E") { wRegion = "RITES LTD. (EASTERN REGION)"; }
            else if (Region == "W") { wRegion = "RITES LTD. (WESTERN REGION)"; }
            else if (Region == "C") { wRegion = "RITES LTD. (CENTRAL REGION)"; }
            ViewBag.RegionCode = GetUserInfo.Region;
            ViewBag.Region = wRegion;
            ViewBag.Action = Action;
            return View();
        }


        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)//InspectionBillingDelayFilterModel model)
        {
            var data = inspectionBillingDelayRepository.Get_Inspection_Billing_Delay(dtParameters, GetUserInfo);
            return Json(data);
        }
    }
}
