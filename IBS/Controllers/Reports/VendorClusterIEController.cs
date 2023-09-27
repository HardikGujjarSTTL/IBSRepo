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

namespace IBS.Controllers.Reports
{
    public class VendorClusterIEController : BaseController
    {
        #region Variables
        private readonly IVendorClusterIERepository vendorClusterIERepository;
        private readonly IWebHostEnvironment env;
        #endregion
        public VendorClusterIEController(IVendorClusterIERepository _vendorClusterIERepository, IWebHostEnvironment _env)
        {
            vendorClusterIERepository = _vendorClusterIERepository;
            this.env = _env;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Manage(string department,string allreport,string departreport)
        {
            VendorClusterReportModel model = new()
            {
                department = department,
                allreport = allreport,
                departreport = departreport
            };
            model.ReportTitle = "Vendor, Cluster And IE Mapping";
            return View(model);
        }

        public IActionResult VendorClusterIEReport(string department)
        {
            string Region = SessionHelper.UserModelDTO.Region;
            string wRegion = "";
            if (Region == "N") { wRegion = "Northern Region"; }
            else if (Region == "S") { wRegion = "Southern Region"; }
            else if (Region == "E") { wRegion = "Eastern Region"; }
            else if (Region == "W") { wRegion = "Western Region"; }
            else if (Region == "C") { wRegion = "Central Region"; }
            VendorClusterReportModel model = vendorClusterIERepository.GetVendorClusterReport(department, Region);
            GlobalDeclaration.VendorClusterReport = model;
            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> GeneratePDF()
        {
            string htmlContent = string.Empty;

            VendorClusterReportModel model = GlobalDeclaration.VendorClusterReport;
            htmlContent = await this.RenderViewToStringAsync("/Views/VendorClusterIE/VendorClusterIEReport.cshtml", model);

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
