using IBS.Filters;
using IBS.Interfaces;
using IBS.Interfaces.Inspection_Billing;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers.InspectionBilling
{
    [Authorization]
    public class AdministratorPurchaseOrderController : BaseController
    {
        #region Variables
        private readonly IAdministratorPurchaseOrderRepository pIAdministratorPurchaseOrderRepository;
        #endregion
        public AdministratorPurchaseOrderController(IAdministratorPurchaseOrderRepository _pIAdministratorPurchaseOrderRepository)
        {
            pIAdministratorPurchaseOrderRepository = _pIAdministratorPurchaseOrderRepository;
        }

        [Authorization("AdministratorPurchaseOrder", "Index", "view")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            string region_code = Convert.ToString(IBS.Helper.SessionHelper.UserModelDTO.Region);
            DTResult<AdministratorPurchaseOrderModel> dTResult = pIAdministratorPurchaseOrderRepository.GetPOMasterList(dtParameters, region_code);
            return Json(dTResult);
        }
    }
}
