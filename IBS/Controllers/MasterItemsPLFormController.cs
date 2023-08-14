using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class MasterItemsPLFormController : BaseController
    {
        #region Variables
        private readonly IMasterItemsPLForm masterItemsPLForm;
        #endregion
        public MasterItemsPLFormController(IMasterItemsPLForm _masterItemsPLForm)
        {
            masterItemsPLForm = _masterItemsPLForm;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Manage(int id)
        {
            MasterItemsPLFormModel model = new();
            if (id > 0)
            {
                model = masterItemsPLForm.FindByID(id);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<MasterItemsPLFormModel> dTResult = masterItemsPLForm.GetMasterItemsPLFormList(dtParameters);
            return Json(dTResult);
        }
        public IActionResult Delete(int id)
        {
            try
            {
                if (masterItemsPLForm.Remove(id, UserId))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "MasterItemsPLForm", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MasterItemsPLFormDetailsSave(MasterItemsPLFormModel model)
        {
            try
            {
                string msg = "Master Items PL Form Inserted Successfully.";

              //  if (model.ItemCd > 0)
                {
                    msg = "Master Items PL Form Updated Successfully.";
                    model.Updatedby = UserId;
                }
                model.Createdby = UserId;
                int i = masterItemsPLForm.MasterItemsPLFormDetailsInsertUpdate(model);
                if (i > 0)
                {
                    return Json(new { status = true, responseText = msg });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "MasterItemsPLForm", "MasterItemsPLFormDetailsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

    }
}
