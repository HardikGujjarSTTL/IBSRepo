using IBS.Filters;
using IBS.Interfaces;
using IBS.Interfaces.Inspection_Billing;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            DTResult<AdministratorPurchaseOrderListModel> dTResult = pIAdministratorPurchaseOrderRepository.GetPOMasterList(dtParameters, region_code);
            return Json(dTResult);
        }

        [Authorization("AdministratorPurchaseOrder", "Index", "view")]
        public IActionResult Manage(string PO_TYPE,string RLY_CD,string CaseNo)
        {
            AdministratorPurchaseOrderModel model = new();
            if (CaseNo != null)
            {
                model = pIAdministratorPurchaseOrderRepository.FindByID(CaseNo);
            }
            ViewBag.PO_TYPE = PO_TYPE;
            ViewBag.RLY_CD = RLY_CD;
            return View(model);
        }

        [HttpGet]
        public IActionResult GetRailwayCode(string type)
        {
            try
            {
                List<SelectListItem> objList = Common.GetRailwayCode(type);
                return Json(new { status = true, list = objList });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "AdministratorPurchaseOrder", "GetRailwayCode", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
    }
}
