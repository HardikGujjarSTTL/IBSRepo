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
    public class IEAlterReportController : BaseController
    {
        #region Variables
        private readonly IIEAlterReportRepository iEAlterReportRepository;
        private readonly IWebHostEnvironment env;
        #endregion
        public IEAlterReportController(IIEAlterReportRepository _iEAlterReportRepository, IWebHostEnvironment _env)
        {
            iEAlterReportRepository = _iEAlterReportRepository;
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

        public IActionResult IEAlterMappingReport()
        {
            string Region = SessionHelper.UserModelDTO.Region;
            string wRegion = "";
            if (Region == "N") { wRegion = "Northern Region"; }
            else if (Region == "S") { wRegion = "Southern Region"; }
            else if (Region == "E") { wRegion = "Eastern Region"; }
            else if (Region == "W") { wRegion = "Western Region"; }
            else if (Region == "C") { wRegion = "Central Region"; }
            IEAlterMappingReportModel model = iEAlterReportRepository.GetIEAlterMappingReport( Region);
            GlobalDeclaration.IEAlterMappingReport = model;
            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> GeneratePDF()
        {
            string htmlContent = string.Empty;

            IEAlterMappingReportModel model = GlobalDeclaration.IEAlterMappingReport;
            htmlContent = await this.RenderViewToStringAsync("/Views/IEAlterReport/IEAlterMappingReport.cshtml", model);

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
