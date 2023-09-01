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

        [Authorization("ClientMaster", "Index", "view")]
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

        [Authorization("ClientMaster", "Index", "view")]
        public IActionResult Manage(int id)
        {
            Clientmaster model = new();
            if (id > 0)
            {
                //model = roleRepository.FindUserRoleByID(id);
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
                string msg = "Client Inserted Successfully.";

                if (model.Id > 0)
                {
                    msg = "Client Updated Successfully.";
                    model.Updatedby = UserId;
                }
                model.Createdby = UserId;
                int i = clientMasterRepository.ClientDetailsInsertUpdate(model);
                if (i > 0)
                {
                    return Json(new { status = true, responseText = msg });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ClientMaster", "ClientDetailsave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

    }
}
