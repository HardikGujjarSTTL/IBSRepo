using IBS.DataAccess;
using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    [Authorization]
    public class CentralQOIIController : BaseController
    {
        #region Variables
        private readonly ICentralQOIIRepository centralQOIIRepository;
        #endregion
        public CentralQOIIController(ICentralQOIIRepository _centralQOIIRepository)
        {
            centralQOIIRepository = _centralQOIIRepository;
        }
        [Authorization("CentralQOII", "Index", "view")]
        public IActionResult Index()
        {
            return View();
        }
        [Authorization("CentralQOII", "Index", "view")]
        public IActionResult Manage(string Client, string QoiDate, string Weight, string QoiLength)
        {
            CentralQOIIModel model = new();
            model.IsEdited = false;
            if (Client != null && QoiDate != null && Weight != null && QoiLength != null)
            {
                model = centralQOIIRepository.FindByID(Client, QoiDate, Weight, QoiLength);
                model.IsEdited = true;
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<CentralQOIIModel> dTResult = centralQOIIRepository.GetCentralQOIIList(dtParameters);
            return Json(dTResult);
        }

        [Authorization("CentralQOII", "Index", "delete")]
        public IActionResult Delete(string Client, string QoiDate, string Weight, string QoiLength)
        {
            try
            {
                if (centralQOIIRepository.Remove(Client, QoiDate, UserId, Weight,QoiLength))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "CentralQOII", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorization("CentralQOII", "Index", "add")]
        public IActionResult CentralQOISave(CentralQOIIModel model)
        {
            try
            {
                string msg = "Inserted Successfully.";
                string Region = IBS.Helper.SessionHelper.UserModelDTO.Region;
                model.Updatedby = UserId;
                model.Createdby = UserId;
                model.QoiDate = model.Year + model.Month;
                model.Region = Region;
                string i = centralQOIIRepository.InsertUpdateCentralQOII(model);
                if (i == "Already Exist")
                {
                    return Json(new { status = false, responseText = "Selected Period and Client Already Exist" });
                }
                if (i != null && i != "Already Exist")
                {
                    return Json(new { status = true, responseText = msg });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "CentralQOII", "CentralQOISave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
    }
}
