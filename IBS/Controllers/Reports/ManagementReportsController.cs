using IBS.Filters;
using IBS.Interfaces;
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

        public IActionResult IEPerformance(DateTime FromDate, DateTime ToDate)
        {
            IEPerformanceModel model = managementReportsRepository.GetIEPerformanceData(FromDate, ToDate, Region);
            return View(model);
        }
    }
}
