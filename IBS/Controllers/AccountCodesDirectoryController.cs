using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class AccountCodesDirectoryController : BaseController
    {
        private readonly IAccountCodesDirectoryRepository accountCodesRepository;

        public AccountCodesDirectoryController(IAccountCodesDirectoryRepository _accountCodesRepository)
        {
            accountCodesRepository = _accountCodesRepository;
        }

        [Authorization("AccountCodesDirectory", "Index", "view")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorization("AccountCodesDirectory", "Index", "view")]
        public IActionResult Manage(int id)
        {
            AccountCodesDirectoryModel model = new();
            if (id > 0)
            {
                model = accountCodesRepository.FindByID(id);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<AccountCodesDirectoryModel> dTResult = accountCodesRepository.GetAccountCodesDirectoryList(dtParameters);
            return Json(dTResult);
        }

        [HttpPost]
        [Authorization("AccountCodesDirectory", "Index", "edit")]
        public IActionResult Manage(AccountCodesDirectoryModel model)
        {
            try
            {
                if (!accountCodesRepository.IsDuplicate(model))
                {
                    if (model.IsNew)
                    {
                        model.Createdby = UserId;
                        accountCodesRepository.SaveDetails(model);
                        AlertAddSuccess("Record Added Successfully.");
                    }
                    else
                    {
                        model.Updatedby = UserId;
                        accountCodesRepository.SaveDetails(model);
                        AlertAddSuccess("Record Updated Successfully.");
                    }

                    return RedirectToAction("Index");
                }
                else
                    AlertAlreadyExist();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "AccountCodesDirectory", "Manage", 1, GetIPAddress());
            }
            return View(model);
        }

        [Authorization("AccountCodesDirectory", "Index", "delete")]
        public IActionResult Delete(int id)
        {
            try
            {
                if (accountCodesRepository.Remove(id))
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
    }
}

