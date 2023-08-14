using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class BillingAdjustment : BaseController
    {
        #region Variables
        private readonly IBillingAdjustmentRepository billingAdjustmentRepository;
        #endregion
        public BillingAdjustment(IBillingAdjustmentRepository _billingAdjustmentRepository)
        {
            billingAdjustmentRepository = _billingAdjustmentRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Manage(string _AdjusmentYrMth)
        {
            BillingAdjustmentModel model = new();
            if (_AdjusmentYrMth != null)
            {
                model = billingAdjustmentRepository.FindByID(_AdjusmentYrMth, GetRegionCode);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<BillingAdjustmentModel> dTResult = billingAdjustmentRepository.GetBillingAdjustmentList(dtParameters,GetRegionCode);
            return Json(dTResult);
        }

        public IActionResult Delete(string _AdjusmentYrMth)
        {
            try
            {
                if(billingAdjustmentRepository.Remove(_AdjusmentYrMth, GetRegionCode))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "BillingAdjustment", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BillingAdjustmentDetailsSave(BillingAdjustmentModel model)
        {
            try
            {
                string msg = "Billing Adjustment Inserted Successfully.";

                if (model.Adjusment_Yr_Mth != null)
                {
                    msg = "Billing Adjustment Updated Successfully.";
                    //model.Updatedby = UserId;
                }
                //model.Createdby = UserId;
                model.Region_Code = GetRegionCode;
                var i = billingAdjustmentRepository.BillingAdjustmentDetailsInsertUpdate(model);
                if (i != null)
                {
                    return Json(new { status = true, responseText = msg });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "BillingAdjustment", "BillingAdjustmentDetailsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
    }
}
