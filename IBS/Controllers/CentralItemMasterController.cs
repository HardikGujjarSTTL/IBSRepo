using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    [Authorization]
    public class CentralItemMasterController : BaseController
    {
        #region Variables
        private readonly ICentralItemMasterRepository centralItemMasterRepository;
        #endregion
        public CentralItemMasterController(ICentralItemMasterRepository _centralItemMasterRepository)
        {
            centralItemMasterRepository = _centralItemMasterRepository;
        }

        #region Master
        [Authorization("CentralItemMaster", "Index", "view")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorization("CentralItemMaster", "Index", "view")]
        public IActionResult Manage(int id)
        {
            CentralItemMasterModel model = new();
            if (id > 0)
            {
                model = centralItemMasterRepository.FindByID(id);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<CentralItemMasterModel> dTResult = centralItemMasterRepository.GetList(dtParameters);
            return Json(dTResult);
        }
        [Authorization("CentralItemMaster", "Index", "delete")]
        public IActionResult Delete(int id)
        {
            try
            {
                if (centralItemMasterRepository.Remove(id, UserId))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "CentralItemMaster", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorization("CentralItemMaster", "Index", "edit")]
        public IActionResult SaveMaster(CentralItemMasterModel model)
        {
            try
            {
                model.UserId = USER_ID.Substring(0, 8);
                if (centralItemMasterRepository.CheckAlreadyExist(model))
                {
                    AlertAlreadyExist("Record already exist.");
                    return RedirectToAction("Manage", "CentralItemMaster", new { id = model.Id });
                }
                else
                {
                    if (model.Id > 0)
                    {
                        model.Updatedby = UserId;
                        centralItemMasterRepository.InsertUpdate(model);
                        AlertAddSuccess("Record Added Successfully.");
                    }
                    else
                    {
                        model.Createdby = UserId;
                        centralItemMasterRepository.InsertUpdate(model);
                        AlertAddSuccess("Record Updated Successfully.");
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "CentralItemMaster", "SaveMaster", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
        #endregion

        #region Details
        [Authorization("CentralItemMaster", "Index", "view")]
        public IActionResult Details(int id)
        {
            ViewBag.RailId = id;
            return View();
        }
        [Authorization("CentralItemMaster", "Index", "view")]
        public IActionResult ManageDetails(int id, int RailId)
        {
            CentralItemDetailModel model = new();
            model.RailId = RailId;
            if (id > 0)
            {
                model = centralItemMasterRepository.FindDetailsByID(id);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadDetailsTable([FromBody] DTParameters dtParameters)
        {
            DTResult<CentralItemDetailModel> dTResult = centralItemMasterRepository.GetDetailsList(dtParameters);
            return Json(dTResult);
        }
        [Authorization("CentralItemMaster", "Index", "delete")]
        public IActionResult DeleteDetails(int id, int RailId)
        {
            try
            {
                if (centralItemMasterRepository.RemoveDetails(id, UserId))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "CentralItemMaster", "DeleteDetails", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Details", new { id = RailId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorization("CentralItemMaster", "Index", "edit")]
        public IActionResult SaveMasterDetails(CentralItemDetailModel model)
        {
            try
            {
                if (model.Id < 0)
                {
                    model.Updatedby = UserId;
                    centralItemMasterRepository.DetailsInsertUpdate(model);
                    AlertAddSuccess("Record Added Successfully.");
                }
                else
                {
                    model.Createdby = UserId;
                    centralItemMasterRepository.DetailsInsertUpdate(model);
                    AlertAddSuccess("Record Updated Successfully.");
                }
                return RedirectToAction("Details", new { id = model.RailId });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "CentralItemMaster", "SaveMasterDetails", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
        #endregion
    }
}
