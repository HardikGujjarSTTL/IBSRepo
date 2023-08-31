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
    }
}
