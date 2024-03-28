//using CrystalDecisions.CrystalReports.Engine;
//using CrystalDecisions.Shared;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class LabPostingReportController : BaseController
    {
        #region Variables
        private readonly ILabPostingReportRepository LabPostingReportRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        #endregion
        public LabPostingReportController(ILabPostingReportRepository _LabPostingReportRepository, IWebHostEnvironment webHostEnvironment)
        {
            LabPostingReportRepository = _LabPostingReportRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult LabPostingReport()
        {

            return View();
        }
        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            string Regin = GetRegionCode;
            DTResult<LabPostingReport> dTResult = LabPostingReportRepository.labPostingReport(dtParameters, Regin);
            return Json(dTResult);
        }

    }
}
