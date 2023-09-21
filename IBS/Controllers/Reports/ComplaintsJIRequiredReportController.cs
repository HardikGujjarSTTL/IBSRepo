using IBS.Filters;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using IBS.Models.Reports;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace IBS.Controllers
{
    public class ComplaintsJIRequiredReportController : BaseController
    {
        #region Variables
        private readonly IComplaintsJIRequiredReportRepository complaintsJIRequiredReportRepository;
        private readonly IWebHostEnvironment env;
        #endregion

        public ComplaintsJIRequiredReportController(IComplaintsJIRequiredReportRepository _complaintsJIRequiredReportRepository, IWebHostEnvironment _env)
        {
            complaintsJIRequiredReportRepository = _complaintsJIRequiredReportRepository;
            this.env = _env;
        }
        [Authorization("ComplaintsJIRequiredReport", "Index", "view")]
        public IActionResult Index()
        {
            string Region = SessionHelper.UserModelDTO.Region;
            string wRegion = "";
            if (Region == "N") { wRegion = "Northern Region"; }
            else if (Region == "S") { wRegion = "Southern Region"; }
            else if (Region == "E") { wRegion = "Eastern Region"; }
            else if (Region == "W") { wRegion = "Western Region"; }
            else if (Region == "C") { wRegion = "Central Region"; }
            ViewBag.Regions = Region;
            return View();
        }

        public ActionResult GetClientType(string Clientwise)
        {
            var json = "";
            try
            {
                json = complaintsJIRequiredReportRepository.GetItems(Clientwise);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ComplaintsJIRequiredReport", "GetClientType", 1, GetIPAddress());
            }
            return Json(json);
        }

        public IActionResult Manage(string FromDate, string ToDate,string AllCM, string AllIEs, string AllVendors, string AllClient, string AllConsignee, string Compact, string AwaitingJI, string JIConclusion, string JIConclusionfollowup,
            string JIconclusionreport, string JIDecidedDT, string All, string ParticularIEs, string IEWise, string CMWise, string VendorWise, string ClientWise, string ConsigneeWise, string FinancialYear, string ParticularCMs, string ParticularClients, string ParticularConsignee,
            string ParticularVendor, string Detailed, string FinancialYears, string ddlsupercm, string ddliename, string Clientwiseddl, string vendor, string Item, string consignee,string FinancialYearsvalue)
        {
            JIRequiredReport model = new() { FromDate = FromDate, ToDate = ToDate, AllCM = AllCM, AllIEs = AllIEs, AllVendors = AllVendors, AllClient = AllClient, AllConsignee = AllConsignee, Compact = Compact, AwaitingJI = AwaitingJI, JIConclusion = JIConclusion,
                JIConclusionfollowup = JIConclusionfollowup,JIconclusionreport = JIconclusionreport,JIDecidedDT = JIDecidedDT,All = All,ParticularIEs = ParticularIEs,IEWise = IEWise,CMWise = CMWise,
                VendorWise = VendorWise,
                ClientWise = ClientWise,
                ConsigneeWise = ConsigneeWise,
                FinancialYear = FinancialYear,
                ParticularCMs = ParticularCMs,
                ParticularClients = ParticularClients,
                ParticularConsignee = ParticularConsignee,
                ParticularVendor = ParticularVendor,
                Detailed = Detailed,
                FinancialYears = FinancialYears,
                ddlsupercm = ddlsupercm,
                ddliename = ddliename,
                Clientwiseddl = Clientwiseddl,
                vendor = vendor,
                Item = Item,
                consignee = consignee,
                FinancialYearsValue = FinancialYearsvalue
            };
            model.ReportTitle = "Complaint Recieved";
            return View(model);
        }

        public IActionResult ComplaintRecieved(string FromDate, string ToDate, string AllCM, string AllIEs, string AllVendors, string AllClient, string AllConsignee, string Compact, string AwaitingJI, string JIConclusion, string JIConclusionfollowup,
            string JIconclusionreport, string JIDecidedDT, string All, string ParticularIEs, string IEWise, string CMWise, string VendorWise, string ClientWise, string ConsigneeWise, string FinancialYear, string ParticularCMs, string ParticularClients, string ParticularConsignee,
            string ParticularVendor, string Detailed, string FinancialYears, string ddlsupercm, string ddliename, string Clientwiseddl, string vendor, string Item, string consignee,string FinancialYearsvalue)
        {
            string Region = SessionHelper.UserModelDTO.Region;
            string wRegion = "";
            if (Region == "N") { wRegion = "Northern Region"; }
            else if (Region == "S") { wRegion = "Southern Region"; }
            else if (Region == "E") { wRegion = "Eastern Region"; }
            else if (Region == "W") { wRegion = "Western Region"; }
            else if (Region == "C") { wRegion = "Central Region"; }
            JIRequiredReport model = complaintsJIRequiredReportRepository.GetJIRequiredList( FromDate,  ToDate, AllCM, AllIEs, AllVendors, AllClient, AllConsignee, Compact, AwaitingJI, JIConclusion, JIConclusionfollowup,
            JIconclusionreport, JIDecidedDT, All, ParticularIEs, IEWise,  CMWise, VendorWise, ClientWise, ConsigneeWise, FinancialYear, ParticularCMs, ParticularClients, ParticularConsignee,
            ParticularVendor, Detailed, FinancialYears, ddlsupercm, ddliename, Clientwiseddl, vendor, Item, consignee, Region, FinancialYearsvalue);
            ViewBag.FromDT = FromDate;
            ViewBag.ToDT = ToDate;
            ViewBag.Financialperiod = FinancialYears;
            ViewBag.Regions = wRegion;
            ViewBag.Detailedchk = Detailed;
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
