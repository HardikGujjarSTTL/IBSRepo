//using CrystalDecisions.CrystalReports.Engine;
//using CrystalDecisions.Shared;
using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Drawing;
using System.Text.Json;

namespace IBS.Controllers
{
    public class DownloadBillsController : BaseController
    {
        #region Variables
        private readonly IDownloadBillsRepository DownloadBillsRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        #endregion
        public DownloadBillsController(IDownloadBillsRepository _DownloadBillsRepository, IWebHostEnvironment webHostEnvironment)
        {
            DownloadBillsRepository = _DownloadBillsRepository;
            _webHostEnvironment = webHostEnvironment;
        }
        

        public IActionResult DownloadBills()
        {

            return View();
        }
        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            string OrgType = OrgnType;
            string Org = Organisation;
            DTResult<DownloadBillsModel> dTResult = DownloadBillsRepository.GetReturnedBills(dtParameters, OrgType, Org, _webHostEnvironment);
            return Json(dTResult);
        }
        

    }
}
