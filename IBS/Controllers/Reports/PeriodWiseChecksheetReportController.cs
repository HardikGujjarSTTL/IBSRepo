using IBS.Filters;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Interfaces.Transaction;
using IBS.Models;
using IBS.Repositories.Transaction;
using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp.Media;
using PuppeteerSharp;
using IBS.Models.Reports;
using IBS.Interfaces.Reports;
using IBS.Repositories.Reports;

namespace IBS.Controllers.Reports
{
    public class PeriodWiseChecksheetReportController : BaseController
    {
        #region Variables
        private readonly IPeriodWiseChecksheetReportRepository periodWiseChecksheetReportRepository;
        private readonly IWebHostEnvironment env;
        #endregion
        public PeriodWiseChecksheetReportController(IPeriodWiseChecksheetReportRepository _periodWiseChecksheetReportRepository, IWebHostEnvironment _env)
        {
            periodWiseChecksheetReportRepository = _periodWiseChecksheetReportRepository;
            this.env = _env;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Manage(string FromDate, string ToDate)
        {
            PeriodWiseChecksheetReportModel model = new()
            {
                FromDate = FromDate,
                ToDate = ToDate,
            };
            model.ReportTitle = "period wise checksheet";
            return View(model);
        }

        public IActionResult PeriodWiseChecksheet(string FromDate, string ToDate)
        {
            string Region = SessionHelper.UserModelDTO.Region;
            string wRegion = "";
            if (Region == "N") { wRegion = "Northern Region"; }
            else if (Region == "S") { wRegion = "Southern Region"; }
            else if (Region == "E") { wRegion = "Eastern Region"; }
            else if (Region == "W") { wRegion = "Western Region"; }
            else if (Region == "C") { wRegion = "Central Region"; }
            ViewBag.Regions = wRegion;
            ViewBag.frmdt = FromDate;
            ViewBag.todt = ToDate;
            PeriodWiseChecksheetReportModel model = periodWiseChecksheetReportRepository.Getperiodwisechecksheetdetails(FromDate, ToDate, Region);
            GlobalDeclaration.PeriodWiseChecksheetReport = model;
            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> GeneratePDF()
        {
            string htmlContent = string.Empty;

            PeriodWiseChecksheetReportModel model = GlobalDeclaration.PeriodWiseChecksheetReport;
            htmlContent = await this.RenderViewToStringAsync("/Views/PeriodWiseChecksheetReport/PeriodWiseChecksheet.cshtml", model);

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
