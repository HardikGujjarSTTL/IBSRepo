using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IBS.Controllers
{
    [Authorization]
    public class ConsigneePurchaseController : BaseController
    {
        #region Variables
        //private readonly IConsignee_PMForm consignee_PMForm;
        private readonly IConsigneePurchaseRepository consigneePurchaseRepository;
        #endregion
        public ConsigneePurchaseController(IConsigneePurchaseRepository _consigneePurchaseRepository)
        {
            consigneePurchaseRepository = _consigneePurchaseRepository;
        }

        [Authorization("ConsigneePurchase", "Index", "view")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            var dTResult = new DTResult<ConsigneePurchaseMasterSearchModel>();
            try
            {
                dTResult = consigneePurchaseRepository.Get_Consignee_Purchase(dtParameters);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ConsigneePurchase", "LoadTable", 1, GetIPAddress());
            }
            return Json(dTResult);
        }
        [Authorization("ConsigneePurchase", "Index", "view")]
        public IActionResult Manage()
        {
            ConsigneePurchaseModel model = new();
            if (Convert.ToString(Request.Query["CONSIGNEE_CD"]) != null || Convert.ToString(Request.Query["CONSIGNEE_CD"]) != "")
            {
                var consignee_cd = Convert.ToInt32(Request.Query["CONSIGNEE_CD"]);
                model = consigneePurchaseRepository.Get_Consignee_Purchase_Detail(consignee_cd);
                if (model != null)
                {
                    var state = consigneePurchaseRepository.Get_State(Convert.ToString(model.ConsigneeCity));
                    model.ConsigneeState = state;
                }
            }
            ViewBag.ConsigneeType = new List<SelectListItem>
            {
                new SelectListItem { Text = "Railway", Value = "R" },
                new SelectListItem { Text = "Private", Value = "P" },
                new SelectListItem { Text = "Foreign Railway", Value = "F" },
                new SelectListItem { Text = "PSU", Value = "U" },
                new SelectListItem { Text = "State Government", Value = "S" },
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorization("ConsigneePurchase", "Index", "edit")]
        public IActionResult ConsigneePurchaseDetailsSave(ConsigneePurchaseModel model)
        {
            try
            {
                string msg = "Consignee Purchase Master Inserted Successfully.";
                if (model.ConsigneeCd > 0)
                {
                    msg = "Consignee Purchase Master Updated Successfully.";
                    model.Updatedby = UserId;
                }
                int id = consigneePurchaseRepository.CongsigneePurchaseInsertUpdate(model, GetUserInfo);
                if (id > 0)
                {
                    return Json(new { status = true, responseText = msg });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ConsigneePurchase", "ConsigneePurchaseDetailsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpPost]
        public IActionResult GetState(string City_CD)
        {
            var state = "";
            try
            {
                state = consigneePurchaseRepository.Get_State(City_CD);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ConsigneePurchase", "GetState", 1, GetIPAddress());
            }
            return Json(state);
        }

        [HttpPost]
        [Authorization("ConsigneePurchase", "Index", "delete")]
        public IActionResult ConsigneePurchaseDelete(int CONSIGNEE_CD)
        {
            var flag = 0;
            try
            {
                flag = consigneePurchaseRepository.ConsigneePurchaseDelete(CONSIGNEE_CD);
            }
            catch (Exception ex)
            {
                flag = 500;
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ConsigneePurchase", "GetState", 1, GetIPAddress());
            }
            return Json(flag);
        }
    }
}
