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
            if (ReportType == "IE_X") model.ReportTitle = "IE Performance";
            else if (ReportType == "CLUSTER_X") model.ReportTitle = "Cluster Wise Performance Report";

            return View(model);
        }

        public IActionResult ManageRWB(string ReportType, string FromYearMonth, string ToYearMonth)
        {
            ManagementReportsModel model = new() { ReportType = ReportType, FromYearMonth = FromYearMonth, ToYearMonth = ToYearMonth };
            if (ReportType == "RWB") model.ReportTitle = "Region Wise Billing Summary";
            return View("Manage", model);
        }

        public IActionResult ManageRWCO(string ReportType, DateTime FromDate)
        {
            ManagementReportsModel model = new() { ReportType = ReportType, FromDate = FromDate };
            if (ReportType == "R") model.ReportTitle = "Region Wise Comparison of Outstanding";
            return View("Manage", model);
        }

        public IActionResult IEPerformance(DateTime FromDate, DateTime ToDate)
        {
            IEPerformanceModel model = managementReportsRepository.GetIEPerformanceData(FromDate, ToDate, Region);
            return PartialView(model);
        }

        public IActionResult ClusterPerformance(DateTime FromDate, DateTime ToDate)
        {
            ClusterPerformanceModel model = managementReportsRepository.GetClusterPerformanceData(FromDate, ToDate, Region);
            return PartialView(model);
        }

        public IActionResult RegionWiseBillingSummary(string FromYearMonth, string ToYearMonth)
        {
            RWBSummaryModel model = managementReportsRepository.GetRWBSummaryData(FromYearMonth, ToYearMonth);
            return PartialView(model);
        }

        public IActionResult RegionWiseComparisonOutstanding(DateTime FromDate)
        {
            RWCOModel model = managementReportsRepository.GetRWCOData(FromDate);
            return PartialView(model);
        }

    }
}
