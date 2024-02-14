using IBS.Filters;
using IBS.Helper;
using IBS.Interfaces.Reports;
using IBS.Models;
using IBS.Models.Reports;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace IBS.Controllers.Reports
{
    [Authorization]
    public class RemitanceReportsController : BaseController
    {

        private readonly IRemitanceReportsRepository remitanceReportsRepository;
        private readonly IWebHostEnvironment env;

        public RemitanceReportsController(IRemitanceReportsRepository _remitanceReportsRepository, IWebHostEnvironment _env)
        {
            remitanceReportsRepository = _remitanceReportsRepository;
            this.env = _env;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Manage(string ReportType, DateTime FromDate, DateTime ToDate, string AccCode, string RReport, string BPOName, string ClientType, string ClientName)
        {
            RemitanceReportsModel model = new() { ReportType = ReportType, FromDate = FromDate, ToDate = ToDate, AccCode = AccCode, RReport = RReport, BPOName = BPOName, ClientType = ClientType, ClientName = ClientName };
            if (ReportType == "R")
            {
                model.ReportTitle = "Remitance Reports";
            }

            return View(model);
        }

        public IActionResult RemitanceReport(DateTime FromDate, DateTime ToDate, string AccCode, string RReport, string BPOName, string ClientType, string ClientName)
        {
            RemitanceModel model = remitanceReportsRepository.GetRemitanceReport(FromDate, ToDate, AccCode, Region, RReport, BPOName, ClientType, ClientName);
            GlobalDeclaration.Remitance = model;
            return PartialView(model);
        }

        public IActionResult BillWiseRemittancesPeriodReport(DateTime FromDate, DateTime ToDate, string AccCode, string RReport, string BPOName, string ClientType, string ClientName)
        {
            RemitanceModel model = remitanceReportsRepository.GetRemitanceReport(FromDate, ToDate, AccCode, Region, RReport, BPOName, ClientType, ClientName);
            GlobalDeclaration.Remitance = model;
            return PartialView(model);
        }

        public IActionResult BillWiseRemittancesCreatedBillReport(DateTime FromDate, DateTime ToDate, string AccCode, string RReport, string BPOName, string ClientType, string ClientName)
        {
            RemitanceModel model = remitanceReportsRepository.GetRemitanceReport(FromDate, ToDate, AccCode, Region, RReport, BPOName, ClientType, ClientName);
            GlobalDeclaration.Remitance = model;
            return PartialView(model);
        }

        public IActionResult ChequeWiseBillRemittanceReport(DateTime FromDate, DateTime ToDate, string AccCode, string RReport, string BPOName, string ClientType, string ClientName)
        {
            RemitanceModel model = remitanceReportsRepository.GetRemitanceReport(FromDate, ToDate, AccCode, Region, RReport, BPOName, ClientType, ClientName);
            GlobalDeclaration.Remitance = model;
            return PartialView(model);
        }

        public IActionResult AccountCodeWiseRemittanceReport(DateTime FromDate, DateTime ToDate, string AccCode, string RReport, string BPOName, string ClientType, string ClientName)
        {
            RemitanceModel model = remitanceReportsRepository.GetRemitanceReport(FromDate, ToDate, AccCode, Region, RReport, BPOName, ClientType, ClientName);
            GlobalDeclaration.Remitance = model;
            return PartialView(model);
        }

        public IActionResult ClientBPOWiseRemittanceReport(DateTime FromDate, DateTime ToDate, string AccCode, string RReport, string BPOName, string ClientType, string ClientName)
        {
            RemitanceModel model = remitanceReportsRepository.GetRemitanceReport(FromDate, ToDate, AccCode, Region, RReport, BPOName, ClientType, ClientName);
            GlobalDeclaration.Remitance = model;
            return PartialView(model);
        }

        public IActionResult StatementExcessRemittanceReport(DateTime FromDate, DateTime ToDate, string AccCode, string RReport, string BPOName, string ClientType, string ClientName)
        {
            RemitanceModel model = remitanceReportsRepository.GetRemitanceReport(FromDate, ToDate, AccCode, Region, RReport, BPOName, ClientType, ClientName);
            GlobalDeclaration.Remitance = model;
            return PartialView(model);
        }

        #region Other Event
        [HttpGet]
        public IActionResult GetBPORlyCd(string ClientType)
        {
            try
            {
                List<SelectListItem> lst = Common.GetBPORlyCd(ClientType);
                return Json(new { status = true, list = lst });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "RemitanceReports", "GetBPORlyCd", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpGet]
        public IActionResult GetlstBPOType(string ClientType, string ClientName)
        {
            try
            {
                List<SelectListItem> lst = Common.GetlstBPOType(ClientType, ClientName);
                return Json(new { status = true, list = lst });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "RemitanceReports", "GetlstBPOType", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
        #endregion

        #region GeneratePDF
        [HttpPost]
        public async Task<IActionResult> GeneratePDF(string ReportType)
        {
            string htmlContent = string.Empty;            
            if (ReportType == "Report1")
            {
                RemitanceModel model = GlobalDeclaration.Remitance;
                htmlContent = await this.RenderViewToStringAsync("/Views/RemitanceReports/RemitanceReport.cshtml", model);
            }
            else if (ReportType == "Report2")
            {
                RemitanceModel model = GlobalDeclaration.Remitance;
                htmlContent = await this.RenderViewToStringAsync("/Views/RemitanceReports/BillWiseRemittancesPeriodReport.cshtml", model);
            }
            else if (ReportType == "Report3")
            {
                RemitanceModel model = GlobalDeclaration.Remitance;
                htmlContent = await this.RenderViewToStringAsync("/Views/RemitanceReports/BillWiseRemittancesCreatedBillReport.cshtml", model);
            }
            else if (ReportType == "Report4")
            {
                RemitanceModel model = GlobalDeclaration.Remitance;
                htmlContent = await this.RenderViewToStringAsync("/Views/RemitanceReports/ChequeWiseBillRemittanceReport.cshtml", model);
            }
            else if (ReportType == "Report5")
            {
                RemitanceModel model = GlobalDeclaration.Remitance;
                htmlContent = await this.RenderViewToStringAsync("/Views/RemitanceReports/AccountCodeWiseRemittanceReport.cshtml", model);
            }
            else if (ReportType == "Report6")
            {
                RemitanceModel model = GlobalDeclaration.Remitance;
                htmlContent = await this.RenderViewToStringAsync("/Views/RemitanceReports/ClientBPOWiseRemittanceReport.cshtml", model);
            }
            else if (ReportType == "Report7")
            {
                RemitanceModel model = GlobalDeclaration.Remitance;
                htmlContent = await this.RenderViewToStringAsync("/Views/RemitanceReports/StatementExcessRemittanceReport.cshtml", model);
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
                Format = PaperFormat.A3,
                PrintBackground = true
            });

            await browser.CloseAsync();

            return File(pdfContent, "application/pdf", Guid.NewGuid().ToString() + ".pdf");
        }
        #endregion
    }
}
