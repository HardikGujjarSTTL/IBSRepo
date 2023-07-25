using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class BankMasterController : BaseController
    {
        #region Variables
        private readonly IBankMaster bankMaster;
        #endregion
        public BankMasterController(IBankMaster _bankMaster)
        {
            bankMaster = _bankMaster;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Manage(int id)
        {
            BankMasterModel model = new();
            if (id > 0)
            {
                model = bankMaster.FindByID(id);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<BankMasterModel> dTResult = bankMaster.GetBankMasterList(dtParameters);
            return Json(dTResult);
        }
        public IActionResult Delete(int id)
        {
            try
            {
                if (bankMaster.Remove(id, UserId))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "BankMaster", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BankMasterDetailsSave(BankMasterModel model)
        {
            try
            {
                string msg = "Bank Master Inserted Successfully.";

                if (model.BankCd > 0)
                {
                    msg = "BankMaster Updated Successfully.";
                    model.Updatedby = UserId;
                }
                model.Createdby = UserId;
                int i = bankMaster.BankMasterDetailsInsertUpdate(model);
                if (i > 0)
                {
                    return Json(new { status = true, responseText = msg });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "BankMaster", "BankMasterDetailsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

    }
}
