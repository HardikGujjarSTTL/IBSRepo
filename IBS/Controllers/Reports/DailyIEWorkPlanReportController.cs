using IBS.Helper;
using IBS.Interfaces.Reports;
using IBS.Models.Reports;
using IBS.Repositories.Reports;
using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp.Media;
using PuppeteerSharp;
using System.Collections.Generic;

namespace IBS.Controllers.Reports
{
    public class DailyIEWorkPlanReportController : BaseController
    {
        #region Variables
        private readonly IDailyIEWorkPlanReportRepository dailyIEWorkPlanReportRepository;
        private readonly IWebHostEnvironment env;
        #endregion
        public DailyIEWorkPlanReportController(IDailyIEWorkPlanReportRepository _dailyIEWorkPlanReportRepository, IWebHostEnvironment _env)
        {
            dailyIEWorkPlanReportRepository = _dailyIEWorkPlanReportRepository;
            this.env = _env;
        }
        public IActionResult Index()
        {
            string Region = SessionHelper.UserModelDTO.Region;
            ViewBag.Regions = Region;
            return View();
        }

        public IActionResult Manage(string FromDate,string ToDate,string lstIE,string lstCM,string AllIEs,string ParticularIEs,string AllCM,string ParticularCMs,string ReportType,string IEWise,string CMWise,string SortedIE,string visitdate )
        {
            DailyIECMWorkPlanReportModel model = new() { 
                ReportType = ReportType,
                FromDate = FromDate, 
                ToDate = ToDate,
                lstIE = lstIE,
                lstCM = lstCM,
                AllIEs = AllIEs,
                ParticularIEs = ParticularIEs,
                AllCM = AllCM,
                IEWise= IEWise,
                CMWise= CMWise,
                SortedIE= SortedIE,
                visitdate= visitdate,
                ParticularCMs = ParticularCMs,
            };
            if (ReportType == "U") model.ReportTitle = "Daily IE Work Plan Report";

            return View(model);
        }

        public IActionResult DailyWorkIECMReport(string FromDate, string ToDate, string lstIE, string lstCM, string AllIEs, string ParticularIEs, string AllCM, string ParticularCMs, string ReportType, string IEWise, string CMWise, string SortedIE, string visitdate)
        {
            string wRegion = "";
            if (Region == "N") { wRegion = "Northern Region"; }
            else if (Region == "S") { wRegion = "Southern Region"; }
            else if (Region == "E") { wRegion = "Eastern Region"; }
            else if (Region == "W") { wRegion = "Western Region"; }
            else if (Region == "C") { wRegion = "Central Region"; }
            ViewBag.Regions = wRegion;
            DailyIECMWorkPlanReportModel model = dailyIEWorkPlanReportRepository.GetDailyWorkData(FromDate, ToDate, lstIE, lstCM, AllIEs, ParticularIEs, AllCM, ParticularCMs, ReportType, IEWise, CMWise, Region,SortedIE, visitdate);
            GlobalDeclaration.DailyIECMWorkPlanReport = model;
            ViewBag.frmdt = FromDate;
            ViewBag.todt = ToDate;
            ViewBag.reporttypes = ReportType;
            return PartialView(model);
        }
        
        public IActionResult DailyWorkIEExcepReport(string FromDate, string ToDate, string lstIE, string lstCM, string AllIEs, string ParticularIEs, string AllCM, string ParticularCMs, string ReportType, string IEWise, string CMWise, string SortedIE, string visitdate)
        {
            string wRegion = "";
            if (Region == "N") { wRegion = "Northern Region"; }
            else if (Region == "S") { wRegion = "Southern Region"; }
            else if (Region == "E") { wRegion = "Eastern Region"; }
            else if (Region == "W") { wRegion = "Western Region"; }
            else if (Region == "C") { wRegion = "Central Region"; }
            ViewBag.Regions = wRegion;
            DailyIECMWorkPlanReportModel model = dailyIEWorkPlanReportRepository.GetDailyWorkData(FromDate, ToDate, lstIE, lstCM, AllIEs, ParticularIEs, AllCM, ParticularCMs, ReportType, IEWise, CMWise, Region,SortedIE, visitdate);
            GlobalDeclaration.DailyIECMWorkPlanReport = model;
            ViewBag.frmdt = FromDate;
            ViewBag.todt = ToDate;
            ViewBag.reporttypes = ReportType;
            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> GeneratePDF(string ReportType)
        {
            string htmlContent = string.Empty;

            if (ReportType == "U")
            {
                DailyIECMWorkPlanReportModel model = GlobalDeclaration.DailyIECMWorkPlanReport;
                htmlContent = await this.RenderViewToStringAsync("/Views/DailyIEWorkPlanReport/DailyWorkIECMReport.cshtml", model);
            }
            if (ReportType == "E")
            {
                DailyIECMWorkPlanReportModel model = GlobalDeclaration.DailyIECMWorkPlanReport;
                htmlContent = await this.RenderViewToStringAsync("/Views/DailyIEWorkPlanReport/DailyWorkIEExcepReport.cshtml", model);
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
