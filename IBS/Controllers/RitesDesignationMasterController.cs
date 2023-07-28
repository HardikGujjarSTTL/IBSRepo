using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class RitesDesignationMasterController : BaseController
    {
        #region Variables
        private readonly IRitesDesignationMaster ritesDesignationMaster;
        #endregion
        public RitesDesignationMasterController(IRitesDesignationMaster _ritesDesignationMaster)
        {

            ritesDesignationMaster = _ritesDesignationMaster;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Manage(int id)
        {
            RDMModel model = new();
            if (id > 0)
            {
                model = ritesDesignationMaster.FindByID(id);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<RDMModel> dTResult = ritesDesignationMaster.GetRDMList(dtParameters);
            return Json(dTResult);
        }
        public IActionResult Delete(int id)
        {
            try
            {
                if (ritesDesignationMaster.Remove(id, UserId))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Role", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RitesDesignationMasterDetailsSave(RDMModel model)
        {
            try
            {
                string msg = "Role Inserted Successfully.";

                if (model.RDesigCd > 0)
                {
                    msg = "Role Updated Successfully.";
                    model.Updatedby = UserId;
                }
                model.Createdby = UserId;
                int i = ritesDesignationMaster.RDMDetailsInsertUpdate(model);
                if (i > 0)
                {
                    return Json(new { status = true, responseText = msg });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Role", "RoleDetailsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

    }
}
