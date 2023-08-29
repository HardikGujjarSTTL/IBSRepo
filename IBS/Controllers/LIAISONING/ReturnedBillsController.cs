//using CrystalDecisions.CrystalReports.Engine;
//using CrystalDecisions.Shared;
using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Drawing;
using System.Text.Json;

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
            string OrgType = "R";
            string Org = "SWR";
            DTResult<ReturnedBillsModel> dTResult = ReturnedBillsRepository.GetReturnedBills(dtParameters, OrgType, Org);
            return Json(dTResult);
        }
        

    }
}
