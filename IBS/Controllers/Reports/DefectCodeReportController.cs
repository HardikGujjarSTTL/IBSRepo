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
            DefectCodeReport model = defectCodeReportRepository.GetDefectCodeWiseData(FromDate, ToDate, Region);
            ViewBag.Regions = Region;
            return PartialView(model);
        }
    }
}
