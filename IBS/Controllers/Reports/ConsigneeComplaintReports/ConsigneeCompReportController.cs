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
using Newtonsoft.Json;

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
            ViewBag.Regions = Region;
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

        public IActionResult Manage(string ReportType)
        {
            ConsigneeCompReports model = new();

            if (TempData.ContainsKey(ReportType))
            {
                model = JsonConvert.DeserializeObject<ConsigneeCompReports>(TempData.Peek(ReportType).ToString());
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult ManageReportData(IFormCollection formCollection)
        {
            ConsigneeCompReports model = new();

            if (formCollection.Keys.Contains("hdnReportType")) model.ReportType = formCollection["hdnReportType"];
            model.ReportTitle = EnumUtility<Enums.ManagementReportsTitle>.GetDescriptionByKey(model.ReportType);

            if (model.ReportType == "CCU")
            {
                if (formCollection.Keys.Contains("hdncompfromdt") && !string.IsNullOrEmpty(formCollection["hdncompfromdt"])) model.FromDate = Convert.ToString(formCollection["hdncompfromdt"]);
                if (formCollection.Keys.Contains("hdncomptodt") && !string.IsNullOrEmpty(formCollection["hdncomptodt"])) model.ToDate = Convert.ToString(formCollection["hdncomptodt"]);
                if (formCollection.Keys.Contains("hdnallRegions") && !string.IsNullOrEmpty(formCollection["hdnallRegions"])) model.Allregion = Convert.ToString(formCollection["hdnallRegions"]);
                if (formCollection.Keys.Contains("hdnnorthern") && !string.IsNullOrEmpty(formCollection["hdnnorthern"])) model.regionorth = Convert.ToString(formCollection["hdnnorthern"]);
                if (formCollection.Keys.Contains("hdnsouthern") && !string.IsNullOrEmpty(formCollection["hdnsouthern"])) model.regionsouth = Convert.ToString(formCollection["hdnsouthern"]);
                if (formCollection.Keys.Contains("hdneastern") && !string.IsNullOrEmpty(formCollection["hdneastern"])) model.regioneast = Convert.ToString(formCollection["hdneastern"]);
                if (formCollection.Keys.Contains("hdnwestern") && !string.IsNullOrEmpty(formCollection["hdnwestern"])) model.regionwest = Convert.ToString(formCollection["hdnwestern"]);
                if (formCollection.Keys.Contains("hdnalljiRegions") && !string.IsNullOrEmpty(formCollection["hdnalljiRegions"])) model.jiallregion = Convert.ToString(formCollection["hdnalljiRegions"]);
                if (formCollection.Keys.Contains("hdnnorthernji") && !string.IsNullOrEmpty(formCollection["hdnnorthernji"])) model.jinorth = Convert.ToString(formCollection["hdnnorthernji"]);
                if (formCollection.Keys.Contains("hdnsouthernji") && !string.IsNullOrEmpty(formCollection["hdnsouthernji"])) model.jisourth = Convert.ToString(formCollection["hdnsouthernji"]);
                if (formCollection.Keys.Contains("hdneasternji") && !string.IsNullOrEmpty(formCollection["hdneasternji"])) model.jieast = Convert.ToString(formCollection["hdneasternji"]);
                if (formCollection.Keys.Contains("hdnwesternji") && !string.IsNullOrEmpty(formCollection["hdnwesternji"])) model.jiwest = Convert.ToString(formCollection["hdnwesternji"]);
                if (formCollection.Keys.Contains("hdncompallRegions") && !string.IsNullOrEmpty(formCollection["hdncompallRegions"])) model.compallregion = Convert.ToString(formCollection["hdncompallRegions"]);
                if (formCollection.Keys.Contains("hdnYes") && !string.IsNullOrEmpty(formCollection["hdnYes"])) model.compyes = Convert.ToString(formCollection["hdnYes"]);
                if (formCollection.Keys.Contains("hdnNo") && !string.IsNullOrEmpty(formCollection["hdnNo"])) model.compno = Convert.ToString(formCollection["hdnNo"]);
                if (formCollection.Keys.Contains("hdnCancelled") && !string.IsNullOrEmpty(formCollection["hdnCancelled"])) model.cancelled = Convert.ToString(formCollection["hdnCancelled"]);
                if (formCollection.Keys.Contains("hdnUnderConsideration") && !string.IsNullOrEmpty(formCollection["hdnUnderConsideration"])) model.underconsider = Convert.ToString(formCollection["hdnUnderConsideration"]);
                if (formCollection.Keys.Contains("hdnallaction") && !string.IsNullOrEmpty(formCollection["hdnallaction"])) model.allaction = Convert.ToString(formCollection["hdnallaction"]);
                if (formCollection.Keys.Contains("hdnParticularAction") && !string.IsNullOrEmpty(formCollection["hdnParticularAction"])) model.particilaraction = Convert.ToString(formCollection["hdnParticularAction"]);
                if (formCollection.Keys.Contains("hdnParticularActiondrp") && !string.IsNullOrEmpty(formCollection["hdnParticularActiondrp"])) model.actiondrp = Convert.ToString(formCollection["hdnParticularActiondrp"]);
            } if (model.ReportType == "COMPJI")
            {
                if (formCollection.Keys.Contains("hdnFromDate") && !string.IsNullOrEmpty(formCollection["hdnFromDate"])) model.FromDate = Convert.ToString(formCollection["hdnFromDate"]);
                if (formCollection.Keys.Contains("hdnToDate") && !string.IsNullOrEmpty(formCollection["hdnToDate"])) model.ToDate = Convert.ToString(formCollection["hdnToDate"]);
                if (formCollection.Keys.Contains("hdnAllCM") && !string.IsNullOrEmpty(formCollection["hdnAllCM"])) model.AllCM = Convert.ToString(formCollection["hdnAllCM"]);
                if (formCollection.Keys.Contains("hdnAllIEs") && !string.IsNullOrEmpty(formCollection["hdnAllIEs"])) model.AllIEs = Convert.ToString(formCollection["hdnAllIEs"]);
                if (formCollection.Keys.Contains("hdnAllVendors") && !string.IsNullOrEmpty(formCollection["hdnAllVendors"])) model.AllVendors = Convert.ToString(formCollection["hdnAllVendors"]);
                if (formCollection.Keys.Contains("hdnAllClient") && !string.IsNullOrEmpty(formCollection["hdnAllClient"])) model.AllClient = Convert.ToString(formCollection["hdnAllClient"]);
                if (formCollection.Keys.Contains("hdnAllConsignee") && !string.IsNullOrEmpty(formCollection["hdnAllConsignee"])) model.AllConsignee = Convert.ToString(formCollection["hdnAllConsignee"]);
                if (formCollection.Keys.Contains("hdnCompact") && !string.IsNullOrEmpty(formCollection["hdnCompact"])) model.Compact = Convert.ToString(formCollection["hdnCompact"]);
                if (formCollection.Keys.Contains("hdnAwaitingJI") && !string.IsNullOrEmpty(formCollection["hdnAwaitingJI"])) model.AwaitingJI = Convert.ToString(formCollection["hdnAwaitingJI"]);
                if (formCollection.Keys.Contains("hdnJIConclusion") && !string.IsNullOrEmpty(formCollection["hdnJIConclusion"])) model.JIConclusion = Convert.ToString(formCollection["hdnJIConclusion"]);
                if (formCollection.Keys.Contains("hdnJIConclusionfollowup") && !string.IsNullOrEmpty(formCollection["hdnJIConclusionfollowup"])) model.JIConclusionfollowup = Convert.ToString(formCollection["hdnJIConclusionfollowup"]);
                if (formCollection.Keys.Contains("hdnJIconclusionreport") && !string.IsNullOrEmpty(formCollection["hdnJIconclusionreport"])) model.JIconclusionreport = Convert.ToString(formCollection["hdnJIconclusionreport"]);
                if (formCollection.Keys.Contains("hdnJIDecidedDT") && !string.IsNullOrEmpty(formCollection["hdnJIDecidedDT"])) model.JIDecidedDT = Convert.ToString(formCollection["hdnJIDecidedDT"]);
                if (formCollection.Keys.Contains("hdnAll") && !string.IsNullOrEmpty(formCollection["hdnAll"])) model.All = Convert.ToString(formCollection["hdnAll"]);
                if (formCollection.Keys.Contains("hdnParticularIEs") && !string.IsNullOrEmpty(formCollection["hdnParticularIEs"])) model.ParticularIEs = Convert.ToString(formCollection["hdnParticularIEs"]);
                if (formCollection.Keys.Contains("hdnIEWise") && !string.IsNullOrEmpty(formCollection["hdnIEWise"])) model.IEWise = Convert.ToString(formCollection["hdnIEWise"]);
                if (formCollection.Keys.Contains("hdnCMWise") && !string.IsNullOrEmpty(formCollection["hdnCMWise"])) model.CMWise = Convert.ToString(formCollection["hdnCMWise"]);
                if (formCollection.Keys.Contains("hdnVendorWise") && !string.IsNullOrEmpty(formCollection["hdnVendorWise"])) model.VendorWise = Convert.ToString(formCollection["hdnVendorWise"]);
                if (formCollection.Keys.Contains("hdnClientWise") && !string.IsNullOrEmpty(formCollection["hdnClientWise"])) model.ClientWise = Convert.ToString(formCollection["hdnClientWise"]);
                if (formCollection.Keys.Contains("hdnConsigneeWise") && !string.IsNullOrEmpty(formCollection["hdnConsigneeWise"])) model.ConsigneeWise = Convert.ToString(formCollection["hdnConsigneeWise"]);
                if (formCollection.Keys.Contains("hdnFinancialYear") && !string.IsNullOrEmpty(formCollection["hdnFinancialYear"])) model.FinancialYear = Convert.ToString(formCollection["hdnFinancialYear"]);
                if (formCollection.Keys.Contains("hdnParticularCMs") && !string.IsNullOrEmpty(formCollection["hdnParticularCMs"])) model.ParticularCMs = Convert.ToString(formCollection["hdnParticularCMs"]);
                if (formCollection.Keys.Contains("hdnParticularClients") && !string.IsNullOrEmpty(formCollection["hdnParticularClients"])) model.ParticularClients = Convert.ToString(formCollection["hdnParticularClients"]);
                if (formCollection.Keys.Contains("hdnParticularConsignee") && !string.IsNullOrEmpty(formCollection["hdnParticularConsignee"])) model.ParticularConsignee = Convert.ToString(formCollection["hdnParticularConsignee"]);
                if (formCollection.Keys.Contains("hdnParticularVendor") && !string.IsNullOrEmpty(formCollection["hdnParticularVendor"])) model.ParticularVendor = Convert.ToString(formCollection["hdnParticularVendor"]);
                if (formCollection.Keys.Contains("hdnDetailed") && !string.IsNullOrEmpty(formCollection["hdnDetailed"])) model.Detailed = Convert.ToString(formCollection["hdnDetailed"]);
                if (formCollection.Keys.Contains("hdnFinancialYears") && !string.IsNullOrEmpty(formCollection["hdnFinancialYears"])) model.FinancialYears = Convert.ToString(formCollection["hdnFinancialYears"]);
                if (formCollection.Keys.Contains("hdnFinancialYears") && !string.IsNullOrEmpty(formCollection["hdnFinancialYears"])) model.FinancialYearsValue = Convert.ToString(formCollection["hdnFinancialYears"]);
                if (formCollection.Keys.Contains("hdnddlsupercm") && !string.IsNullOrEmpty(formCollection["hdnddlsupercm"])) model.ddlsupercm = Convert.ToString(formCollection["hdnddlsupercm"]);
                if (formCollection.Keys.Contains("hdnddliename") && !string.IsNullOrEmpty(formCollection["hdnddliename"])) model.ddliename = Convert.ToString(formCollection["hdnddliename"]);
                if (formCollection.Keys.Contains("hdnClientwiseddl") && !string.IsNullOrEmpty(formCollection["hdnClientwiseddl"])) model.Clientwiseddl = Convert.ToString(formCollection["hdnClientwiseddl"]);
                if (formCollection.Keys.Contains("hdnvendor") && !string.IsNullOrEmpty(formCollection["hdnvendor"])) model.vendor = Convert.ToString(formCollection["hdnvendor"]);
                if (formCollection.Keys.Contains("hdnItem") && !string.IsNullOrEmpty(formCollection["hdnItem"])) model.Item = Convert.ToString(formCollection["hdnItem"]);
                if (formCollection.Keys.Contains("hdnconsignee") && !string.IsNullOrEmpty(formCollection["hdnconsignee"])) model.consignee = Convert.ToString(formCollection["hdnconsignee"]);
            }if (model.ReportType == "CORP")
            {
                if (formCollection.Keys.Contains("hdncompfromdtcorp") && !string.IsNullOrEmpty(formCollection["hdncompfromdtcorp"])) model.FromDate = Convert.ToString(formCollection["hdncompfromdtcorp"]);
                if (formCollection.Keys.Contains("hdncomptodtcorp") && !string.IsNullOrEmpty(formCollection["hdncomptodtcorp"])) model.ToDate = Convert.ToString(formCollection["hdncomptodtcorp"]);
                if (formCollection.Keys.Contains("hdnallRegionscorp") && !string.IsNullOrEmpty(formCollection["hdnallRegionscorp"])) model.Allregion = Convert.ToString(formCollection["hdnallRegionscorp"]);
                if (formCollection.Keys.Contains("hdnnortherncorp") && !string.IsNullOrEmpty(formCollection["hdnnortherncorp"])) model.regionorth = Convert.ToString(formCollection["hdnnortherncorp"]);
                if (formCollection.Keys.Contains("hdnsoutherncorp") && !string.IsNullOrEmpty(formCollection["hdnsoutherncorp"])) model.regionsouth = Convert.ToString(formCollection["hdnsoutherncorp"]);
                if (formCollection.Keys.Contains("hdneasterncorp") && !string.IsNullOrEmpty(formCollection["hdneasterncorp"])) model.regioneast = Convert.ToString(formCollection["hdneasterncorp"]);
                if (formCollection.Keys.Contains("hdnwesterncorp") && !string.IsNullOrEmpty(formCollection["hdnwesterncorp"])) model.regionwest = Convert.ToString(formCollection["hdnwesterncorp"]);
                if (formCollection.Keys.Contains("hdnalljiRegionscorp") && !string.IsNullOrEmpty(formCollection["hdnalljiRegionscorp"])) model.jiallregion = Convert.ToString(formCollection["hdnalljiRegionscorp"]);
                if (formCollection.Keys.Contains("hdnnorthernjicorp") && !string.IsNullOrEmpty(formCollection["hdnnorthernjicorp"])) model.jinorth = Convert.ToString(formCollection["hdnnorthernjicorp"]);
                if (formCollection.Keys.Contains("hdnsouthernjicorp") && !string.IsNullOrEmpty(formCollection["hdnsouthernjicorp"])) model.jisourth = Convert.ToString(formCollection["hdnsouthernjicorp"]);
                if (formCollection.Keys.Contains("hdneasternjicorp") && !string.IsNullOrEmpty(formCollection["hdneasternjicorp"])) model.jieast = Convert.ToString(formCollection["hdneasternjicorp"]);
                if (formCollection.Keys.Contains("hdnwesternjicorp") && !string.IsNullOrEmpty(formCollection["hdnwesternjicorp"])) model.jiwest = Convert.ToString(formCollection["hdnwesternjicorp"]);
                if (formCollection.Keys.Contains("hdncompallRegionscorp") && !string.IsNullOrEmpty(formCollection["hdncompallRegionscorp"])) model.compallregion = Convert.ToString(formCollection["hdncompallRegionscorp"]);
                if (formCollection.Keys.Contains("hdnYescorp") && !string.IsNullOrEmpty(formCollection["hdnYescorp"])) model.compyes = Convert.ToString(formCollection["hdnYescorp"]);
                if (formCollection.Keys.Contains("hdnNocorp") && !string.IsNullOrEmpty(formCollection["hdnNocorp"])) model.compno = Convert.ToString(formCollection["hdnNocorp"]);
                if (formCollection.Keys.Contains("hdnCancelledcorp") && !string.IsNullOrEmpty(formCollection["hdnCancelledcorp"])) model.cancelled = Convert.ToString(formCollection["hdnCancelledcorp"]);
                if (formCollection.Keys.Contains("hdnUnderConsiderationcorp") && !string.IsNullOrEmpty(formCollection["hdnUnderConsiderationcorp"])) model.underconsider = Convert.ToString(formCollection["hdnUnderConsiderationcorp"]);
                if (formCollection.Keys.Contains("hdnallactioncorp") && !string.IsNullOrEmpty(formCollection["hdnallactioncorp"])) model.allaction = Convert.ToString(formCollection["hdnallactioncorp"]);
                if (formCollection.Keys.Contains("hdnParticularActioncorp") && !string.IsNullOrEmpty(formCollection["hdnParticularActioncorp"])) model.particilaraction = Convert.ToString(formCollection["hdnParticularActioncorp"]);
                if (formCollection.Keys.Contains("hdnDefectCodecorp") && !string.IsNullOrEmpty(formCollection["hdnDefectCodecorp"])) model.particilarcode = Convert.ToString(formCollection["hdnDefectCodecorp"]);
                if (formCollection.Keys.Contains("hdnParticularjicodecorp") && !string.IsNullOrEmpty(formCollection["hdnParticularjicodecorp"])) model.particilarjicode = Convert.ToString(formCollection["hdnParticularjicodecorp"]);
                if (formCollection.Keys.Contains("hdnParticularDefectCodecorp") && !string.IsNullOrEmpty(formCollection["hdnParticularDefectCodecorp"])) model.actioncodedrp = Convert.ToString(formCollection["hdnParticularDefectCodecorp"]);
                if (formCollection.Keys.Contains("hdnParticularJIcorp") && !string.IsNullOrEmpty(formCollection["hdnParticularJIcorp"])) model.actionjidrp = Convert.ToString(formCollection["hdnParticularJIcorp"]);
            }
            if (model.ReportType == "TOPNHIGH")
            {
                if (formCollection.Keys.Contains("hdnmonth") && !string.IsNullOrEmpty(formCollection["hdnmonth"])) model.month = Convert.ToString(formCollection["hdnmonth"]);
                if (formCollection.Keys.Contains("hdnmonth") && !string.IsNullOrEmpty(formCollection["hdnmonth"])) model.monthChar = Convert.ToString(formCollection["hdnmonth"]);
                if (formCollection.Keys.Contains("hdnyear") && !string.IsNullOrEmpty(formCollection["hdnyear"])) model.year = Convert.ToString(formCollection["hdnyear"]);
                if (formCollection.Keys.Contains("hdnvalinsp") && !string.IsNullOrEmpty(formCollection["hdnvalinsp"])) model.valinsp = Convert.ToString(formCollection["hdnvalinsp"]);
                if (formCollection.Keys.Contains("hdnFromDate") && !string.IsNullOrEmpty(formCollection["hdnFromDate"])) model.FromDate = Convert.ToString(formCollection["hdnFromDate"]);
                if (formCollection.Keys.Contains("hdnToDate") && !string.IsNullOrEmpty(formCollection["hdnToDate"])) model.ToDate = Convert.ToString(formCollection["hdnToDate"]);
                if (formCollection.Keys.Contains("hdnICDate") && !string.IsNullOrEmpty(formCollection["hdnICDate"])) model.ICDate = Convert.ToString(formCollection["hdnICDate"]);
                if (formCollection.Keys.Contains("hdnBillDate") && !string.IsNullOrEmpty(formCollection["hdnBillDate"])) model.BillDate = Convert.ToString(formCollection["hdnBillDate"]);
                if (formCollection.Keys.Contains("hdnformonth") && !string.IsNullOrEmpty(formCollection["hdnformonth"])) model.formonth = Convert.ToString(formCollection["hdnformonth"]);
                if (formCollection.Keys.Contains("hdnforperiod") && !string.IsNullOrEmpty(formCollection["hdnforperiod"])) model.forperiod = Convert.ToString(formCollection["hdnforperiod"]);
            }

            TempData[model.ReportType] = JsonConvert.SerializeObject(model);
            return RedirectToAction("Manage", new { model.ReportType });
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

            model.Regions = region;
            model.jirequired = jirequired;
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
            model.FromDate = Convert.ToDateTime(FromDate).ToString("dd/MM/yyyy");
            model.ToDate = Convert.ToDateTime(ToDate).ToString("dd/MM/yyyy");
            model.FinancialYears = FinancialYears;
            model.Regions = wRegion;
            model.Detailed = Detailed;
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

            model.Regions = region;
            model.jirequired = jirequired;
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
            model.Regions = wRegion;
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
            model.FinancialYearsText = FinancialYearsText;
            model.Regions = wRegion;
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
            model.Regions = wRegion;
            model.FromDate = Convert.ToDateTime(FromDate).ToString("dd/MM/yyyy");
            model.ToDate = Convert.ToDateTime(ToDate).ToString("dd/MM/yyyy");
            model.year = year;
            model.monthChar = monthChar;
            model.valinsp = valinsp;
            model.BillDate = (BillDate == "true") ? "Report Based On Bill Date" : "";
            model.ICDate = (ICDate == "true") ? "Report Based On IC Date" : "";
            GlobalDeclaration.HighValueInspReports = model;
            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> GeneratePDF(string ReportType)
        {
            string htmlContent = string.Empty;

            if (ReportType == "CCU" || ReportType == "CORP")
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
                JIRequiredReport model = GlobalDeclaration.JIRequiredReports;
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
