using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    [Authorization]
    public class BillingOperatingTarget : BaseController
    {
        #region Variables
        private readonly IBillingOperatingTargetRepository billingOperatingTargetRepository;
        #endregion
        public BillingOperatingTarget(IBillingOperatingTargetRepository _billingOperatingTargetRepository)
        {
            billingOperatingTargetRepository = _billingOperatingTargetRepository;
        }
        [Authorization("BillingOperatingTarget", "Index", "view")]
        public IActionResult Index()
        {
            return View();
        }
        [Authorization("BillingOperatingTarget", "Index", "view")]
        public IActionResult Manage(string _BePer)
        {
            BillingOperatingTargetModel model = new();
            if (_BePer != null)
            {
                model = billingOperatingTargetRepository.FindByID(_BePer, GetRegionCode);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<BillingOperatingTargetModel> dTResult = billingOperatingTargetRepository.GetBillingOperatingTargetList(dtParameters,GetRegionCode);
            return Json(dTResult);
        }
        [Authorization("BillingOperatingTarget", "Index", "delete")]
        public IActionResult Delete(string _highDt)
        {
            try
            {
                if(billingOperatingTargetRepository.Remove(_highDt, GetRegionCode))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "BillingOperatingTarget", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorization("BillingOperatingTarget", "Index", "add")]
        public IActionResult BillingOperatingDetailsSave(BillingOperatingTargetModel model)
        {
            try
            {
                string msg = "Billing Operating Inserted Successfully.";

                if (model.Be_Per != null)
                {
                    msg = "Billing Operating Updated Successfully.";
                    //model.Updatedby = UserId;
                }
                //model.Createdby = UserId;
                model.Region_Code = GetRegionCode;
                var i = billingOperatingTargetRepository.BillingOperatingDetailsInsertUpdate(model);
                if (i != null)
                {
                    return Json(new { status = true, responseText = msg });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "BillingOperatingTarget", "BillingOperatingDetailsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
    }
}
