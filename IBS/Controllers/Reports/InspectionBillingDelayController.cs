using IBS.Filters;
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
            if (Request.Query["Action"] != "")
            {

            }
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
