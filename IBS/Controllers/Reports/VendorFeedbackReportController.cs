using IBS.Helper;
using IBS.Models;
using IBS.Repositories.Reports;
using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp.Media;
using PuppeteerSharp;
using IBS.Interfaces.Reports;
using IBS.Models.Reports;

namespace IBS.Controllers.Reports
{
    public class VendorFeedbackReportController : BaseController
    {
        #region Variables
        private readonly IVendorFeedbackReportRepository vendorFeedbackReportRepository;
        private readonly IWebHostEnvironment env;
        #endregion
        public VendorFeedbackReportController(IVendorFeedbackReportRepository _vendorFeedbackReportRepository, IWebHostEnvironment _env)
        {
            vendorFeedbackReportRepository = _vendorFeedbackReportRepository;
            this.env = _env;
        }
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Manage()
        {
            return View();
        }

        public IActionResult VendorFeedback()
        {
            string Region = SessionHelper.UserModelDTO.Region;
            string wRegion = "";
            if (Region == "N") { wRegion = "Northern Region"; }
            else if (Region == "S") { wRegion = "Southern Region"; }
            else if (Region == "E") { wRegion = "Eastern Region"; }
            else if (Region == "W") { wRegion = "Western Region"; }
            else if (Region == "C") { wRegion = "Central Region"; }
            VendorFeedbackReportModel model = vendorFeedbackReportRepository.GetVendorFeedbackReport(Region);
            ViewBag.Regions = wRegion;
            GlobalDeclaration.VendorFeedbackReport = model;
            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> GeneratePDF()
        {
            string htmlContent = string.Empty;

            VendorFeedbackReportModel model = GlobalDeclaration.VendorFeedbackReport;
            htmlContent = await this.RenderViewToStringAsync("/Views/VendorFeedbackReport/VendorFeedback.cshtml", model);

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
