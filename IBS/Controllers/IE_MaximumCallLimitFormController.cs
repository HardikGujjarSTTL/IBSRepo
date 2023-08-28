using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class IE_MaximumCallLimitFormController : BaseController
    {
        #region Variables
        private readonly I_IE_MaximumCallLimitForm iE_MaximumCallLimitForm;
        #endregion
        public IE_MaximumCallLimitFormController(I_IE_MaximumCallLimitForm _iE_MaximumCallLimitForm)
        {
            iE_MaximumCallLimitForm = _iE_MaximumCallLimitForm;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Manage(string Region)
        {
            IE_MaximumCallLimitFormModel model = new();
            Region = GetRegionCode;
            if (Region != null)
            {
                model = iE_MaximumCallLimitForm.FindByID(Region);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<IE_MaximumCallLimitFormModel> dTResult = iE_MaximumCallLimitForm.GetIE_MaximumCallLimitFormList(dtParameters,GetRegionCode);
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
                string msg = "Inserted Successfully.";

                if (model.RegionCode != null)
                {
                    msg = "Updated Successfully.";
                    model.Updatedby = UserId;
                }
                model.Createdby = UserId;
                string i = iE_MaximumCallLimitForm.IE_MaximumCallLimitFormDetailsInsertUpdate(model);
                if (i != null)
                {
                    AlertAddSuccess(msg);
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "IE_MaximumCallLimitForm", "Save_Update", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

    }
}

