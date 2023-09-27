using IBS.Helper;
using IBS.Interfaces.Reports;
using IBS.Models.Reports;
using IBS.Repositories.Reports;
using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp.Media;
using PuppeteerSharp;

namespace IBS.Controllers.Reports
{
    public class PeriodWiseTechnicalReportController : BaseController
    {
        #region Variables
        private readonly IPeriodWiseTechnicalReportRepository periodWiseTechnicalReportRepository;
        private readonly IWebHostEnvironment env;
        #endregion
        public PeriodWiseTechnicalReportController(IPeriodWiseTechnicalReportRepository _periodWiseTechnicalReportRepository, IWebHostEnvironment _env)
        {
            periodWiseTechnicalReportRepository = _periodWiseTechnicalReportRepository;
            this.env = _env;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Manage(string FromDate, string ToDate)
        {
            PeriodWiseTechnicalRefReportModel model = new()
            {
                FromDate = FromDate,
                ToDate = ToDate,
            };
            model.ReportTitle = "period wise Technical";
            return View(model);
        }

        public IActionResult PeriodWiseTechRef(string FromDate, string ToDate)
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
            PeriodWiseTechnicalRefReportModel model = periodWiseTechnicalReportRepository.Getperiodwisetechrefdetails(FromDate, ToDate, Region);
            GlobalDeclaration.PeriodWiseTechnicalRefReport = model;
            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> GeneratePDF()
        {
            string htmlContent = string.Empty;

            PeriodWiseTechnicalRefReportModel model = GlobalDeclaration.PeriodWiseTechnicalRefReport;
            htmlContent = await this.RenderViewToStringAsync("/Views/PeriodWiseTechnicalReport/PeriodWiseTechRef.cshtml", model);

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
