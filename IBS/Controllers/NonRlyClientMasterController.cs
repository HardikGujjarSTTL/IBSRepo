using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class NonRlyClientMasterController : BaseController
    {
        #region Variables
        private readonly INonRlyClientMasterRepository nonRlyClientMasterRepository;
        #endregion
        public NonRlyClientMasterController(INonRlyClientMasterRepository _nonRlyClientMasterRepository)
        {
            nonRlyClientMasterRepository = _nonRlyClientMasterRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Manage(int id)
        {
            Clientmaster model = new();
            if (id > 0)
            {
                model = nonRlyClientMasterRepository.FindNonClientByID(id);
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorization("NonRlyClientMaster", "Index", "edit")]
        public IActionResult NonClientDetailsave(Clientmaster model)
        {
            try
            {
                if (model.Id > 0)
                {
                    model.Updatedby = UserId;
                    int i = nonRlyClientMasterRepository.ClientDetailsInsertUpdate(model);
                    if (i > 0)
                    {
                        AlertAddSuccess("Record Updated Successfully.");
                    }
                    else
                    {
                        AlertAlreadyExist("This BPO Orgnatation,BPO Railway And BPO Type Are Already Exist !!!");
                        return View("Manage", model);
                    }
                }
                else
                {
                    model.Createdby = UserId;
                    int i = nonRlyClientMasterRepository.ClientDetailsInsertUpdate(model);
                    if (i > 0)
                    {
                        AlertAddSuccess("Record Add Successfully.");
                    }
                    else
                    {
                        AlertAlreadyExist("This BPO Orgnatation,BPO Railway And BPO Type Are Already Exist !!!");
                        return View("Manage", model);
                    }
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "NonRlyClientMaster", "NonClientDetailsave", 1, GetIPAddress());
            }
            return View("Manage", model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<Clientmaster> dTResult = nonRlyClientMasterRepository.GetNonClientList(dtParameters);
            return Json(dTResult);
        }

        [Authorization("NonRlyClientMaster", "Index", "delete")]
        public IActionResult Delete(int ID)
        {
            try
            {
                if (nonRlyClientMasterRepository.Remove(ID, UserId))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "NonRlyClientMaster", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }
    }
}
