using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class MasterTableStatusController : BaseController
    {
        #region Variables
        private readonly IMasterTableStatusRepository mastertablestatusRepository;
        #endregion
        public MasterTableStatusController(IMasterTableStatusRepository _mastertablestatusRepository)
        {
            mastertablestatusRepository = _mastertablestatusRepository;
        }

        public IActionResult MasterTableStatusList()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<MasterTableStatusModel> dTResult = mastertablestatusRepository.GetMessageList(dtParameters);
            return Json(dTResult);
        }
    }
}
