using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Text.Json;

namespace IBS.Controllers.Client
{
    public class SpecificPOCallStatusController : BaseController
    {
        #region Variables
        private readonly ISpecificPOCallStatusRepository SpecificPOCallStatusRepository;
        #endregion
        public SpecificPOCallStatusController(ISpecificPOCallStatusRepository _SpecificPOCallStatusRepository)
        {
            SpecificPOCallStatusRepository = _SpecificPOCallStatusRepository;
        }
        public IActionResult POCallStatus()
        {           
            return View();
        }
        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            
            DTResult<ClientCallRptModel> dTResult = new DTResult<ClientCallRptModel>();
            string OrgType = OrgnTypeClient;
            string Org = OrganisationClient;
            dTResult = SpecificPOCallStatusRepository.GetPOCallStatusIndex(dtParameters, OrgType, Org);
            
            return Json(dTResult);
        }
        [HttpPost]
        public IActionResult LoadTable1([FromBody] DTParameters dtParameters)
        {

            DTResult<ClientCallRptModel> dTResult = new DTResult<ClientCallRptModel>();

            dTResult = SpecificPOCallStatusRepository.GetPOCallStatus(dtParameters);

            return Json(dTResult);
        }

    }
}
