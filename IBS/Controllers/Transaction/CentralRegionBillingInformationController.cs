using IBS.Filters;
using IBS.Helper;
using IBS.Interfaces.Transaction;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers.Transaction
{
    [Authorization]
    public class CentralRegionBillingInformationController : BaseController
    {
        #region Variables
        private readonly ICentralRegionBillingInformationRepository centralRegionBillingInformationRepository;
        #endregion
        public CentralRegionBillingInformationController(ICentralRegionBillingInformationRepository _centralRegionBillingInformationRepository)
        {
            centralRegionBillingInformationRepository = _centralRegionBillingInformationRepository;
        }
        [Authorization("CentralRegionBillingInformation", "Index", "view")]
        public IActionResult Index()
        {
            string Region = SessionHelper.UserModelDTO.Region;
            string wRegion = "";
            if (Region == "N") { wRegion = "Northern Region"; }
            else if (Region == "S") { wRegion = "Southern Region"; }
            else if (Region == "E") { wRegion = "Eastern Region"; }
            else if (Region == "W") { wRegion = "Western Region"; }
            else if (Region == "C") { wRegion = "Central Region"; }
            ViewBag.Region = wRegion;
            return View();
        }
        [Authorization("CentralRegionBillingInformation", "Index", "view")]
        public IActionResult Manage(string id)
        {
            CentralRegionBillingInformationModel model = new();
            if (id != "" && id != null)
            {
                model = centralRegionBillingInformationRepository.FindByID(id);
                model.IsEdited = true;
            }
            else
            {
                model.IsEdited = false;
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            string Region = SessionHelper.UserModelDTO.Region;
            DTResult<CentralRegionBillingInformationListModel> dTResult = centralRegionBillingInformationRepository.GetBillingInformationList(dtParameters, Region);
            return Json(dTResult);
        }
        [Authorization("CentralRegionBillingInformation", "Index", "delete")]
        public IActionResult Delete(string id)
        {
            try
            {
                if (centralRegionBillingInformationRepository.Remove(id, UserId))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "CentralRegionBillingInformation", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult CheckAlreadyExist(string BillNo)
        {
            try
            {
                if (centralRegionBillingInformationRepository.AlreadyExist(BillNo))
                {
                    return Json(new { status = false, responseText = "Bill No Already Exist." });
                }
                else
                {
                    return Json(new { status = true, responseText = "" });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "CentralRegionBillingInformation", "CheckAlreadyExist", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [Authorization("CentralRegionBillingInformation", "Index", "edit")]
        public IActionResult BillingInformationSave(CentralRegionBillingInformationModel model)
        {
            try
            {
                string Region = SessionHelper.UserModelDTO.Region;
                model.UserId = USER_ID.Length > 8 ? USER_ID.Substring(0, 8) : USER_ID;
                model.Region = Region;
                if (model.BillNo != null && model.IsEdited == true)
                {
                    model.Updatedby = UserId;
                    centralRegionBillingInformationRepository.BillingInformationInsertUpdate(model);
                    AlertAddSuccess("Record Added Successfully.");
                }
                else
                {
                    model.Createdby = UserId;
                    centralRegionBillingInformationRepository.BillingInformationInsertUpdate(model);
                    AlertAddSuccess("Record Updated Successfully.");
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "CentralRegionBillingInformation", "BillingInformationSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
    }
}
