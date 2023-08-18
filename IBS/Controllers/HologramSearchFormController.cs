using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class HologramSearchFormController : BaseController
    {
        #region Variables
        private readonly IHologramSearchForm hologramSearchForm;
        #endregion
        public HologramSearchFormController(IHologramSearchForm _hologramSearchForm)
        {
            hologramSearchForm = _hologramSearchForm;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Manage(int id)
        {
            HologramSearchFormModel model = new();
            if (id > 0)
            {
                model = hologramSearchForm.FindByID(id);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<HologramSearchFormModel> dTResult = hologramSearchForm.GetHologramSearchFormList(dtParameters);
            return Json(dTResult);
        }
        public IActionResult Delete(string id)
        {
            try
            {
                if (hologramSearchForm.Remove(id, UserId))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "HologramSearchForm", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult HologramSearchFormDetailsSave(HologramSearchFormModel model)
        {
            try
            {
                string msg = "Search of Hologram to IE Inserted Successfully.";

                //if (model.HgNoFr > 0)
                {
                    msg = "Search of Hologram to IE Updated Successfully.";
                    model.Updatedby = UserId;
                }
                model.Createdby = UserId;
                int i = hologramSearchForm.HologramSearchFormDetailsInsertUpdate(model);
                if (i > 0)
                {
                    return Json(new { status = true, responseText = msg });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "HologramSearchForm", "HologramSearchFormDetailsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

    }
}
