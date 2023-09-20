using IBS.Filters;
using IBS.Interfaces.Reports;
using IBS.Models.Reports;
using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp.Media;
using PuppeteerSharp;
using IBS.Helper;
using Newtonsoft.Json;

namespace IBS.Controllers.Reports
{
    [Authorization]
    public class ManagementReportsController : BaseController
    {
        private readonly IManagementReportsRepository managementReportsRepository;
        private readonly IWebHostEnvironment env;

        public ManagementReportsController(IManagementReportsRepository _managementReportsRepository, IWebHostEnvironment _env)
        {
            managementReportsRepository = _managementReportsRepository;
            this.env = _env;
        }

        public IActionResult Index()
        {
            ViewBag.Region = Region;
            return View();
        }

        public IActionResult Manage(string ReportType, DateTime FromDate, DateTime ToDate)
        {
            ManagementReportsModel model = new() { ReportType = ReportType, FromDate = FromDate, ToDate = ToDate };
            if (ReportType == "IE_X") model.ReportTitle = "IE Performance";
            else if (ReportType == "CLUSTER_X") model.ReportTitle = "Cluster Wise Performance Report";
            else if (ReportType == "ICSUBMIT") model.ReportTitle = "IC Submission Report";
            else if (ReportType == "CALLSWITHOUTIC") model.ReportTitle = "Pending IC's Against Calls where Material has been Sccepted or Rejected";

            return View(model);
        }

        public IActionResult ManageRWB(string ReportType, string FromYearMonth, string ToYearMonth)
        {
            ManagementReportsModel model = new() { ReportType = ReportType, FromYearMonth = FromYearMonth, ToYearMonth = ToYearMonth };
            if (ReportType == "RWB") model.ReportTitle = "Region Wise Billing Summary";
            return View("Manage", model);
        }

        public IActionResult ManageRWCO(string ReportType, DateTime FromDate, string Outstanding)
        {
            ManagementReportsModel model = new() { ReportType = ReportType, FromDate = FromDate, Outstanding = Outstanding };
            if (ReportType == "R") model.ReportTitle = "Region Wise Comparison of Outstanding";
            return View("Manage", model);
        }

        public IActionResult ManageSuperSurprise(string ReportType, DateTime FromDate, DateTime ToDate, string ParticularCM, string ParticularSector)
        {
            ManagementReportsModel model = new() { ReportType = ReportType, FromDate = FromDate, ToDate = ToDate, ParticularCM = ParticularCM, ParticularSector = ParticularSector };
            if (ReportType == "SUPSUR") model.ReportTitle = "Super Surprise Details";
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

        public IActionResult RegionWiseComparisonOutstanding(DateTime FromDate, string Outstanding)
        {
            RWCOModel model = managementReportsRepository.GetRWCOData(FromDate, Outstanding);
            return PartialView(model);
        }

        public IActionResult ICSubmission(DateTime FromDate, DateTime ToDate)
        {
            ICSubmissionModel model = managementReportsRepository.GetICSubmissionData(FromDate, ToDate, Region);
            return PartialView(model);
        }

        public IActionResult PendingICAgainstCalls(DateTime FromDate, DateTime ToDate)
        {
            PendingICAgainstCallsModel model = managementReportsRepository.GetPendingICAgainstCallsData(FromDate, ToDate, Region);
            return PartialView(model);
        }

        public IActionResult SuperSurpriseDetails(DateTime FromDate, DateTime ToDate, string ParticularCM, string ParticularSector)
        {
            SuperSurpriseDetailsModel model = managementReportsRepository.GetSuperSurpriseDetailsData(FromDate, ToDate, Region, ParticularCM, ParticularSector);
            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> GeneratePDF(string htmlContent)
        {
            //PendingICAgainstCallsModel _model = JsonConvert.DeserializeObject<PendingICAgainstCallsModel>(TempData[model.ReportType].ToString());
            //htmlContent = await this.RenderViewToStringAsync("/Views/ManagementReports/PendingICAgainstCalls.cshtml", _model);

            await new BrowserFetcher().DownloadAsync();
            await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
                DefaultViewport = null
            });
            await using var page = await browser.NewPageAsync();
            await page.EmulateMediaTypeAsync(MediaType.Screen);
            await page.SetContentAsync(htmlContent);

            string cssPath = env.WebRootPath + "/css/report.css";

            AddTagOptions bootstrapCSS = new AddTagOptions() { Path = cssPath };
            await page.AddStyleTagAsync(bootstrapCSS);

            var pdfContent = await page.PdfStreamAsync(new PdfOptions
            {
                Landscape = true,
                Format = PaperFormat.Letter,
                PrintBackground = true
            });

            await browser.CloseAsync();

            return File(pdfContent, "application/pdf", Guid.NewGuid().ToString() + ".pdf");
        }
    }
}