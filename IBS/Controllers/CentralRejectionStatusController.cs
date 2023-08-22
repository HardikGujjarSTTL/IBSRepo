using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class CentralRejectionStatusController : BaseController
    {
        #region Variables
        private readonly ICentralRejectionStatusRepository CentralRejectionStatusRepository;
        #endregion
        public CentralRejectionStatusController(ICentralRejectionStatusRepository _CentralRejectionStatusRepository)
        {
            CentralRejectionStatusRepository = _CentralRejectionStatusRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Manage(int ID)
        {
            CentralRejectionStatusModel model = new();
            if (ID>0)
            {
                model = CentralRejectionStatusRepository.FindByID(ID);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<CentralRejectionStatusModel> dTResult = CentralRejectionStatusRepository.GetCentralRejectionStatusList(dtParameters);
            return Json(dTResult);
        }

        public IActionResult Delete(int ID)
        {
            try
            {
                if (CentralRejectionStatusRepository.Remove(ID, UserId))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "CentralRejectionStatus", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CentralRejectionStatusSave(CentralRejectionStatusModel model)
        {
            try
            {
                string msg = "Central Rejection Status Inserted Successfully.";
                string Region = IBS.Helper.SessionHelper.UserModelDTO.Region;
                if (model.Id > 0)
                {
                    msg = "Central Rejection Status Updated Successfully.";
                    model.Updatedby = UserId;
                }
                model.Createdby = UserId;
                model.RejDt = model.Year + model.Month;
                model.Region = Region;
                int i = CentralRejectionStatusRepository.InsertUpdateCentralRejectionStatus(model);
                if (i > 0)
                {
                    return Json(new { status = true, responseText = msg });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "CentralRejectionStatus", "CentralRejectionStatusSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
    }
}
