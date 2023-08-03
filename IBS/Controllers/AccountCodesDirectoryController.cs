using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class AccountCodesDirectoryController : BaseController
    {
        #region Variables
        private readonly IAccountCodesDirectory accountCodes;
        #endregion
        public AccountCodesDirectoryController(IAccountCodesDirectory _accountCodes)
        {
            accountCodes = _accountCodes;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Manage(int id)
        {
            AccountCodesDirectoryModel model = new();
            if (id > 0)
            {
                model = accountCodes.FindByID(id);
            }
            return View(model);
        }


        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<AccountCodesDirectoryModel> dTResult = accountCodes.GetAccountCodesDirectoryList(dtParameters);
            return Json(dTResult);
        }
        public IActionResult Delete(int id)
        {
            try
            {
                if (accountCodes.Remove(id))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "AccountCodesDirectory", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AccountCodesDirectoryDetailsSave(AccountCodesDirectoryModel model)
        {
            try
            {
                string msg = "Account Codes Directory Inserted Successfully.";

                if (model.AccCd > 0)
                {
                    msg = "Account Codes Directory Updated Successfully.";
                    model.Updatedby = UserId;
                }
                model.Createdby = UserId;
                int i = accountCodes.AccountCodesDirectoryDetailsInsertUpdate(model);
                if (i > 0)
                {
                    return Json(new { status = true, responseText = msg });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "AccountCodesDirectory", "AccountCodesDirectoryDetailsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

    }
}

