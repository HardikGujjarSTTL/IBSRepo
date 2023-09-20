using IBS.Helper;
using IBS.Interfaces;
using IBS.Interfaces.Reports;
using IBS.Models;
using IBS.Models.Reports;
using IBS.Repositories.Reports;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers.Reports
{
    public class DefectCodeReportController : BaseController
    {
        #region Variables
        private readonly IDefectCodeReportRepository defectCodeReportRepository;
        #endregion
        public DefectCodeReportController(IDefectCodeReportRepository _defectCodeReportRepository)
        {
            defectCodeReportRepository = _defectCodeReportRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Manage(DateTime FromDate, DateTime ToDate)
        {
            DefectCodeReport model = new() {FromDate = FromDate, ToDate = ToDate };
            model.ReportTitle = "Defect Code Wise Analysis of Complaints";
            return View(model);
        }

        public IActionResult DefectCodeReport(DateTime FromDate, DateTime ToDate)
        {
            string Region = SessionHelper.UserModelDTO.Region;
            string wRegion = "";
            if (Region == "N") { wRegion = "Northern Region"; }
            else if (Region == "S") { wRegion = "Southern Region"; }
            else if (Region == "E") { wRegion = "Eastern Region"; }
            else if (Region == "W") { wRegion = "Western Region"; }
            else if (Region == "C") { wRegion = "Central Region"; }
            DefectCodeReport model = defectCodeReportRepository.GetDefectCodeWiseData(FromDate, ToDate, Region);
            ViewBag.Regions = wRegion;
            return PartialView(model);
        }
    }
}
