using IBS.Filters;
using IBS.Helper;
using IBS.Interfaces.Reports;
using IBS.Models;
using IBS.Models.Reports;
using IBS.Repositories.Reports;
using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp.Media;
using PuppeteerSharp;
using IBS.Interfaces;

namespace IBS.Controllers.Reports
{
    public class NCRCWiseReportController : BaseController
    {
        private readonly INCRCWiseReportRepository iNCRCWiseReportRepository;
        private readonly IWebHostEnvironment env;
        public NCRCWiseReportController(INCRCWiseReportRepository _iNCRCWiseReportRepository, IWebHostEnvironment _env)
        {
            iNCRCWiseReportRepository = _iNCRCWiseReportRepository;
            this.env = _env;
        }
        [Authorization("NCRCWiseReport", "Index", "view")]
        public IActionResult Index()
        {
            string Region = SessionHelper.UserModelDTO.Region;
            string wRegion = "";
            if (Region == "N") { wRegion = "Northern Region"; }
            else if (Region == "S") { wRegion = "Southern Region"; }
            else if (Region == "E") { wRegion = "Eastern Region"; }
            else if (Region == "W") { wRegion = "Western Region"; }
            else if (Region == "C") { wRegion = "Central Region"; }
            ViewBag.Regions = Region;
            return View();
        }

        public IActionResult Manage(string month,string year,string FromDate,string ToDate,string AllCM,string forCM,string All,string Outstanding,string formonth,string forperiod,string monthChar,string iecmname, string reporttype,string COName)
        {
            NCRReport model = new()
            {
                month = month,
                year = year,
                FromDate = FromDate,
                ToDate = ToDate,
                AllCM = AllCM,
                forCM = forCM,
                All = All,
                Outstanding = Outstanding,
                formonth = formonth,
                monthChar = monthChar,
                iecmname = iecmname,
                reporttype = reporttype,
                forperiod = forperiod,
                COName= COName
            };
            model.ReportTitle = "NCR Report Controling Wise";
            return View(model);
        }

        public IActionResult NCRCWiseReport(string month, string year, string FromDate, string ToDate, string AllCM, string forCM, string All, string Outstanding, string formonth, string forperiod, string monthChar, string iecmname, string reporttype, string COName, string IENametext)
        {
            string Region = SessionHelper.UserModelDTO.Region;
            string wRegion = "";
            if (Region == "N") { wRegion = "Northern Region"; }
            else if (Region == "S") { wRegion = "Southern Region"; }
            else if (Region == "E") { wRegion = "Eastern Region"; }
            else if (Region == "W") { wRegion = "Western Region"; }
            else if (Region == "C") { wRegion = "Central Region"; }
            NCRReport model = iNCRCWiseReportRepository.GetNCRIECOWiseData(month, year, FromDate, ToDate, AllCM, forCM, All, Outstanding, formonth, forperiod, Region, iecmname, reporttype);
            ViewBag.Regions = wRegion;
            ViewBag.FromDT = FromDate;
            ViewBag.AllCMData = AllCM;
            ViewBag.ToDT = ToDate;
            ViewBag.yearshow = year;
            ViewBag.monthshow = monthChar;
            ViewBag.CONameview = COName;
            ViewBag.Todaydt = DateTime.Now.ToString("dd/MM/yyyyy"); 
            GlobalDeclaration.NCRReports = model;
            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> GeneratePDF()
        {
            string htmlContent = string.Empty;
            NCRReport model = GlobalDeclaration.NCRReports;
            htmlContent = await this.RenderViewToStringAsync("/Views/NCRCWiseReport/NCRCWiseReport.cshtml", model);

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
