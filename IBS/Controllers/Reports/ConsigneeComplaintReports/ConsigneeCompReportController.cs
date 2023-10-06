using IBS.Filters;
using IBS.Helper;
using IBS.Models;
using IBS.Models.Reports;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp.Media;
using PuppeteerSharp;
using IBS.Interfaces;
using IBS.Interfaces.Reports.ConsigneeComplaintReports;
using static Microsoft.AspNetCore.Razor.Language.TagHelperMetadata;
using System.Numerics;
using NuGet.Protocol.Plugins;
using IBS.Repositories.Reports;
using IBS.Interfaces.Reports;

namespace IBS.Controllers.Reports.ConsigneeComplaintReports
{
    [Authorization]
    public class ConsigneeCompReportController : BaseController
    {
        private readonly IConsigneeCompReportRepository consigneeCompReportRepository;
        private readonly IWebHostEnvironment env;
        public ConsigneeCompReportController(IConsigneeCompReportRepository _consigneeCompReportRepository, IWebHostEnvironment _env)
        {
            consigneeCompReportRepository = _consigneeCompReportRepository;
            this.env = _env;
        }
        public IActionResult Index()
        {
            string Region = SessionHelper.UserModelDTO.Region;
            ViewBag.Region = Region;
            return View();
        }

        public ActionResult GetClientType(string Clientwise)
        {
            var json = "";
            try
            {
                json = consigneeCompReportRepository.GetItems(Clientwise);
            }
            catch (Exception ex)
            {
               // Common.AddException(ex.ToString(), ex.Message.ToString(), "ComplaintsJIRequiredReport", "GetClientType", 1, GetIPAddress());
            }
            return Json(json);
        }

        public IActionResult Manage(string ReportType, string FromDate, string ToDate, string Allregion, string regionorth, string regionsouth, string regioneast, string regionwest, string jiallregion,
            string jinorth, string jisourth, string jieast, string jiwest, string compallregion, string compyes, string compno, string cancelled, string underconsider, string allaction, string particilaraction, string actiondrp)
        {
            ConsigneeCompReports model = new() {
                ReportType = ReportType,
                FromDate = FromDate,
                ToDate = ToDate,
                Allregion = Allregion,
                regionorth = regionorth,
                regionsouth = regionsouth,
                regioneast = regioneast,
                regionwest = regionwest,
                jiallregion = jiallregion,
                jinorth = jinorth,
                jisourth = jisourth,
                jieast = jieast,
                jiwest = jiwest,
                compallregion = compallregion,
                compyes = compyes,
                compno = compno,
                cancelled = cancelled,
                underconsider = underconsider,
                allaction = allaction,
                particilaraction = particilaraction,
                actiondrp = actiondrp
            };
            if (ReportType == "U") model.ReportTitle = "Consignee Complaints For The Period";
            return View(model);
        }

        public IActionResult ManageByCompJI(string ReportType, string FromDate, string ToDate, string AllCM, string AllIEs, string AllVendors, string AllClient, string AllConsignee, string Compact, string AwaitingJI, string JIConclusion, string JIConclusionfollowup,
            string JIconclusionreport, string JIDecidedDT, string All, string ParticularIEs, string IEWise, string CMWise, string VendorWise, string ClientWise, string ConsigneeWise, string FinancialYear, string ParticularCMs, string ParticularClients, string ParticularConsignee,
            string ParticularVendor, string Detailed, string FinancialYears, string ddlsupercm, string ddliename, string Clientwiseddl, string vendor, string Item, string consignee, string FinancialYearsvalue)
        {
            ConsigneeCompReports model = new() { ReportType = ReportType ,
                FromDate = FromDate,
                ToDate = ToDate,
                AllCM = AllCM,
                AllIEs = AllIEs,
                AllVendors = AllVendors,
                AllClient = AllClient,
                AllConsignee = AllConsignee,
                Compact = Compact,
                AwaitingJI = AwaitingJI,
                JIConclusion = JIConclusion,
                JIConclusionfollowup = JIConclusionfollowup,
                JIconclusionreport = JIconclusionreport,
                JIDecidedDT = JIDecidedDT,
                All = All,
                ParticularIEs = ParticularIEs,
                IEWise = IEWise,
                CMWise = CMWise,
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
            if (ReportType == "CCOMPJI") model.ReportTitle = "Summary Of Consignee Complaints where JI Required";
            return View("Manage", model);
        }

        public IActionResult ManageByTopJI(string ReportType, string JISNO)
        {
            ConsigneeCompReports model = new() { ReportType = ReportType, JiSno = JISNO };
            if (ReportType == "TOPJI") model.ReportTitle = "JI Topsheet";
            return View("Manage", model);
        }
        
        public IActionResult ManageByDefectCode(string ReportType, DateTime FromDate, DateTime ToDate)
        {
            ConsigneeCompReports model = new() { ReportType = ReportType, FromDate = FromDate.ToString(), ToDate = ToDate.ToString() };
            if (ReportType == "DCWACOMPS") model.ReportTitle = "DEFECT CODE WISE ANALYSIS OF COMPLAINTS";
            return View("Manage", model);
        }
        
        public IActionResult ManageByCOCOMP(string ReportType, string FinancialYearsText, string FinancialYearsValue)
        {
            ConsigneeCompReports model = new() { ReportType = ReportType, FinancialYearsText = FinancialYearsText, FinancialYearsValue = FinancialYearsValue };
            if (ReportType == "COCOMPJI") model.ReportTitle = "Summarized Position Consignee Rejection (Region Wise)";
            return View("Manage", model);
        }
        
        public IActionResult ManageByTopN(string ReportType, string month, string year, string valinsp, string FromDate, string ToDate, string ICDate, string BillDate, string formonth, string forperiod, string monthChar)
        {
            ConsigneeCompReports model = new() { ReportType = ReportType,
                month = month,
                year = year,
                valinsp = valinsp,
                FromDate = FromDate,
                ToDate = ToDate,
                ICDate = ICDate,
                BillDate = BillDate,
                formonth = formonth,
                monthChar = monthChar,
                forperiod = forperiod
            };
            if (ReportType == "TOPNHIGH") model.ReportTitle = "Top 'N' High Value Inspection";
            return View("Manage", model);
        }

        public IActionResult ManageByCORP(string ReportType, string FromDate, string ToDate, string Allregion, string regionorth, string regionsouth, string regioneast, string regionwest, string jiallregion,
           string jinorth, string jisourth, string jieast, string jiwest, string compallregion, string compyes, string compno, string cancelled, string underconsider, string allaction, string particilaraction, string actiondrp
            , string particilarcode, string particilarjicode, string actioncodedrp, string actionjidrp)
        {
            ConsigneeCompReports model = new() { ReportType = ReportType,
                FromDate = FromDate,
                ToDate = ToDate,
                Allregion = Allregion,
                regionorth = regionorth,
                regionsouth = regionsouth,
                regioneast = regioneast,
                regionwest = regionwest,
                jiallregion = jiallregion,
                jinorth = jinorth,
                jisourth = jisourth,
                jieast = jieast,
                jiwest = jiwest,
                compallregion = compallregion,
                compyes = compyes,
                compno = compno,
                cancelled = cancelled,
                underconsider = underconsider,
                allaction = allaction,
                particilaraction = particilaraction,
                actiondrp = actiondrp,
                particilarcode = particilarcode,
                particilarjicode = particilarcode,
                actioncodedrp = actioncodedrp,
                actionjidrp = actionjidrp
            };
            if (ReportType == "CORP") model.ReportTitle = "JI CONSIGNEE COMPLAINTS";
            return View("Manage", model);
        }

        public IActionResult ComplaintsByPeriod(string FromDate, string ToDate, string Allregion, string regionorth, string regionsouth, string regioneast, string regionwest,
            string compallregion, string compyes, string compno, string cancelled, string underconsider, string actiondrp, string actioncodedrp, string actionjidrp)
        {
            string region = "", jirequired = "";
            ConsigneeCompReports model = consigneeCompReportRepository.GetCompPeriodData(FromDate, ToDate, actiondrp, actioncodedrp, actionjidrp);

            region = (Allregion == "true") ? "AllRegion" :
                     (regionorth == "true") ? "Northern Region" :
                     (regionsouth == "true") ? "Southern Region" :
                     (regioneast == "true") ? "Eastern Region" :
                     (regionwest == "true") ? "Western Region" :
                     "";

            jirequired = (compallregion == "true") ? "" :
                      (compyes == "true") ? "& JI Required" :
                      (compno == "true") ? "& JI Not Required" :
                      (cancelled == "true") ? " & JI Cancelled" :
                      (underconsider == "true") ? "& JI Under Consideration" :
                      "";

            ViewBag.Regions = region;
            ViewBag.JiRequiredStatus = jirequired;
            GlobalDeclaration.ConsigneeCompPeriod = model;
            return PartialView(model);
        }

        public IActionResult ComplaintRecieved(string FromDate, string ToDate, string AllCM, string AllIEs, string AllVendors, string AllClient, string AllConsignee, string Compact, string AwaitingJI, string JIConclusion, string JIConclusionfollowup,
           string JIconclusionreport, string JIDecidedDT, string All, string ParticularIEs, string IEWise, string CMWise, string VendorWise, string ClientWise, string ConsigneeWise, string FinancialYear, string ParticularCMs, string ParticularClients, string ParticularConsignee,
           string ParticularVendor, string Detailed, string FinancialYears, string ddlsupercm, string ddliename, string Clientwiseddl, string vendor, string Item, string consignee, string FinancialYearsvalue)
        {
            string Region = SessionHelper.UserModelDTO.Region;
            string wRegion = "";
            if (Region == "N") { wRegion = "Northern Region"; }
            else if (Region == "S") { wRegion = "Southern Region"; }
            else if (Region == "E") { wRegion = "Eastern Region"; }
            else if (Region == "W") { wRegion = "Western Region"; }
            else if (Region == "C") { wRegion = "Central Region"; }
            JIRequiredReport model = consigneeCompReportRepository.GetJIRequiredList(FromDate, ToDate, AllCM, AllIEs, AllVendors, AllClient, AllConsignee, Compact, AwaitingJI, JIConclusion, JIConclusionfollowup,
            JIconclusionreport, JIDecidedDT, All, ParticularIEs, IEWise, CMWise, VendorWise, ClientWise, ConsigneeWise, FinancialYear, ParticularCMs, ParticularClients, ParticularConsignee,
            ParticularVendor, Detailed, FinancialYears, ddlsupercm, ddliename, Clientwiseddl, vendor, Item, consignee, Region, FinancialYearsvalue);
            ViewBag.FromDT = FromDate;
            ViewBag.ToDT = ToDate;
            ViewBag.Financialperiod = FinancialYears;
            ViewBag.Regions = wRegion;
            ViewBag.Detailedchk = Detailed;
            GlobalDeclaration.JIRequiredReports = model;
            return PartialView(model);
        }

        public IActionResult JIComplaintsReport(string JISNO)
        {
            string Region = SessionHelper.UserModelDTO.Region;
            string wRegion = "";
            if (Region == "N") { wRegion = "Northern Region"; }
            else if (Region == "S") { wRegion = "Southern Region"; }
            else if (Region == "E") { wRegion = "Eastern Region"; }
            else if (Region == "W") { wRegion = "Western Region"; }
            else if (Region == "C") { wRegion = "Central Region"; }
            ConsigneeComplaints model = consigneeCompReportRepository.GetComplaintReportDetails(JISNO, Region);
            GlobalDeclaration.ConsigneeComplaint = model;
            ViewBag.Regions = wRegion;
            return PartialView(model);
        }

        public IActionResult ComplaintsByCONJI(string FromDate, string ToDate, string Allregion, string regionorth, string regionsouth, string regioneast, string regionwest,
           string compallregion, string compyes, string compno, string cancelled, string underconsider, string actiondrp, string actioncodedrp, string actionjidrp)
        {
            string region = "", jirequired = "";
            ConsigneeCompReports model = consigneeCompReportRepository.GetCompPeriodData(FromDate, ToDate, actiondrp, actioncodedrp, actionjidrp);

            region = (Allregion == "true") ? "AllRegion" :
                     (regionorth == "true") ? "Northern Region" :
                     (regionsouth == "true") ? "Southern Region" :
                     (regioneast == "true") ? "Eastern Region" :
                     (regionwest == "true") ? "Western Region" :
                     "";

            jirequired = (compallregion == "true") ? "" :
                      (compyes == "true") ? "& JI Required" :
                      (compno == "true") ? "& JI Not Required" :
                      (cancelled == "true") ? " & JI Cancelled" :
                      (underconsider == "true") ? "& JI Under Consideration" :
                      "";

            ViewBag.Regions = region;
            ViewBag.JiRequiredStatus = jirequired;
            return PartialView("~/Views/ConsigneeCompPeriod/ComplaintsByPeriod.cshtml", model);
        }

        public IActionResult DefectCodeReport(DateTime FromDate, DateTime ToDate)
        {
            string Region = SessionHelper.UserModelDTO.Region;
            string wRegion = "";
            if (Region == "N") { wRegion = "Northern Region"; }
            else if (Region == "S") { wRegion = "Southern Region"; }
            else if (Region == "E") { wRegion = "Eastern Region"; }
            else if (Region == "W") { wRegion = "Western Region"; }
            else if (Region == "C") { wRegion = "Central Region"; }
            DefectCodeReport model = consigneeCompReportRepository.GetDefectCodeWiseData(FromDate, ToDate, Region);
            ViewBag.Regions = wRegion;
            GlobalDeclaration.DefectCodeReports = model;
            return PartialView(model);
        }

        public IActionResult JICompReport(string FinancialYearsText, string FinancialYearsValue)
        {
            string Region = SessionHelper.UserModelDTO.Region;
            string wRegion = "";
            if (Region == "N") { wRegion = "Northern Region"; }
            else if (Region == "S") { wRegion = "Southern Region"; }
            else if (Region == "E") { wRegion = "Eastern Region"; }
            else if (Region == "W") { wRegion = "Western Region"; }
            else if (Region == "C") { wRegion = "Central Region"; }
            JIRequiredReport model = consigneeCompReportRepository.GetJIComplaintsList(FinancialYearsText, FinancialYearsValue);
            ViewBag.Financialperiod = FinancialYearsText;
            ViewBag.Regions = wRegion;
            GlobalDeclaration.JIRequiredReports = model;
            return PartialView(model);
        }

        public IActionResult TopNHighValueInsp(string month, string year, string valinsp, string FromDate, string ToDate, string ICDate, string BillDate, string formonth, string forperiod, string monthChar)
        {
            string Region = SessionHelper.UserModelDTO.Region;
            string wRegion = "";
            if (Region == "N") { wRegion = "Northern Region"; }
            else if (Region == "S") { wRegion = "Southern Region"; }
            else if (Region == "E") { wRegion = "Eastern Region"; }
            else if (Region == "W") { wRegion = "Western Region"; }
            else if (Region == "C") { wRegion = "Central Region"; }
            HighValueInspReport model = consigneeCompReportRepository.GetHighValueInspdata(month, year, valinsp, FromDate, ToDate, ICDate, BillDate, formonth, forperiod, Region);
            ViewBag.Regions = wRegion;
            ViewBag.FromDT = FromDate;
            ViewBag.ToDT = ToDate;
            ViewBag.yearshow = year;
            ViewBag.monthshow = monthChar;
            ViewBag.TotalInspValue = valinsp;
            ViewBag.BillDT = (BillDate == "true") ? "Report Based On Bill Date" : "";
            ViewBag.ICDT = (ICDate == "true") ? "Report Based On IC Date" : "";
            GlobalDeclaration.HighValueInspReports = model;
            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> GeneratePDF(string ReportType)
        {
            string htmlContent = string.Empty;

            if (ReportType == "U" && ReportType == "CORP")
            {
                ConsigneeCompReports model = GlobalDeclaration.ConsigneeCompPeriod;
                htmlContent = await this.RenderViewToStringAsync("/Views/ConsigneeCompReport/ComplaintsByPeriod.cshtml", model);
            }else if (ReportType == "COMPJI")
            {
                JIRequiredReport model = GlobalDeclaration.JIRequiredReports;
                htmlContent = await this.RenderViewToStringAsync("/Views/ConsigneeCompReport/ComplaintRecieved.cshtml", model);
            }else if (ReportType == "TOPJI")
            {
                ConsigneeComplaints model = GlobalDeclaration.ConsigneeComplaint;
                htmlContent = await this.RenderViewToStringAsync("/Views/ConsigneeCompReport/JIComplaintsReport.cshtml", model);
            }else if (ReportType == "DCWACOMPS")
            {
                DefectCodeReport model = GlobalDeclaration.DefectCodeReports;
                htmlContent = await this.RenderViewToStringAsync("/Views/ConsigneeCompReport/DefectCodeReport.cshtml", model);
            }else if (ReportType == "COCOMPJI")
            {
                DefectCodeReport model = GlobalDeclaration.DefectCodeReports;
                htmlContent = await this.RenderViewToStringAsync("/Views/ConsigneeCompReport/JICompReport.cshtml", model);
            }else if (ReportType == "TOPNHIGH")
            {
                HighValueInspReport model = GlobalDeclaration.HighValueInspReports;
                htmlContent = await this.RenderViewToStringAsync("/Views/ConsigneeCompReport/TopNHighValueInsp.cshtml", model);
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
