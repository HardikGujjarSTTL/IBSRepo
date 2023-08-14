using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class IE_MaximumCallLimitFormController : BaseController
    {
        #region Variables
        private readonly IE_MaximumCallLimitForm iE_MaximumCallLimitForm;
        #endregion
        public IE_MaximumCallLimitFormController(IE_MaximumCallLimitForm _iE_MaximumCallLimitForm)
        {
            iE_MaximumCallLimitForm = _iE_MaximumCallLimitForm;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Manage(int id)
        {
            IE_MaximumCallLimitFormModel model = new();
            if (id > 0)
            {
                //model = iE_MaximumCallLimitForm.FindByID(id);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<IE_MaximumCallLimitFormModel> dTResult = iE_MaximumCallLimitForm.GetIE_MaximumCallLimitFormList(dtParameters);
            return Json(dTResult);
        }
        public IActionResult Delete(int id)
        {
            try
            {
                if (iE_MaximumCallLimitForm.Remove(id, UserId))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "IE_MaximumCallLimitForm", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult IE_MaximumCallLimitFormDetailsSave(IE_MaximumCallLimitFormModel model)
        {
            try
            {
                string msg = "IE Maximum Call Limit Form Inserted Successfully.";

                //if (model.RegionCode > 0)
                {
                    msg = "IE Maximum Call Limit Form Updated Successfully.";
                    model.Updatedby = UserId;
                }
                model.Createdby = UserId;
                int i = iE_MaximumCallLimitForm.IE_MaximumCallLimitFormDetailsInsertUpdate(model);
                if (i > 0)
                {
                    return Json(new { status = true, responseText = msg });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "IE_MaximumCallLimitForm", "IE_MaximumCallLimitFormDetailsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

    }
}

