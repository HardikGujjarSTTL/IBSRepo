using IBS.Filters;
using IBS.Helper;
using IBS.Interfaces.Reports;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp.Media;
using PuppeteerSharp;

namespace IBS.Controllers.Reports
{
    [Authorization]
    public class InspectionBillingDelayController : BaseController
    {
        #region Variables
        private readonly IInspectionBillingDelayRepository inspectionBillingDelayRepository;
        private readonly IWebHostEnvironment env;
        #endregion
        public InspectionBillingDelayController(IInspectionBillingDelayRepository _inspectionBillingDelayRepository, IWebHostEnvironment _environment)
        {
            inspectionBillingDelayRepository = _inspectionBillingDelayRepository;
            env = _environment;
        }
        [Authorization("InspectionBillingDelay", "Index", "view")]
        public IActionResult Index()
        {
            string Region = SessionHelper.UserModelDTO.Region;
            var Action = Convert.ToString(Request.Query["Action"].FirstOrDefault());
            string wRegion = "";
            if (Region == "N") { wRegion = "RITES LTD. (NORTHERN REGION)"; }
            else if (Region == "S") { wRegion = "RITES LTD. (SOUTHERN REGION)"; }
            else if (Region == "E") { wRegion = "RITES LTD. (EASTERN REGION)"; }
            else if (Region == "W") { wRegion = "RITES LTD. (WESTERN REGION)"; }
            else if (Region == "C") { wRegion = "RITES LTD. (CENTRAL REGION)"; }
            ViewBag.RegionCode = GetUserInfo.Region;
            ViewBag.Region = wRegion;
            ViewBag.Action = Action;
            return View();
        }


        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)//InspectionBillingDelayFilterModel model)
        {
            string data = null;// inspectionBillingDelayRepository.Get_Inspection_Billing_Delay(dtParameters, GetUserInfo);
            return Json(data);
        }
        public IActionResult Report(bool MonthWise, bool DateWise, string Month, string Year, string FromDate, string ToDate, bool BillDate, bool IEName, bool IcDt, bool FInspDt, bool LFnspDt, bool AllIE, bool PartiIE, string IECD, string ReportTitle)
        {
            InspectionBillingDelayReportModel model = new()
            {
                MonthWise = MonthWise,
                DateWise = DateWise,
                Month = Month,
                Year = Year,
                FromDate = FromDate,
                ToDate = ToDate,
                IsBillDate = BillDate,
                IsIEName = IEName,
                IsIcDt = IcDt,
                IsFInspDt = FInspDt,
                IsLFnspDt = LFnspDt,
                IsAllIE = AllIE,
                IsPartiIE = PartiIE,
                IECD = IECD,
                ReportTitle = ReportTitle
            };
            return View(model);
        }

        public IActionResult InspectionBillingDelay(bool MonthWise, bool DateWise, string Month, string Year, string FromDate, string ToDate, bool BillDate, bool IEName, bool IcDt, bool FInspDt, bool LFnspDt, bool AllIE, bool PartiIE, string IECD, string ReportTitle)
        {
            string wRegion = "";
            string Region = SessionHelper.UserModelDTO.Region;
            if (Region == "N") { wRegion = "RITES LTD. (NORTHERN REGION)"; }
            else if (Region == "S") { wRegion = "RITES LTD. (SOUTHERN REGION)"; }
            else if (Region == "E") { wRegion = "RITES LTD. (EASTERN REGION)"; }
            else if (Region == "W") { wRegion = "RITES LTD. (WESTERN REGION)"; }
            else if (Region == "C") { wRegion = "RITES LTD. (CENTRAL REGION)"; }
            ViewBag.RegionCode = GetUserInfo.Region;
            ViewBag.Region = wRegion;
            ViewBag.ReportTitle = ReportTitle;
            InspectionBillingDelayReportModel obj = new()
            {
                MonthWise = MonthWise,
                DateWise = DateWise,
                Month = Month,
                Year = Year,
                FromDate = FromDate,
                ToDate = ToDate,
                IsBillDate = BillDate,
                IsIEName = IEName,
                IsIcDt = IcDt,
                IsFInspDt = FInspDt,
                IsLFnspDt = LFnspDt,
                IsAllIE = AllIE,
                IsPartiIE = PartiIE,
                IECD = IECD,
                ReportTitle = ReportTitle,
                Region = wRegion
            };
            obj.InspBillDelayList = inspectionBillingDelayRepository.Get_Inspection_Billing_Delay(obj, GetUserInfo);
            GlobalDeclaration.InspBillDelayList = obj;
            return PartialView(obj);
        }

        [HttpPost]
        public async Task<IActionResult> GeneratePDF()
        {
            string htmlContent = string.Empty;


            InspectionBillingDelayReportModel model = GlobalDeclaration.InspBillDelayList;
            htmlContent = await this.RenderViewToStringAsync("/Views/InspectionBillingDelay/InspectionBillingDelay.cshtml", model);


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
