using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class ClientMasterController : BaseController
    {
        #region Variables
        private readonly IClientMasterRepository clientMasterRepository ;
        #endregion
        public ClientMasterController(IClientMasterRepository _clientMasterRepository)
        {
            clientMasterRepository = _clientMasterRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<Clientmaster> dTResult  = clientMasterRepository.GetClientList(dtParameters);
            return Json(dTResult);
        }

        public IActionResult Manage(int id)
        {
            Clientmaster model = new();
            if (id > 0)
            {
                model = clientMasterRepository.FindClientByID(id);
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorization("ClientMaster", "Index", "edit")]
        public IActionResult ClientDetailsave(Clientmaster model)
        {
            try
            {
                if (model.Id > 0)
                {
                    model.Updatedby = UserId;
                   int i = clientMasterRepository.ClientDetailsInsertUpdate(model);
                    if(i > 0)
                    {
                        AlertAddSuccess("Record Updated Successfully.");
                    }
                    else
                    {
                        AlertAlreadyExist("Entry Already Exist For Given Mobile!!!");
                        return View("Manage",model);
                    }
                }
                else
                {
                    model.Createdby = UserId;
                   int i = clientMasterRepository.ClientDetailsInsertUpdate(model);
                    if (i > 0)
                    {
                        AlertAddSuccess("Record Add Successfully.");
                    }
                    else
                    {
                        AlertAlreadyExist("Entry Already Exist For Given Mobile!!!");
                        return View("Manage", model);
                    }
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ClientMaster", "ClientDetailsave", 1, GetIPAddress());
            }
            return View("Manage", model);
        }

        [Authorization("ClientMaster", "Index", "delete")]
        public IActionResult Delete(int ID)
        {
            try
            {
                if (clientMasterRepository.Remove(ID, UserId))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ClientMaster", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }

    }
}
