using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class MasterItemsPLFormController : BaseController
    {
        private readonly IMasterItemsPLFormRepository masterItemsPLFormRepository;

        public MasterItemsPLFormController(IMasterItemsPLFormRepository _masterItemsPLFormRepository)
        {
            masterItemsPLFormRepository = _masterItemsPLFormRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Manage(string id)
        {
            MasterItemsPLFormModel model = new();
            if (!string.IsNullOrEmpty(id))
            {
                model = masterItemsPLFormRepository.FindByID(id);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<MasterItemsPLFormModel> dTResult = masterItemsPLFormRepository.GetMasterItemsPLFormList(dtParameters);
            return Json(dTResult);
        }

        [HttpPost]
        public IActionResult Manage(MasterItemsPLFormModel model)
        {
            try
            {
                if (!masterItemsPLFormRepository.IsDuplicate(model))
                {
                    if (model.IsNew)
                    {
                        model.Createdby = UserId;
                        model.UserId = USER_ID.Substring(0, 8);
                        masterItemsPLFormRepository.SaveDetails(model);
                        AlertAddSuccess("Record Added Successfully.");
                    }
                    else
                    {
                        model.Updatedby = UserId;
                        model.UserId = USER_ID.Substring(0, 8);
                        masterItemsPLFormRepository.SaveDetails(model);
                        AlertAddSuccess("Record Updated Successfully.");
                    }

                    return RedirectToAction("Index");
                }
                else
                    AlertAlreadyExist();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "RailwaysDirectory", "Manage", 1, GetIPAddress());
            }
            return View(model);
        }

        public IActionResult Delete(string id)
        {
            try
            {
                if (masterItemsPLFormRepository.Remove(id))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "MasterItemsPLForm", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }

    }
}
