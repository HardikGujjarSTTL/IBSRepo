using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class Consignee_PMFormController : BaseController
    {
        #region Variables
        private readonly IConsignee_PMForm consignee_PMForm;
        #endregion
        public Consignee_PMFormController(IConsignee_PMForm _consignee_PMForm)
        {
            consignee_PMForm = _consignee_PMForm;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Manage(int id)
        {
            ConsigneePurchaseModel model = new();
            if (id > 0)
            {
                model = consignee_PMForm.FindByID(id);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<ConsigneePurchaseModel> dTResult = consignee_PMForm.GetConsignee_PMFormList(dtParameters);
            return Json(dTResult);
        }
        public IActionResult Delete(int id)
        {
            try
            {
                if (consignee_PMForm.Remove(id, UserId))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Consignee_PMForm", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Consignee_PMFormDetailsSave(ConsigneePurchaseModel model)
        {
            try
            {
                string msg = "Consignee/Purchaser Master Inserted Successfully.";

                if (model.ConsigneeCd > 0)
                {
                    msg = "Consignee/Purchaser Master Updated Successfully.";
                    model.Updatedby = UserId;
                }
                model.Createdby = UserId;
                int i = consignee_PMForm.Consignee_PMFormDetailsInsertUpdate(model);
                if (i > 0)
                {
                    return Json(new { status = true, responseText = msg });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Consignee_PMForm", "Consignee_PMFormDetailsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

    }
}