using IBS.Filters;
using IBS.Helper;
using IBS.Interfaces.Reports;
using IBS.Models.Reports;
using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace IBS.Controllers.Reports
{
    [Authorization]
    public class MonthlyReportsController : BaseController
    {
        private readonly IMonthlyReportsRepository monthlyReportsRepository;
        private readonly IWebHostEnvironment env;
        private readonly IConfiguration config;

        public MonthlyReportsController(IMonthlyReportsRepository _monthlyReportsRepository, IWebHostEnvironment _env, IConfiguration _config)
        {
            monthlyReportsRepository = _monthlyReportsRepository;
            this.env = _env;
            config = _config;
        }
        public IActionResult Index()
        {
            ViewBag.ReportUrl = config.GetSection("AppSettings")["ReportUrl"];
            ViewBag.Region = GetUserInfo.Region;
            return View();
        }

        public IActionResult Manage(string ReportType, DateTime FromDate, DateTime ToDate)
        {
            MonthlyReportModel model = new() { ReportType = ReportType, FromDate = FromDate, ToDate = ToDate };
            if (ReportType == "ReInspectionICs") model.ReportTitle = "RE-INSPECTION IC's";
            return View(model);
        }

        public IActionResult ManageICStatus(string ReportType, DateTime FromDate, DateTime ToDate, string IE_CD)
        {
            MonthlyReportModel model = new() { ReportType = ReportType, FromDate = FromDate, ToDate = ToDate, Ie_Cd = IE_CD };
            if (ReportType == "ICStatus") model.ReportTitle = "IC ACCOUNTAL STATEMENT";
            return View("Manage", model);
        }

        public IActionResult ICStatusAll(DateTime FromDate, DateTime ToDate, string IE_CD)
        {
            AllICStatusModel model = monthlyReportsRepository.GetAllICStatus(FromDate, ToDate, IE_CD, Region);
            GlobalDeclaration.AllICStatus = model;
            return PartialView(model);
        }

        public IActionResult ReInspectionICs(DateTime FromDate, DateTime ToDate)
        {
            ReInspectionICsModel model = monthlyReportsRepository.GetReInspectionICs(FromDate, ToDate, Region);
            GlobalDeclaration.ReInspectionICs = model;
            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> GeneratePDF(string ReportType)
        {
            string htmlContent = string.Empty;

            if (ReportType == "ICStatus")
            {
                AllICStatusModel model = GlobalDeclaration.AllICStatus;
                htmlContent = await this.RenderViewToStringAsync("/Views/MonthlyReports/ICStatusAll.cshtml", model);
            }
            else if (ReportType == "ReInspectionICs")
            {
                ReInspectionICsModel model = GlobalDeclaration.ReInspectionICs;
                htmlContent = await this.RenderViewToStringAsync("/Views/MonthlyReports/ReInspectionICs.cshtml", model);
            }

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
