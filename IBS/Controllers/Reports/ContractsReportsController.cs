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
using IBS.Repositories.Reports;

namespace IBS.Controllers.Reports
{
    public class ContractsReportsController : BaseController
    {
        #region Variables
        private readonly IContractsReportsRepository contractsReportsRepository;
        private readonly IWebHostEnvironment env;
        #endregion
        public ContractsReportsController(IContractsReportsRepository _contractsReportsRepository, IWebHostEnvironment _env)
        {
            contractsReportsRepository = _contractsReportsRepository;
            this.env = _env;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Manage(string FromDate, string ToDate, string Region, string clientname)
        {
            ContractReportModel model = new()
            {
                FromDate = FromDate,
                ToDate = ToDate,
                Region = Region,
                clientname = clientname
            };
            model.ReportTitle = "Contracts";
            return View(model);
        }

        public IActionResult ContractReport(string FromDate, string ToDate, string Region, string clientname)
        {
            ContractReportModel model = contractsReportsRepository.GetContractDetails(FromDate, ToDate, Region, clientname);
            GlobalDeclaration.ContractReport = model;
            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> GeneratePDF()
        {
            string htmlContent = string.Empty;

            ContractReportModel model = GlobalDeclaration.ContractReport;
            htmlContent = await this.RenderViewToStringAsync("/Views/ContractsReports/ContractReport.cshtml", model);

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
