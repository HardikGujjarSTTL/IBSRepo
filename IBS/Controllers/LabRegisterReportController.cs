﻿//using CrystalDecisions.CrystalReports.Engine;
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
    public class LabRegisterReportController : BaseController
    {
        #region Variables
        private readonly ILabRegisterReportRepository LabRegisterReportRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        #endregion
        public LabRegisterReportController(ILabRegisterReportRepository _LabRegisterReportRepository, IWebHostEnvironment webHostEnvironment)
        {
            LabRegisterReportRepository = _LabRegisterReportRepository;
            _webHostEnvironment = webHostEnvironment;
        }
        
        public IActionResult LabRegisterReport()
        {
            LabRegisterReport labRegisterReport = new LabRegisterReport();
            labRegisterReport.Region = GetRegionCode;
            return View(labRegisterReport);
        }        
        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            string Regin = GetRegionCode;
            DTResult<LabRegisterReport> dTResult = LabRegisterReportRepository.labRegisterReport(dtParameters, Regin);          
            return Json(dTResult);
        }
        
    }
}
