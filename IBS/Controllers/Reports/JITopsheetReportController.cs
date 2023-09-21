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

namespace IBS.Controllers
{
    public class JITopsheetReportController : BaseController
    {
        #region Variables
        private readonly IJITopsheetReportRepository jITopsheetReportRepository;
        private readonly IWebHostEnvironment env;
        #endregion
        public JITopsheetReportController(IJITopsheetReportRepository _jITopsheetReportRepository, IWebHostEnvironment _env)
        {
            jITopsheetReportRepository = _jITopsheetReportRepository;
            this.env = _env;
        }

        [Authorization("JITopsheetReport", "Index", "view")]
        public IActionResult Index()
        {
            string Region = SessionHelper.UserModelDTO.Region;
            string wRegion = "";
            if (Region == "N") { wRegion = "Northern Region"; }
            else if (Region == "S") { wRegion = "Southern Region"; }
            else if (Region == "E") { wRegion = "Eastern Region"; }
            else if (Region == "W") { wRegion = "Western Region"; }
            else if (Region == "C") { wRegion = "Central Region"; }
            ViewBag.Region = wRegion;
            return View();
        }

        public IActionResult Manage(string JISNO)
        {
            ConsigneeComplaints model = new()
            {
                JiSno = JISNO
            };
            model.ReportTitle = "JI Complaint Report";
            return View(model);
        }

        public IActionResult JIComplaintsReport(string JISNO)
        {
            string Region = SessionHelper.UserModelDTO.Region;
            string wRegion = "";
            if (Region == "N") { wRegion = "Northern Region"; }
            else if (Region == "S") { wRegion = "Southern Region"; }
            else if (Region == "E") { wRegion = "Eastern Region"; }
            else if (Region == "W") { wRegion = "Western Region"; }
            else if (Region == "C") { wRegion = "Central Region"; }
            ConsigneeComplaints model = jITopsheetReportRepository.GetComplaintReportDetails(JISNO, Region);
            ViewBag.Regions = wRegion;
            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> GeneratePDF()
        {
            string htmlContent = string.Empty;

            ConsigneeComplaints model = GlobalDeclaration.ConsigneeComplaint;
            htmlContent = await this.RenderViewToStringAsync("/Views/JITopsheetReport/JIComplaintsReport.cshtml", model);

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
