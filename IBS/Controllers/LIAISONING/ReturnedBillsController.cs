//using CrystalDecisions.CrystalReports.Engine;
//using CrystalDecisions.Shared;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class ReturnedBillsController : BaseController
    {
        #region Variables
        private readonly IReturnedBillsRepository ReturnedBillsRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        #endregion
        public ReturnedBillsController(IReturnedBillsRepository _ReturnedBillsRepository, IWebHostEnvironment webHostEnvironment)
        {
            ReturnedBillsRepository = _ReturnedBillsRepository;
            _webHostEnvironment = webHostEnvironment;
        }


        public IActionResult ReturnedBills()
        {

            return View();
        }
        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            string OrgType = OrgnType;
            string Org = Organisation;
            DTResult<ReturnedBillsModel> dTResult = ReturnedBillsRepository.GetReturnedBills(dtParameters, OrgType, Org);
            return Json(dTResult);
        }


    }
}
