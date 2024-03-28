using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace IBS.Controllers
{
    public class LabSamplePaymentReportController : BaseController
    {
        #region Variables
        private readonly ILabSamplePaymentRptRepository LabSamplePaymentRptRepository;
        private readonly IWebHostEnvironment env;
        #endregion
        public LabSamplePaymentReportController(ILabSamplePaymentRptRepository _LabSamplePaymentRptRepository, IWebHostEnvironment _env)
        {
            LabSamplePaymentRptRepository = _LabSamplePaymentRptRepository;
            this.env = _env;
        }
        public IActionResult Index()
        {
            ViewBag.Region = Region;
            return View();
        }
        public IActionResult Manage(string ReportType, string wFrmDtO, string wToDt, string Status, string rbsrdt)
        {

            LabSamplePaymentRptModel model = new()
            {
                ReportType = ReportType,
                wFrmDtO = wFrmDtO,
                wToDt = wToDt,
                ReportStatus = Status,
                call_recv_dt = rbsrdt
            };
            //if (ReportType == "LabReg") model.ReportTitle = "LAB SAMPLE INFO DETAILS";
            return View(model);
        }
        public IActionResult LabSamplePayment(string ReportType, string wFrmDtO, string wToDt, string Status, string rbsrdt)
        {
            ViewBag.From = wFrmDtO;
            ViewBag.To = wToDt;
            ViewBag.Date = DateTime.Now.ToString("dd/MM/yyyy");
            ViewBag.Time = DateTime.Now.ToShortTimeString();
            string Region = GetRegionCode;
            if (Region == "N")
            { ViewBag.Region = "NORTHERN REGION"; }
            else if (Region == "S")
            { ViewBag.Region = "SOUTHERN REGION"; }
            else if (Region == "E")
            { ViewBag.Region = "EASTERN REGION"; }
            else if (Region == "W")
            { ViewBag.Region = "WESTERN REGION"; }
            else if (Region == "C")
            { ViewBag.Region = "CENTRAL REGION"; }
            LabSamplePaymentRptModel dTResult = LabSamplePaymentRptRepository.GetPaymentReport(ReportType, wFrmDtO, wToDt, Status, rbsrdt, Region);
            return PartialView(dTResult);
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
