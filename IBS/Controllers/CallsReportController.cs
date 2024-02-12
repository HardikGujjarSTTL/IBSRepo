using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace IBS.Controllers
{
    public class CallsReportController : BaseController
    {

        private readonly ICallsReportRepository CallsReportRepository;
        private readonly IWebHostEnvironment env;
        public CallsReportController(ICallsReportRepository _CallsReportRepository, IWebHostEnvironment _env)
        {
            CallsReportRepository = _CallsReportRepository;
            this.env = _env;
        }
        public IActionResult Index()
        {
            ViewBag.Region = Region;
            return View();
        }

        [HttpPost]
        public IActionResult Dropdown(string selectedValue)
        {
            if (selectedValue == "R")
            {
                List<railway_dropdown1> result = CallsReportRepository.GetValue(selectedValue);
                return Json(result);
            }
            else
            {
                List<railway_dropdown1> result = CallsReportRepository.GetValue2(selectedValue);
                return Json(result);
            }

        }
        [HttpPost]
        public IActionResult gridData([FromBody] DTParameters dtParameters)
        {
            DTResult<Statement_IeVendorWiseModel> dTResult = CallsReportRepository.gridData(dtParameters);
            return Json(dTResult);
        }

        public IActionResult Manage(string ReportType, string frmDate, string toDate, string WiseRadio, string IeStatus, int Days, string includeNSIC, string pendingCallsOnly, string PO_NO, string PO_DT, string RLY_NONRLY, string RLY_CD, string wSortkEy)
        {
            if (ReportType != "IeVendorWise" && ReportType != "OverdueCalls" && ReportType != "ApprovalReport" && ReportType != "CallMarked")
            {
                ReportType = "SpecificPO";
            }

            Statement_IeVendorWiseModel model = new() { ReportType = ReportType, FromDate = frmDate, ToDate = toDate, WiseRadio = WiseRadio, IeStatus = IeStatus, Days = Days, includeNSIC = includeNSIC, pendingCallsOnly = pendingCallsOnly, PO_NO = PO_NO, PO_DT = Convert.ToDateTime(PO_DT), RLY_NONRLY = RLY_NONRLY, RLY_CD = RLY_CD, wSortkEy = wSortkEy };
            if (ReportType == "IeVendorWise") model.ReportTitle = "STATEMENT OF IE AND VENDOR WISE CALLS CANCELLED";
            else if (ReportType == "OverdueCalls") model.ReportTitle = "STATEMENT OF OVERDUE CALLS";
            else if (ReportType == "ApprovalReport") model.ReportTitle = "CALL CANCELLATION APPROVAL REPORT ";
            else if (ReportType == "SpecificPO") model.ReportTitle = "Call Detail For Specific PO";
            else if (ReportType == "CallMarked") model.ReportTitle = "Call Marked Period Wise";

            return View(model);
        }
        public IActionResult Statement_IeVendorWise(string ReportType, string frmDate, string toDate)
        {
            ViewBag.Frm = frmDate;
            ViewBag.To = toDate;

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


            Statement_IeVendorWiseModel model = CallsReportRepository.Statement_IeVendorWise(ReportType, frmDate, toDate, Region);

            return PartialView(model);
        }

        public IActionResult Statement_OverdueCalls(string ReportType, string WiseRadio, string IeStatus, int Days, string includeNSIC, string pendingCallsOnly)
        {
            ViewBag.OnDate = DateTime.Now;
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


            Statement_IeVendorWiseModel model = CallsReportRepository.Statement_OverdueCalls(ReportType, WiseRadio, IeStatus, Days, includeNSIC, pendingCallsOnly, Region);

            return PartialView(model);
        }

        public IActionResult Statement_ApprovalReport(string ReportType, string frmDate, string toDate, string WiseRadio, string IeStatus, int Days, string includeNSIC, string pendingCallsOnly)
        {
            ViewBag.Frm = frmDate;
            ViewBag.To = toDate;
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


            Statement_IeVendorWiseModel model = CallsReportRepository.Statement_ApprovalReport(ReportType, frmDate, toDate, Region);

            return PartialView(model);
        }

        public IActionResult Statement_SpecificPO(string ReportType, string PO_NO, string PO_DT, string RLY_NONRLY, string RLY_CD)
        {

            //string Region = GetRegionCode;
            //if (Region == "N")
            //{ ViewBag.Region = "NORTHERN REGION"; }
            //else if (Region == "S")
            //{ ViewBag.Region = "SOUTHERN REGION"; }
            //else if (Region == "E")
            //{ ViewBag.Region = "EASTERN REGION"; }
            //else if (Region == "W")
            //{ ViewBag.Region = "WESTERN REGION"; }
            //else if (Region == "C")
            //{ ViewBag.Region = "CENTRAL REGION"; }


            Statement_IeVendorWiseModel model = CallsReportRepository.Statement_SpecificPO(ReportType, PO_NO, PO_DT, RLY_NONRLY, RLY_CD);

            return PartialView(model);
        }
        public IActionResult Statement_CallMarked(string ReportType, string frmDate, string toDate, string wSortkEy)
        {

            ViewBag.Frm = frmDate;
            ViewBag.To = toDate;
            if (wSortkEy == "V")
            {
                ViewBag.Sort = "VENDOR";
            }
            else
            {
                ViewBag.Sort = "Call Date";
            }
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


            Statement_IeVendorWiseModel model = CallsReportRepository.Statement_CallMarked(ReportType, frmDate, toDate, wSortkEy, Region);

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
                Format = PaperFormat.A3,
                PrintBackground = true
            });

            await browser.CloseAsync();

            return File(pdfContent, "application/pdf", Guid.NewGuid().ToString() + ".pdf");
        }
    }
}
