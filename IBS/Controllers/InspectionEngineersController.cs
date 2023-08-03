using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class InspectionEngineersController : BaseController
    {
        #region Variables
        private readonly IInspectionEngineers inspectionEngineers;
        #endregion
        public InspectionEngineersController(IInspectionEngineers _inspectionEngineers)
        {
            inspectionEngineers = _inspectionEngineers;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Manage(int id)
        {
            InspectionEngineersModel model = new();
            if (id > 0)
            {
                model = inspectionEngineers.FindByID(id);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<InspectionEngineersModel> dTResult = inspectionEngineers.GetInspectionEngineersList(dtParameters);
            return Json(dTResult);
        }
        public IActionResult Delete(int id)
        {
            try
            {
                if (inspectionEngineers.Remove(id, UserId))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "InspectionEngineers", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult InspectionEngineersDetailsSave(InspectionEngineersModel model)
        {
            try
            {
                string msg = "Inspection Engineers Inserted Successfully.";

                if (model.IeCd > 0)
                {
                    msg = "Inspection Engineers Updated Successfully.";
                    model.Updatedby = UserId;
                }
                model.Createdby = UserId;
                int i = inspectionEngineers.InspectionEngineersDetailsInsertUpdate(model);
                if (i > 0)
                {
                    return Json(new { status = true, responseText = msg });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "InspectionEngineers", "InspectionEngineersDetailsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

    }
}
