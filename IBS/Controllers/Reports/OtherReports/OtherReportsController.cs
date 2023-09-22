using IBS.Filters;
using IBS.Helper;
using IBS.Interfaces.Reports;
using IBS.Interfaces.Reports.OtherReports;
using IBS.Models.Reports;
using IBS.Repositories.Reports;
using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp.Media;
using PuppeteerSharp;

namespace IBS.Controllers.Reports.OtherReports
{
    [Authorization]
    public class OtherReportsController : BaseController
    {
        private readonly IOtherReportsRepository otherReportsRepository;
        private readonly IWebHostEnvironment env;

        public OtherReportsController(IOtherReportsRepository _otherReportsRepository, IWebHostEnvironment _env)
        {
            otherReportsRepository = _otherReportsRepository;
            this.env = _env;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Manage(string ReportType)
        {
            OtherReportsModel model = new() { ReportType = ReportType};
            if (ReportType == "COWISEIE") model.ReportTitle = "Controlling Officer's Wise Inspection Engineers";
            return View(model);
        }

        public IActionResult CoWiseIE()
        {
            ControllingOfficerIEModel model = otherReportsRepository.GetControllingOfficerWiseIE(Region);
            GlobalDeclaration.ControllingOfficerIE = model;
            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> GeneratePDF(string ReportType)
        {
            string htmlContent = string.Empty;

            if (ReportType == "COWISEIE")
            {
                ControllingOfficerIEModel model = GlobalDeclaration.ControllingOfficerIE;
                htmlContent = await this.RenderViewToStringAsync("/Views/OtherReports/CoWiseIE.cshtml", model);
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
