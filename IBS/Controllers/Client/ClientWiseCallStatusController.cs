using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Text.Json;

namespace IBS.Controllers.Client
{
    public class ClientWiseCallStatusController : BaseController
    {
        #region Variables
        private readonly IClientCallStatusRepository ClientCallStatusRepository;
        #endregion
        public ClientWiseCallStatusController(IClientCallStatusRepository _ClientCallStatusRepository)
        {
            ClientCallStatusRepository = _ClientCallStatusRepository;
        }
        public IActionResult ClientCallStatusReport()
        {

            return View();
        }
        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            //string Regin = GetRegionCode;
            DTResult<ClientCallRptModel> dTResult = ClientCallStatusRepository.GetCallStatus(dtParameters);
            return Json(dTResult);
        }

    }
}
