using IBS.Filters;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using IBS.Models.Reports;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp.Media;
using PuppeteerSharp;

namespace IBS.Controllers.Reports
{
    public class HighValueInspecReportController : BaseController
    {
        #region Variables
        private readonly IHighValueInspecReportRepository highValueInspecReportRepository;
        private readonly IWebHostEnvironment env;
        #endregion
        public HighValueInspecReportController(IHighValueInspecReportRepository _highValueInspecReportRepository, IWebHostEnvironment _env)
        {
            highValueInspecReportRepository = _highValueInspecReportRepository;
            this.env = _env;
        }
        [Authorization("HighValueInspecReport", "Index", "view")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Manage(string month,string year,string valinsp,string FromDate,string ToDate,string ICDate,string BillDate,string formonth,string forperiod)
        {
            HighValueInspReport model = new()
            {
                month = month,
                year = year,
                valinsp = valinsp,
                FromDate = FromDate,
                ToDate = ToDate,
                ICDate = ICDate,
                BillDate = BillDate,
                formonth = formonth,
                forperiod = forperiod
            };
            model.ReportTitle = "High Value Inspection";
            return View(model);
        }

        public IActionResult TopNHighValueInsp(string month, string year, string valinsp, string FromDate, string ToDate, string ICDate, string BillDate, string formonth, string forperiod)
        {
            string Region = SessionHelper.UserModelDTO.Region;
            string wRegion = "";
            if (Region == "N") { wRegion = "Northern Region"; }
            else if (Region == "S") { wRegion = "Southern Region"; }
            else if (Region == "E") { wRegion = "Eastern Region"; }
            else if (Region == "W") { wRegion = "Western Region"; }
            else if (Region == "C") { wRegion = "Central Region"; }
            HighValueInspReport model = highValueInspecReportRepository.GetHighValueInspdata(month, year, valinsp, FromDate, ToDate, ICDate, BillDate, formonth, forperiod, Region);
            ViewBag.Regions = wRegion;
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
