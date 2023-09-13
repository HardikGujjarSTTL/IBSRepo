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
    public class LabSamInfoReportController : BaseController
    {
        #region Variables
        private readonly ILabSamInfoReportRepository LabSamInfoReportRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        #endregion
        public LabSamInfoReportController(ILabSamInfoReportRepository _LabSamInfoReportRepository, IWebHostEnvironment webHostEnvironment)
        {
            LabSamInfoReportRepository = _LabSamInfoReportRepository;
            _webHostEnvironment = webHostEnvironment;
        }
        
        public IActionResult LabSamInfoReport()
        {
           
            return View();
        }        
        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            string Regin = GetRegionCode;
            DTResult<LabSamInfoReportModel> dTResult = LabSamInfoReportRepository.LabSamInfoReport(dtParameters, Regin);          
            return Json(dTResult);
        }
        
    }
}
