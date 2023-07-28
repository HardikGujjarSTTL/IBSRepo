using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class LabBilling : BaseController
    {
        #region Variables
        private readonly ILabBillingRepository labBillingRepository;
        #endregion
        public LabBilling(ILabBillingRepository _labBillingRepository)
        {
            labBillingRepository = _labBillingRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Manage(int id)
        {
            LabBillingModel model = new();
            if (id > 0)
            {
                model = labBillingRepository.FindByID(id);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<LabBillingModel> dTResult = labBillingRepository.GetLabBillingList(dtParameters);
            return Json(dTResult);
        }

        public IActionResult Delete(int id)
        {
            try
            {
                if (labBillingRepository.Remove(id, UserId))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "LabBilling", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LabBillingDetailsSave(LabBillingModel model)
        {
            try
            {
                string msg = "Lab Bill Inserted Successfully.";

                if (model.Id > 0)
                {
                    msg = "Lab Bill Updated Successfully.";
                    model.Updatedby = UserId;
                }
                model.Createdby = UserId;
                int i = labBillingRepository.LabBillingDetailsInsertUpdate(model);
                if (i > 0)
                {
                    return Json(new { status = true, responseText = msg });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "LabBilling", "LabBillingDetailsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

    }
}
