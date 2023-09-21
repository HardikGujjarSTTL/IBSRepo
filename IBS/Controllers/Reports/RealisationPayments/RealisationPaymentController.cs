using IBS.Filters;
using IBS.Interfaces.Reports;
using IBS.Interfaces;
using Microsoft.AspNetCore.Mvc;
using IBS.Interfaces.Reports.RealisationPayment;
using IBS.Models;
using IBS.Models.Reports;
using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace IBS.Controllers.Reports.RealisationPayments
{
    [Authorization]
    public class RealisationPaymentController : BaseController
    {
        #region Variables
        private readonly IRealisationPaymentRepository realisationPaymentRepository;
        private readonly IWebHostEnvironment env;
        private readonly IConfiguration _config;
        #endregion

        public RealisationPaymentController(IRealisationPaymentRepository _realisationPaymentRepository, IWebHostEnvironment _environment, IConfiguration configuration)
        {
            realisationPaymentRepository = _realisationPaymentRepository;
            env = _environment;
            _config = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Manage(string ReportType, DateTime FromDate, DateTime ToDate)
        {
            RealisationPaymentReportsModel model = new() { ReportType = ReportType, FromDate = FromDate, ToDate = ToDate };
            if (ReportType == "ONLINENRPAYMENTS") model.ReportTitle = "Summary Online Payment";
            return View(model);
        }

        public IActionResult ManageCrisRlyDetail(string ReportType, DateTime FromDate, DateTime ToDate, string IsDetailed, string IsRly, string Rly, string IsAU, string AU, string IsAllRegion, string Status)
        {
            RealisationPaymentReportsModel model = new() { ReportType = ReportType, FromDate = FromDate, ToDate = ToDate, IsDetailed = IsDetailed, IsRly = IsRly, Rly = Rly, IsAU = IsAU, AU = AU, IsAllRegion = IsAllRegion, Status = Status };
            return View("Manage", model);
        }

        public IActionResult ManageCrisRlySummary(string ReportType, DateTime FromDate, DateTime ToDate, string IsRlyWise, string Status)
        {
            RealisationPaymentReportsModel model = new() { ReportType = ReportType, FromDate = FromDate, ToDate = ToDate, IsRlyWise = IsRlyWise, Status = Status };
            return View("Manage", model);
        }

        public IActionResult SummaryOnlinePayment(DateTime FromDate, DateTime ToDate)
        {
            SummaryOnlinePaymentModel model = realisationPaymentRepository.GetSummaryOnlinePayment(FromDate, ToDate, Region);
            return PartialView(model);
        }

        public IActionResult SummaryCrisRlyPaymentDetail(DateTime FromDate, DateTime ToDate, string IsRly, string Rly, string IsAU, string AU, string IsAllRegion, string Status)
        {
            SummaryCrisRlyPaymentModel model = realisationPaymentRepository.GetSummaryCrisRlyPaymentDetailed(FromDate, ToDate, IsRly, Rly, IsAU, AU, IsAllRegion, Status, Region);
            return PartialView(model);
        }

        public IActionResult SummaryCrisRlyPaymentSummary(DateTime FromDate, DateTime ToDate, string IsRlyWise, string Status)
        {
            SummaryCrisRlyPaymentModel model = realisationPaymentRepository.GetSummaryCrisRlyPaymentSummary(FromDate, ToDate, IsRlyWise, Status, Region);
            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> GeneratePDF(string htmlContent)
        {
            //PendingICAgainstCallsModel _model = JsonConvert.DeserializeObject<PendingICAgainstCallsModel>(TempData[model.ReportType].ToString());
            //htmlContent = await this.RenderViewToStringAsync("/Views/ManagementReports/PendingICAgainstCalls.cshtml", _model);

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
