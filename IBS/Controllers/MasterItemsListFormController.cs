using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class MasterItemsListFormController : BaseController
    {
        #region Variables
        private readonly IMasterItemsListForm masterItemsListForm;
        #endregion
        public MasterItemsListFormController(IMasterItemsListForm _masterItemsListForm)
        {
            masterItemsListForm = _masterItemsListForm;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Manage(string id)
        {
            MasterItemsListFormModel model = new();
            if (id != null)
            {
                model = masterItemsListForm.FindByID(id);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<MasterItemsListFormModel> dTResult = masterItemsListForm.GetMasterItemsListFormList(dtParameters);
            return Json(dTResult);
        }
        public IActionResult Delete(String id)
        {
            try
            {
                if (masterItemsListForm.Remove(id, UserId))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "MasterItemsListForm", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MasterItemsListFormDetailsSave(MasterItemsListFormModel model)
        {
            try
            {
                string msg = "Master Items List Form Inserted Successfully.";

                // if (model.ItemCd > 0)
                {
                    msg = "Master Items List Form Updated Successfully.";
                    model.Updatedby = UserId;
                }
                model.Createdby = UserId;
                string i = masterItemsListForm.MasterItemsListFormInsertUpdate(model);
                if (i != "" && i != null)
                {
                    return Json(new { status = true, responseText = msg });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "MasterItemsListForm", "MasterItemsListFormDetailsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

    }
}
