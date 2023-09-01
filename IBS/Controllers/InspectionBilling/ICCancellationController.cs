using IBS.Filters;
using IBS.Interfaces;
using IBS.Interfaces.Inspection_Billing;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers.InspectionBilling
{
    public class ICCancellationController : BaseController
    {
        #region Variables
        private readonly IICCancellationRepository iCCancellationRepository;
        #endregion
        public ICCancellationController(IICCancellationRepository _iCCancellationRepository)
        {
            iCCancellationRepository = _iCCancellationRepository;
        }
        public IActionResult Index()
        {
            string Region = Convert.ToString(IBS.Helper.SessionHelper.UserModelDTO.Region);
            ViewBag.Region = Region;
            return View();
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            string Region = Convert.ToString(IBS.Helper.SessionHelper.UserModelDTO.Region);
            DTResult<ICCancellationListModel> dTResult = iCCancellationRepository.GetCancellationList(dtParameters, Region);
            return Json(dTResult);
        }

        public IActionResult Manage(string REGION,string BK_NO,string SET_NO)
        {
            ICCancellationModel model = new();
            string Region = Convert.ToString(IBS.Helper.SessionHelper.UserModelDTO.Region);
            ViewBag.Region = Region;
            if (REGION != null && BK_NO != "" && SET_NO != "")
            {
                model = iCCancellationRepository.FindByID(REGION, BK_NO, SET_NO);
                model.IsEdit = 1;
            }
            else
            {
                model.IsEdit = 0;
                model.Region = Region;
            }
            return View(model);
        }

        public IActionResult Delete(string REGION, string BK_NO, string SET_NO)
        {
            try
            {
                if (iCCancellationRepository.Remove(REGION, BK_NO, SET_NO))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ICCancellation", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ICCancellationSave(ICCancellationModel model)
        {
            try
            {
                if (model.IsEdit > 0)
                {
                    iCCancellationRepository.ICCancellationSave(model);
                    AlertAddSuccess("IC Cancellation Updated Successfully.");
                }
                else
                {
                    iCCancellationRepository.ICCancellationSave(model);
                    AlertAddSuccess("IC Cancellation Inserted Successfully.");
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ICCancellation", "ICCancellationSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
    }
}
