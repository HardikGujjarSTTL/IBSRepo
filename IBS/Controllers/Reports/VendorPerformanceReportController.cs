using IBS.Helper;
using IBS.Interfaces.Reports;
using IBS.Models;
using IBS.Models.Reports;
using IBS.Repositories.Reports;
using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp.Media;
using PuppeteerSharp;

namespace IBS.Controllers.Reports
{
    public class VendorPerformanceReportController : BaseController
    {
        #region Variables
        private readonly IVendorPerformanceReportRepository vendorPerformanceReportRepository;
        private readonly IWebHostEnvironment env;
        #endregion
        public VendorPerformanceReportController(IVendorPerformanceReportRepository _vendorPerformanceReportRepository, IWebHostEnvironment _env)
        {
            vendorPerformanceReportRepository = _vendorPerformanceReportRepository;
            this.env = _env;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Manage(string FromDate, string ToDate, string formonth, string forperiod, string month, string year, string vendcd,string vendor,string monthtxt)
        {
            VendorPerformanceReportModel model = new()
            {
                FromDate = FromDate,
                ToDate = ToDate,
                formonth = formonth,
                forperiod = forperiod,
                month = month,
                year = year,
                vendcd = vendcd,
                vendor= vendor,
                monthtxt= monthtxt
            };
            model.ReportTitle = "Vendor Performance";
            return View(model);
        }

        public IActionResult VendorPerforeport(string FromDate, string ToDate, string formonth, string forperiod, string month, string year, string vendcd,string vendor,string monthtxt)
        {
            string Region = SessionHelper.UserModelDTO.Region;
            string wRegion = "";
            if (Region == "N") { wRegion = "Northern Region"; }
            else if (Region == "S") { wRegion = "Southern Region"; }
            else if (Region == "E") { wRegion = "Eastern Region"; }
            else if (Region == "W") { wRegion = "Western Region"; }
            else if (Region == "C") { wRegion = "Central Region"; }
            VendorPerformanceReportModel model = vendorPerformanceReportRepository.GetVendorperformanceReport(FromDate, ToDate, formonth, forperiod, month, year, vendcd, Region);
            GlobalDeclaration.VendorPerformanceReport = model;
            ViewBag.vendortext = vendor;
            ViewBag.frmdt = FromDate;
            ViewBag.todt = ToDate;
            ViewBag.Region = wRegion;
            ViewBag.monthtxtshow = monthtxt;
            ViewBag.yearshow = year;
            ViewBag.todaydate = DateTime.Now.ToString("dd-MM-yyyy");
            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> GeneratePDF()
        {
            string htmlContent = string.Empty;

            VendorPerformanceReportModel model = GlobalDeclaration.VendorPerformanceReport;
            htmlContent = await this.RenderViewToStringAsync("/Views/VendorPerformanceReport/VendorPerforeport.cshtml", model);

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
