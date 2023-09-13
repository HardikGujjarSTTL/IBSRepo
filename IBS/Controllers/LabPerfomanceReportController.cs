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
    public class LabPerfomanceReportController : BaseController
    {
        #region Variables
        private readonly ILabPerfomanceReportRepository LabPerfomanceReportRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        #endregion
        public LabPerfomanceReportController(ILabPerfomanceReportRepository _LabPerfomanceReportRepository, IWebHostEnvironment webHostEnvironment)
        {
            LabPerfomanceReportRepository = _LabPerfomanceReportRepository;
            _webHostEnvironment = webHostEnvironment;
        }
        
        public IActionResult LabPerformanceReport()
        {
            
            return View();
        }        
        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            string Regin = GetRegionCode;
            DTResult<LabPerfomanceReport> dTResult = LabPerfomanceReportRepository.labPerformanceReport(dtParameters, Regin);          
            return Json(dTResult);
        }
        
    }
}
