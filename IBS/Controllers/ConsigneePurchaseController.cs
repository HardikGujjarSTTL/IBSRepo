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
        private readonly IConsigneePurchaseRepository consigneePurchaseRepository;

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
            DTResult<ConsigneePurchaseMasterSearchModel> dTResult = consigneePurchaseRepository.GetConsigneePurchaseList(dtParameters);
            return Json(dTResult);
        }

        [Authorization("ConsigneePurchase", "Index", "view")]
        public IActionResult Manage(int id)
        {
            //ConsigneePurchaseModel model = new();
            //if (Convert.ToString(Request.Query["CONSIGNEE_CD"]) != null || Convert.ToString(Request.Query["CONSIGNEE_CD"]) != "")
            //{
            //    var consignee_cd = Convert.ToInt32(Request.Query["CONSIGNEE_CD"]);
            //    model = consigneePurchaseRepository.Get_Consignee_Purchase_Detail(consignee_cd);
            //    if (model != null)
            //    {
            //        var state = consigneePurchaseRepository.Get_State(Convert.ToString(model.ConsigneeCity));
            //        model.ConsigneeState = state;
            //    }
            //}
            //ViewBag.ConsigneeType = new List<SelectListItem>
            //{
            //    new SelectListItem { Text = "Railway", Value = "R" },
            //    new SelectListItem { Text = "Private", Value = "P" },
            //    new SelectListItem { Text = "Foreign Railway", Value = "F" },
            //    new SelectListItem { Text = "PSU", Value = "U" },
            //    new SelectListItem { Text = "State Government", Value = "S" },
            //};
            //return View(model);

            ConsigneePurchaseModel model = new();
            if (id > 0)
            {
                model = consigneePurchaseRepository.FindByID(id);
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorization("ConsigneePurchase", "Index", "edit")]
        public IActionResult Manage(ConsigneePurchaseModel model)
        {
            try
            {
                if (model.ConsigneeCd == 0)
                {
                    model.Createdby = UserId;
                    model.UserId = USER_ID.Substring(0, 8);
                    consigneePurchaseRepository.SaveDetails(model);
                    AlertAddSuccess("Record Added Successfully.");
                }
                else
                {
                    model.Updatedby = UserId;
                    model.UserId = USER_ID.Substring(0, 8);
                    consigneePurchaseRepository.SaveDetails(model);
                    AlertAddSuccess("Record Updated Successfully.");
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ConsigneePurchase", "Manage", 1, GetIPAddress());
            }
            return View(model);
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
        public IActionResult Delete(int id)
        {
            try
            {
                if (consigneePurchaseRepository.Remove(id, UserId))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ConsigneePurchase", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }
    }
}
