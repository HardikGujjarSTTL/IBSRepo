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
    public class OngoingContractsReportController : BaseController
    {
        #region Variables
        private readonly IOngoingContractsReportRepository ongoingContractsReportRepository;
        private readonly IWebHostEnvironment env;
        #endregion
        public OngoingContractsReportController(IOngoingContractsReportRepository _ongoingContractsReportRepository, IWebHostEnvironment _env)
        {
            ongoingContractsReportRepository = _ongoingContractsReportRepository;
            this.env = _env;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Manage(string StatusOffer, string Region, string StatusOffertxt,string Regiontxt,string rdoregionwise)
        {
            OngoingContrcatsReportModel model = new()
            {
                StatusOffer = StatusOffer,
                Region = Region,
                StatusOffertxt = StatusOffertxt,
                Regiontxt = Regiontxt,
                rdoregionwise = rdoregionwise
            };
            model.ReportTitle = "Ongoing Contracts";
            return View(model);
        }

        public IActionResult OngoingContractReport(string StatusOffer, string Region, string StatusOffertxt, string Regiontxt, string rdoregionwise)
        {
            OngoingContrcatsReportModel model = ongoingContractsReportRepository.Getongoingcontractdetails(StatusOffer, Region, StatusOffertxt, Regiontxt, rdoregionwise);
            GlobalDeclaration.OngoingContrcatsReport = model;
            ViewBag.StatusOffer = StatusOffertxt;
            ViewBag.Region = Regiontxt;
            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> GeneratePDF()
        {
            string htmlContent = string.Empty;

            OngoingContrcatsReportModel model = GlobalDeclaration.OngoingContrcatsReport;
            htmlContent = await this.RenderViewToStringAsync("/Views/OngoingContractsReport/OngoingContractReport.cshtml", model);

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
