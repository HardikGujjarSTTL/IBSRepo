using IBS.DataAccess;
using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    [Authorization]
    public class CentralQOIController : BaseController
    {
        #region Variables
        private readonly ICentralQOIRepository centralQOIRepository;
        #endregion
        public CentralQOIController(ICentralQOIRepository _centralQOIRepository)
        {
            centralQOIRepository = _centralQOIRepository;
        }
        [Authorization("CentralQOI", "Index", "view")]
        public IActionResult Index()
        {
            return View();
        }
        [Authorization("CentralQOI", "Index", "view")]
        public IActionResult Manage(string Client, string QtyDate)
        {
            CentralQOIModel model = new();
            model.IsEdited = false;
            if (Client != null && QtyDate != null)
            {
                model = centralQOIRepository.FindByID(Client, QtyDate);
                model.IsEdited = true;
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<CentralQOIModel> dTResult = centralQOIRepository.GetCentralQOIList(dtParameters);
            return Json(dTResult);
        }
        [Authorization("CentralQOI", "Index", "delete")]
        public IActionResult Delete(string Client,string QtyDate)
        {
            try
            {
                if (centralQOIRepository.Remove(Client, QtyDate, UserId))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "CentralQOI", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorization("CentralQOI", "Index", "edit")]
        public IActionResult CentralQOIISave(CentralQOIModel model)
        {
            try
            {
                string msg = "Inserted Successfully.";
                string Region = IBS.Helper.SessionHelper.UserModelDTO.Region;
                model.Updatedby = UserId;
                model.Createdby = UserId;
                model.QtyDate = model.Year + model.Month;
                model.RegionCode = Region;
                string i = centralQOIRepository.InsertUpdateCentralQOI(model);
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
                Common.AddException(ex.ToString(), ex.Message.ToString(), "CentralQOI", "CentralQOISave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
    }
}
