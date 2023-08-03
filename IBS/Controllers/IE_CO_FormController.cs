using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class IE_CO_FormController : BaseController
    {
        #region Variables
        private readonly I_IE_CO_Form iE_CO_Form;
        #endregion
        public IE_CO_FormController(I_IE_CO_Form _iE_CO_Form)
        {
            iE_CO_Form = _iE_CO_Form;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Manage(int id)
        {
            IE_CO_FormModel model = new();
            if (id > 0)
            {
                model = iE_CO_Form.FindByID(id);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<IE_CO_FormModel> dTResult = iE_CO_Form.GetCOList(dtParameters);
            return Json(dTResult);
        }
        public IActionResult Delete(int id)
        {
            try
            {
                if (iE_CO_Form.Remove(id, UserId))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "IE_CO_Form", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult IE_CO_FormDetailsSave(IE_CO_FormModel model)
        {
            try
            {
                string msg = "Controlling Officers of Inspecting Engineers Inserted Successfully.";

                if (model.CoCd > 0)
                {
                    msg = "Controlling Officers of Inspecting Engineers Updated Successfully.";
                    model.Updatedby = UserId;
                }
                model.Createdby = UserId;
                int i = iE_CO_Form.CODetailsInsertUpdate(model);
                if (i > 0)
                {
                    return Json(new { status = true, responseText = msg });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "IE_CO_Form", "IE_CO_FormDetailsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

    }
}