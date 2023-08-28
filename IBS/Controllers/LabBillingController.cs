using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    [Authorization]
    public class LabBilling : BaseController
    {
        #region Variables
        private readonly ILabBillingRepository labBillingRepository;
        #endregion
        public LabBilling(ILabBillingRepository _labBillingRepository)
        {
            labBillingRepository = _labBillingRepository;
        }
        [Authorization("LabBilling", "Index", "view")]
        public IActionResult Index()
        {
            return View();
        }
        [Authorization("LabBilling", "Index", "view")]
        public IActionResult Manage(string _labBillPer)
        {
            LabBillingModel model = new();
            if (_labBillPer != null)
            {
                model = labBillingRepository.FindByID(_labBillPer, GetRegionCode);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<LabBillingModel> dTResult = labBillingRepository.GetLabBillingList(dtParameters,GetRegionCode);
            return Json(dTResult);
        }

        [Authorization("LabBilling", "Index", "delete")]
        public IActionResult Delete(string LabBillPer)
        {
            try
            {
                if (labBillingRepository.Remove(LabBillPer, GetRegionCode,UserId))
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
        [Authorization("LabBilling", "Index", "edit")]
        public IActionResult LabBillingDetailsSave(LabBillingModel model)
        {
            try
            {
                string msg = "Lab Bill Inserted Successfully.";

                if (model.Lab_Bill_Per != null)
                {
                    msg = "Lab Bill Updated Successfully.";
                    model.Updatedby = UserId;
                }
                model.Createdby = UserId;
                model.User_Id = Convert.ToString(UserId);
                model.Region_Code = GetRegionCode;
                var i = labBillingRepository.LabBillingDetailsInsertUpdate(model);
                if (i != null)
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
