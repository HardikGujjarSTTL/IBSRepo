using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    [Authorization]
    public class ClientContractController : BaseController
    {
        #region Variables
        private readonly IClientContractRepository clientcontractRepository;
        #endregion
        public ClientContractController(IClientContractRepository _clientcontractRepository)
        {
            clientcontractRepository = _clientcontractRepository;
        }
        [Authorization("ClientContract", "Index", "view")]
        public IActionResult Index(string type)
        {
            ViewBag.Type = type;
            return View();
        }
        [Authorization("ClientContract", "Index", "view")]
        public IActionResult Manage(int id, string Type)
        {
            string Region = IBS.Helper.SessionHelper.UserModelDTO.Region;
            ClientContractModel model = new();
            if (id > 0)
            {
                model = clientcontractRepository.FindByID(id);
            }
            else
            {
                model.TypeCb = Type;
            }
            model.RegionCd= Region;
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<ClientContractModel> dTResult = clientcontractRepository.GetClientContractList(dtParameters);
            return Json(dTResult);
        }
        [Authorization("ClientContract", "Index", "delete")]
        public IActionResult Delete(int id,string Type)
        {
            try
            {
                if (clientcontractRepository.Remove(id, UserId))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ClientContract", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index", new { type= Type });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorization("ClientContract", "Index", "edit")]
        public IActionResult ClientContractDetailsSave(ClientContractModel model)
        {
            try
            {
                string msg = "ClientContract Inserted Successfully.";
                if (model.Id > 0)
                {
                    msg = "ClientContract Updated Successfully.";
                    model.Updatedby = UserId;
                }
                model.Createdby = UserId;
                int i = clientcontractRepository.ClientContractDetailsInsertUpdate(model);
                if (i > 0)
                {
                    return Json(new { status = true, responseText = msg });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ClientContract", "ClientContractDetailsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        public IActionResult GetClientByClientType(string CoCd)
        {
            return Json(Common.GetClientByClientType(CoCd).ToList());
        }

    }
}
