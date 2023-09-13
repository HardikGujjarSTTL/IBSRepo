using IBS.Filters;
using IBS.Interfaces.Reports;
using IBS.Models.Reports;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers.Reports
{
    [Authorization]
    public class ManagementReportsController : BaseController
    {
        private readonly IManagementReportsRepository managementReportsRepository;

        public ManagementReportsController(IManagementReportsRepository _managementReportsRepository)
        {
            managementReportsRepository = _managementReportsRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Manage(string ReportType, DateTime FromDate, DateTime ToDate)
        {
            ManagementReportsModel model = new() { ReportType = ReportType, FromDate = FromDate, ToDate = ToDate };
            if (ReportType != "") model.ReportTitle = "IE Performance";
            return View(model);
        }

        public IActionResult IEPerformance(DateTime FromDate, DateTime ToDate)
        {
            IEPerformanceModel model = managementReportsRepository.GetIEPerformanceData(FromDate, ToDate, Region);
            return PartialView(model);
        }
    }
}
