using IBS.Filters;
using IBS.Helper;
using IBS.Interfaces.Reports.OtherReports;
using IBS.Models;
using IBS.Models.Reports;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace IBS.Controllers.Reports.OtherReports
{
    [Authorization]
    public class OtherReportsController : BaseController
    {
        private readonly IOtherReportsRepository otherReportsRepository;
        private readonly IWebHostEnvironment env;

        public OtherReportsController(IOtherReportsRepository _otherReportsRepository, IWebHostEnvironment _env)
        {
            otherReportsRepository = _otherReportsRepository;
            this.env = _env;
        }
        public IActionResult Index()
        {
            ViewBag.Regions = Region;
            return View();
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            string Region = SessionHelper.UserModelDTO.Region;
            DTResult<IEICPhotoEnclosedModelReport> dTResult = otherReportsRepository.GetDataList(dtParameters, Region);
            return Json(dTResult);
        }

        public IActionResult Manage(string ReportType)
        {
            OtherReportsModel model = new() { ReportType = ReportType };
            if (ReportType == "COWISEIE") model.ReportTitle = "Controlling Officer's Wise Inspection Engineers";
            return View(model);
        }

        public IActionResult ManageCoIeWiseCalls(string ReportType, string Case_No, string Call_Recv_Date, string Call_SNo)
        {
            OtherReportsModel model = new() { ReportType = ReportType, Case_No = Case_No, Call_Recv_Date = Call_Recv_Date, Call_SNo = Call_SNo };
            return View("Manage", model);
        }
        public IActionResult Manage1(string ReportType)
        {
            OtherReportsModel model = new();

            if (TempData.ContainsKey(ReportType))
            {
                model = JsonConvert.DeserializeObject<OtherReportsModel>(TempData.Peek(ReportType).ToString());
            }
            return View("Manage", model);
        }
        [HttpPost]
        public IActionResult ManageReportData(IFormCollection formCollection)
        {
            OtherReportsModel model = new();

            if (formCollection.Keys.Contains("hdnReportType")) model.ReportType = formCollection["hdnReportType"];
            model.ReportTitle = EnumUtility<Enums.ManagementReportsTitle>.GetDescriptionByKey(model.ReportType);

            if (model.ReportType == "C" || model.ReportType == "I")
            {
                if (formCollection.Keys.Contains("hdnmonthtxt") && !string.IsNullOrEmpty(formCollection["hdnmonthtxt"])) model.monthChar = Convert.ToString(formCollection["hdnmonthtxt"]);
                if (formCollection.Keys.Contains("hdnmonth") && !string.IsNullOrEmpty(formCollection["hdnmonth"])) model.month = Convert.ToString(formCollection["hdnmonth"]);
                if (formCollection.Keys.Contains("hdnyear") && !string.IsNullOrEmpty(formCollection["hdnyear"])) model.year = Convert.ToString(formCollection["hdnyear"]);
                if (formCollection.Keys.Contains("hdnFromDate") && !string.IsNullOrEmpty(formCollection["hdnFromDate"])) model.FromDate = Convert.ToDateTime(formCollection["hdnFromDate"]);
                if (formCollection.Keys.Contains("hdnToDate") && !string.IsNullOrEmpty(formCollection["hdnToDate"])) model.ToDate = Convert.ToDateTime(formCollection["hdnToDate"]);
                if (formCollection.Keys.Contains("hdnAllCM") && !string.IsNullOrEmpty(formCollection["hdnAllCM"])) model.AllCM = Convert.ToString(formCollection["hdnAllCM"]);
                if (formCollection.Keys.Contains("hdnforCM") && !string.IsNullOrEmpty(formCollection["hdnforCM"])) model.forCM = Convert.ToString(formCollection["hdnforCM"]);
                if (formCollection.Keys.Contains("hdnAll") && !string.IsNullOrEmpty(formCollection["hdnAll"])) model.All = Convert.ToString(formCollection["hdnAll"]);
                if (formCollection.Keys.Contains("hdnOutstanding") && !string.IsNullOrEmpty(formCollection["hdnOutstanding"])) model.Outstanding = Convert.ToString(formCollection["hdnOutstanding"]);
                if (formCollection.Keys.Contains("hdnformonth") && !string.IsNullOrEmpty(formCollection["hdnformonth"])) model.formonth = Convert.ToString(formCollection["hdnformonth"]);
                if (formCollection.Keys.Contains("hdnforperiod") && !string.IsNullOrEmpty(formCollection["hdnforperiod"])) model.forperiod = Convert.ToString(formCollection["hdnforperiod"]);
                if (formCollection.Keys.Contains("hdniecmname") && !string.IsNullOrEmpty(formCollection["hdniecmname"])) model.iecmname = Convert.ToString(formCollection["hdniecmname"]);
                if (formCollection.Keys.Contains("hdnCOName") && !string.IsNullOrEmpty(formCollection["hdnCOName"])) model.COName = Convert.ToString(formCollection["hdnCOName"]);
                if (formCollection.Keys.Contains("hdnIENametext") && !string.IsNullOrEmpty(formCollection["hdnIENametext"])) model.IENametext = Convert.ToString(formCollection["hdnIENametext"]);
            }
            else if (model.ReportType == "IEWISET")
            {
                if (formCollection.Keys.Contains("hdnIENAME") && !string.IsNullOrEmpty(formCollection["hdnIENAME"])) model.IEName = Convert.ToString(formCollection["hdnIENAME"]);
                if (formCollection.Keys.Contains("TrainingArea") && !string.IsNullOrEmpty(formCollection["TrainingArea"])) model.TrainingArea = Convert.ToString(formCollection["TrainingArea"]);
                if (formCollection.Keys.Contains("hdnMechanical") && !string.IsNullOrEmpty(formCollection["hdnMechanical"])) model.Mechanical = Convert.ToString(formCollection["hdnMechanical"]);
                if (formCollection.Keys.Contains("hdnElectrical") && !string.IsNullOrEmpty(formCollection["hdnElectrical"])) model.Electrical = Convert.ToString(formCollection["hdnElectrical"]);
                if (formCollection.Keys.Contains("hdnCivil") && !string.IsNullOrEmpty(formCollection["hdnCivil"])) model.Civil = Convert.ToString(formCollection["hdnCivil"]);
                if (formCollection.Keys.Contains("hdnRegular") && !string.IsNullOrEmpty(formCollection["hdnRegular"])) model.Regular = Convert.ToString(formCollection["hdnRegular"]);
                if (formCollection.Keys.Contains("hdnDeputaion") && !string.IsNullOrEmpty(formCollection["hdnDeputaion"])) model.Deputaion = Convert.ToString(formCollection["hdnDeputaion"]);
                if (formCollection.Keys.Contains("hdnParticularie") && !string.IsNullOrEmpty(formCollection["hdnParticularie"])) model.Particularie = Convert.ToString(formCollection["hdnParticularie"]);
                if (formCollection.Keys.Contains("hdnParticularArea") && !string.IsNullOrEmpty(formCollection["hdnParticularArea"])) model.ParticularArea = Convert.ToString(formCollection["hdnParticularArea"]);
            }
            else if (model.ReportType == "ONGCON")
            {
                if (formCollection.Keys.Contains("hdnStatusOffer") && !string.IsNullOrEmpty(formCollection["hdnStatusOffer"])) model.StatusOffer = Convert.ToString(formCollection["hdnStatusOffer"]);
                if (formCollection.Keys.Contains("hdnRegion") && !string.IsNullOrEmpty(formCollection["hdnRegion"])) model.Region = Convert.ToString(formCollection["hdnRegion"]);
                if (formCollection.Keys.Contains("hdnStatusOffertxt") && !string.IsNullOrEmpty(formCollection["hdnStatusOffertxt"])) model.StatusOffertxt = Convert.ToString(formCollection["hdnStatusOffertxt"]);
                if (formCollection.Keys.Contains("hdnRegiontxt") && !string.IsNullOrEmpty(formCollection["hdnRegiontxt"])) model.Regiontxt = Convert.ToString(formCollection["hdnRegiontxt"]);
                if (formCollection.Keys.Contains("hdnrdoregionwise") && !string.IsNullOrEmpty(formCollection["hdnrdoregionwise"])) model.rdoregionwise = Convert.ToString(formCollection["hdnrdoregionwise"]);
            }
            else if (model.ReportType == "CONTRACT")
            {
                if (formCollection.Keys.Contains("hdnFromDate") && !string.IsNullOrEmpty(formCollection["hdnFromDate"])) model.FromDate = Convert.ToDateTime(formCollection["hdnFromDate"]);
                if (formCollection.Keys.Contains("hdnToDate") && !string.IsNullOrEmpty(formCollection["hdnToDate"])) model.ToDate = Convert.ToDateTime(formCollection["hdnToDate"]);
                if (formCollection.Keys.Contains("hdnRegion") && !string.IsNullOrEmpty(formCollection["hdnRegion"])) model.Region = Convert.ToString(formCollection["hdnRegion"]);
                if (formCollection.Keys.Contains("hdnclientname") && !string.IsNullOrEmpty(formCollection["hdnclientname"])) model.clientname = Convert.ToString(formCollection["hdnclientname"]);
            }
            else if (model.ReportType == "CLUSVENDOR")
            {
                if (formCollection.Keys.Contains("hdndepartment") && !string.IsNullOrEmpty(formCollection["hdndepartment"])) model.department = Convert.ToString(formCollection["hdndepartment"]);
                if (formCollection.Keys.Contains("hdnallreport") && !string.IsNullOrEmpty(formCollection["hdnallreport"])) model.allreport = Convert.ToString(formCollection["hdnallreport"]);
                if (formCollection.Keys.Contains("hdndepartreport") && !string.IsNullOrEmpty(formCollection["hdndepartreport"])) model.reporttype = Convert.ToString(formCollection["hdndepartreport"]);
            }
            else if (model.ReportType == "VENDPER")
            {
                if (formCollection.Keys.Contains("hdnFromDate") && !string.IsNullOrEmpty(formCollection["hdnFromDate"])) model.FromDate = Convert.ToDateTime(formCollection["hdnFromDate"]);
                if (formCollection.Keys.Contains("hdnToDate") && !string.IsNullOrEmpty(formCollection["hdnToDate"])) model.ToDate = Convert.ToDateTime(formCollection["hdnToDate"]);
                if (formCollection.Keys.Contains("hdnformonth") && !string.IsNullOrEmpty(formCollection["hdnformonth"])) model.formonth = Convert.ToString(formCollection["hdnformonth"]);
                if (formCollection.Keys.Contains("hdnforperiod") && !string.IsNullOrEmpty(formCollection["hdnforperiod"])) model.forperiod = Convert.ToString(formCollection["hdnforperiod"]);
                if (formCollection.Keys.Contains("hdnmonth") && !string.IsNullOrEmpty(formCollection["hdnmonth"])) model.month = Convert.ToString(formCollection["hdnmonth"]);
                if (formCollection.Keys.Contains("hdnyear") && !string.IsNullOrEmpty(formCollection["hdnyear"])) model.year = Convert.ToString(formCollection["hdnyear"]);
                if (formCollection.Keys.Contains("hdnvendor") && !string.IsNullOrEmpty(formCollection["hdnvendor"])) model.vendor = Convert.ToString(formCollection["hdnvendor"]);
                if (formCollection.Keys.Contains("hdnvendcd") && !string.IsNullOrEmpty(formCollection["hdnvendcd"])) model.vendorcd = Convert.ToString(formCollection["hdnvendcd"]);
                if (formCollection.Keys.Contains("hdnmonthtxt") && !string.IsNullOrEmpty(formCollection["hdnmonthtxt"])) model.monthtxt = Convert.ToString(formCollection["hdnmonthtxt"]);
            }
            else if (model.ReportType == "CHECK" || model.ReportType == "TECH")
            {
                if (formCollection.Keys.Contains("hdnFromDatePWCHECK") && !string.IsNullOrEmpty(formCollection["hdnFromDatePWCHECK"])) model.FromDate = Convert.ToDateTime(formCollection["hdnFromDatePWCHECK"]);
                if (formCollection.Keys.Contains("hdnToDatePWCHECK") && !string.IsNullOrEmpty(formCollection["hdnToDatePWCHECK"])) model.ToDate = Convert.ToDateTime(formCollection["hdnToDatePWCHECK"]);
            }
            else if (model.ReportType == "U" || model.ReportType == "E")
            {
                if (formCollection.Keys.Contains("hdnddlienameUE") && !string.IsNullOrEmpty(formCollection["hdnddlienameUE"])) model.lstIE = Convert.ToString(formCollection["hdnddlienameUE"]);
                if (formCollection.Keys.Contains("hdnddlsupercmUE") && !string.IsNullOrEmpty(formCollection["hdnddlsupercmUE"])) model.lstCM = Convert.ToString(formCollection["hdnddlsupercmUE"]);
                if (formCollection.Keys.Contains("hdnFromDateUE") && !string.IsNullOrEmpty(formCollection["hdnFromDateUE"])) model.FromDate = Convert.ToDateTime(formCollection["hdnFromDateUE"]);
                if (formCollection.Keys.Contains("hdnToDateUE") && !string.IsNullOrEmpty(formCollection["hdnToDateUE"])) model.ToDate = Convert.ToDateTime(formCollection["hdnToDateUE"]);
                if (formCollection.Keys.Contains("hdnAllIEsUE") && !string.IsNullOrEmpty(formCollection["hdnAllIEsUE"])) model.AllIEs = Convert.ToString(formCollection["hdnAllIEsUE"]);
                if (formCollection.Keys.Contains("hdnParticularCMsUE") && !string.IsNullOrEmpty(formCollection["hdnParticularCMsUE"])) model.ParticularCMs = Convert.ToString(formCollection["hdnParticularCMsUE"]);
                if (formCollection.Keys.Contains("hdnParticularIEsUE") && !string.IsNullOrEmpty(formCollection["hdnParticularIEsUE"])) model.ParticularIEs = Convert.ToString(formCollection["hdnParticularIEsUE"]);
                if (formCollection.Keys.Contains("hdnIEWiseUE") && !string.IsNullOrEmpty(formCollection["hdnIEWiseUE"])) model.IEWise = Convert.ToString(formCollection["hdnIEWiseUE"]);
                if (formCollection.Keys.Contains("hdnCMWiseUE") && !string.IsNullOrEmpty(formCollection["hdnCMWiseUE"])) model.CMWise = Convert.ToString(formCollection["hdnCMWiseUE"]);
                if (formCollection.Keys.Contains("hdnSortedIEUE") && !string.IsNullOrEmpty(formCollection["hdnSortedIEUE"])) model.SortedIE = Convert.ToString(formCollection["hdnSortedIEUE"]);
                if (formCollection.Keys.Contains("hdnvisitdateUE") && !string.IsNullOrEmpty(formCollection["hdnvisitdateUE"])) model.visitdate = Convert.ToString(formCollection["hdnvisitdateUE"]);
            }
            else if (model.ReportType == "IEICPHOTO")
            {
                if (formCollection.Keys.Contains("hdnCaseNo") && !string.IsNullOrEmpty(formCollection["hdnCaseNo"])) model.CaseNo = Convert.ToString(formCollection["hdnCaseNo"]);
                if (formCollection.Keys.Contains("hdnCallRecDT") && !string.IsNullOrEmpty(formCollection["hdnCallRecDT"])) model.CallRecDT = Convert.ToString(formCollection["hdnCallRecDT"]);
                if (formCollection.Keys.Contains("hdnCallSno") && !string.IsNullOrEmpty(formCollection["hdnCallSno"])) model.CallSno = Convert.ToString(formCollection["hdnCallSno"]);
                if (formCollection.Keys.Contains("hdnBKNO") && !string.IsNullOrEmpty(formCollection["hdnBKNO"])) model.BKNO = Convert.ToString(formCollection["hdnBKNO"]);
                if (formCollection.Keys.Contains("hdnSETNO") && !string.IsNullOrEmpty(formCollection["hdnSETNO"])) model.SETNO = Convert.ToString(formCollection["hdnSETNO"]);
            }


            TempData[model.ReportType] = JsonConvert.SerializeObject(model);
            return RedirectToAction("Manage1", new { model.ReportType });
        }

        public IActionResult CoWiseIE()
        {
            ControllingOfficerIEModel model = otherReportsRepository.GetControllingOfficerWiseIE(Region);
            GlobalDeclaration.ControllingOfficerIE = model;
            return PartialView(model);
        }

        public IActionResult CoIeWiseCalls(string Case_No, string Call_Recv_Date, string Call_SNo)
        {
            CoIeWiseCallsModel model = otherReportsRepository.GetCoIeWiseCallsReport(Case_No, Call_Recv_Date, Call_SNo);
            GlobalDeclaration.CoIeWiseCalls = model;
            return PartialView(model);
        }

        public IActionResult TermsAndConditions()
        {
            return View();
        }
        public IActionResult NCRCWiseReport(string month, string year, string FromDate, string ToDate, string AllCM, string forCM, string All, string Outstanding, string formonth, string forperiod, string monthChar, string iecmname, string reporttype, string COName, string IENametext)
        {
            string Region = SessionHelper.UserModelDTO.Region;
            string wRegion = "";
            DateTime currentDateAndTime = DateTime.Now;
            if (Region == "N") { wRegion = "Northern Region"; }
            else if (Region == "S") { wRegion = "Southern Region"; }
            else if (Region == "E") { wRegion = "Eastern Region"; }
            else if (Region == "W") { wRegion = "Western Region"; }
            else if (Region == "C") { wRegion = "Central Region"; }
            NCRReport model = otherReportsRepository.GetNCRIECOWiseData(month, year, FromDate, ToDate, AllCM, forCM, All, Outstanding, formonth, forperiod, Region, iecmname, reporttype);
            model.Regions = wRegion;
            model.Todaydate = currentDateAndTime;
            model.reporttype = reporttype;
            model.IENametext = IENametext;
            model.COName = COName;
            model.monthChar = monthChar;
            GlobalDeclaration.NCRReports = model;
            return PartialView(model);
        }

        public IActionResult IEWiseTrainingReport(string IENAME, string TrainingArea, string Mechanical, string Electrical, string Civil, string Regular, string Deputaion, string Particularie, string ParticularArea)
        {
            string Region = SessionHelper.UserModelDTO.Region;
            string wRegion = "";
            if (Region == "N") { wRegion = "Northern Region"; }
            else if (Region == "S") { wRegion = "Southern Region"; }
            else if (Region == "E") { wRegion = "Eastern Region"; }
            else if (Region == "W") { wRegion = "Western Region"; }
            else if (Region == "C") { wRegion = "Central Region"; }
            IEWiseTrainingReportModel model = otherReportsRepository.GetIEWiseTrainingDetails(IENAME, TrainingArea, Mechanical, Electrical, Civil, Regular, Deputaion, Particularie, ParticularArea, Region);
            model.Regions = wRegion;
            GlobalDeclaration.IEWiseTrainingReport = model;
            return PartialView(model);
        }

        public IActionResult OngoingContractReport(string StatusOffer, string Region, string StatusOffertxt, string Regiontxt, string rdoregionwise)
        {
            OngoingContrcatsReportModel model = otherReportsRepository.Getongoingcontractdetails(StatusOffer, Region, StatusOffertxt, Regiontxt, rdoregionwise);
            GlobalDeclaration.OngoingContrcatsReport = model;
            model.Region = Regiontxt;
            model.StatusOffertxt = StatusOffertxt;
            return PartialView(model);
        }

        public IActionResult ContractReport(string FromDate, string ToDate, string Region, string clientname)
        {
            ContractReportModel model = otherReportsRepository.GetContractDetails(FromDate, ToDate, Region, clientname);
            GlobalDeclaration.ContractReport = model;
            return PartialView(model);
        }

        public IActionResult VendorClusterIEReport(string department)
        {
            string Region = SessionHelper.UserModelDTO.Region;
            //string wRegion = "";
            //if (Region == "N") { wRegion = "Northern Region"; }
            //else if (Region == "S") { wRegion = "Southern Region"; }
            //else if (Region == "E") { wRegion = "Eastern Region"; }
            //else if (Region == "W") { wRegion = "Western Region"; }
            //else if (Region == "C") { wRegion = "Central Region"; }
            VendorClusterReportModel model = otherReportsRepository.GetVendorClusterReport(department, Region);
            GlobalDeclaration.VendorClusterReport = model;
            return PartialView(model);
        }

        public IActionResult IEAlterMappingReport()
        {
            string Region = SessionHelper.UserModelDTO.Region;
            //string wRegion = "";
            //if (Region == "N") { wRegion = "Northern Region"; }
            //else if (Region == "S") { wRegion = "Southern Region"; }
            //else if (Region == "E") { wRegion = "Eastern Region"; }
            //else if (Region == "W") { wRegion = "Western Region"; }
            //else if (Region == "C") { wRegion = "Central Region"; }
            IEAlterMappingReportModel model = otherReportsRepository.GetIEAlterMappingReport(Region);
            GlobalDeclaration.IEAlterMappingReport = model;
            return PartialView(model);
        }

        public IActionResult VendorPerforeport(string FromDate, string ToDate, string formonth, string forperiod, string month, string year, string vendcd, string vendor, string monthtxt)
        {
            string Region = SessionHelper.UserModelDTO.Region;
            string wRegion = "";
            if (Region == "N") { wRegion = "Northern Region"; }
            else if (Region == "S") { wRegion = "Southern Region"; }
            else if (Region == "E") { wRegion = "Eastern Region"; }
            else if (Region == "W") { wRegion = "Western Region"; }
            else if (Region == "C") { wRegion = "Central Region"; }
            VendorPerformanceReportModel model = otherReportsRepository.GetVendorperformanceReport(FromDate, ToDate, formonth, forperiod, month, year, vendcd, Region);
            GlobalDeclaration.VendorPerformanceReport = model;
            model.vendor = vendor;
            model.FromDate = Convert.ToDateTime(FromDate).ToString("dd/MM/yyyy");
            model.ToDate = Convert.ToDateTime(ToDate).ToString("dd/MM/yyyy");
            model.Region = wRegion;
            model.monthtxt = monthtxt;
            model.year = year;
            model.Region = DateTime.Now.ToString("dd-MM-yyyy");
            return PartialView(model);
        }

        public IActionResult VendorFeedback()
        {
            string Region = SessionHelper.UserModelDTO.Region;
            string wRegion = "";
            if (Region == "N") { wRegion = "Northern Region"; }
            else if (Region == "S") { wRegion = "Southern Region"; }
            else if (Region == "E") { wRegion = "Eastern Region"; }
            else if (Region == "W") { wRegion = "Western Region"; }
            else if (Region == "C") { wRegion = "Central Region"; }
            VendorFeedbackReportModel model = otherReportsRepository.GetVendorFeedbackReport(Region);
            model.Regions = wRegion;
            GlobalDeclaration.VendorFeedbackReport = model;
            return PartialView(model);
        }

        public IActionResult PeriodWiseChecksheet(string FromDate, string ToDate)
        {
            string Region = SessionHelper.UserModelDTO.Region;
            string wRegion = "";
            if (Region == "N") { wRegion = "Northern Region"; }
            else if (Region == "S") { wRegion = "Southern Region"; }
            else if (Region == "E") { wRegion = "Eastern Region"; }
            else if (Region == "W") { wRegion = "Western Region"; }
            else if (Region == "C") { wRegion = "Central Region"; }
            PeriodWiseChecksheetReportModel model = otherReportsRepository.Getperiodwisechecksheetdetails(FromDate, ToDate, Region);
            model.FromDate = Convert.ToDateTime(FromDate).ToString("dd/MM/yyyy");
            model.ToDate = Convert.ToDateTime(ToDate).ToString("dd/MM/yyyy");
            model.Regions = wRegion;
            GlobalDeclaration.PeriodWiseChecksheetReport = model;
            return PartialView(model);
        }

        public IActionResult PeriodWiseTechRef(string FromDate, string ToDate)
        {
            string Region = SessionHelper.UserModelDTO.Region;
            string wRegion = "";
            if (Region == "N") { wRegion = "Northern Region"; }
            else if (Region == "S") { wRegion = "Southern Region"; }
            else if (Region == "E") { wRegion = "Eastern Region"; }
            else if (Region == "W") { wRegion = "Western Region"; }
            else if (Region == "C") { wRegion = "Central Region"; }
            PeriodWiseTechnicalRefReportModel model = otherReportsRepository.Getperiodwisetechrefdetails(FromDate, ToDate, Region);
            model.FromDate = Convert.ToDateTime(FromDate).ToString("dd/MM/yyyy");
            model.ToDate = Convert.ToDateTime(ToDate).ToString("dd/MM/yyyy");
            model.Regions = wRegion;
            GlobalDeclaration.PeriodWiseTechnicalRefReport = model;
            return PartialView(model);
        }

        public IActionResult DailyWorkIECMReport(string FromDate, string ToDate, string lstIE, string lstCM, string AllIEs, string ParticularIEs, string AllCM, string ParticularCMs, string ReportType, string IEWise, string CMWise, string SortedIE, string visitdate)
        {
            string wRegion = "";
            if (Region == "N") { wRegion = "Northern Region"; }
            else if (Region == "S") { wRegion = "Southern Region"; }
            else if (Region == "E") { wRegion = "Eastern Region"; }
            else if (Region == "W") { wRegion = "Western Region"; }
            else if (Region == "C") { wRegion = "Central Region"; }
            DailyIECMWorkPlanReportModel model = otherReportsRepository.GetDailyWorkData(FromDate, ToDate, lstIE, lstCM, AllIEs, ParticularIEs, AllCM, ParticularCMs, ReportType, IEWise, CMWise, Region, SortedIE, visitdate);
            GlobalDeclaration.DailyIECMWorkPlanReport = model;
            model.FromDate = Convert.ToDateTime(FromDate).ToString("dd/MM/yyyy");
            model.ToDate = Convert.ToDateTime(ToDate).ToString("dd/MM/yyyy");
            model.ReportType = ReportType;
            model.Regions = wRegion;
            return PartialView(model);
        }

        public IActionResult DailyWorkIEExcepReport(string FromDate, string ToDate, string lstIE, string lstCM, string AllIEs, string ParticularIEs, string AllCM, string ParticularCMs, string ReportType, string IEWise, string CMWise, string SortedIE, string visitdate)
        {
            string wRegion = "";
            if (Region == "N") { wRegion = "Northern Region"; }
            else if (Region == "S") { wRegion = "Southern Region"; }
            else if (Region == "E") { wRegion = "Eastern Region"; }
            else if (Region == "W") { wRegion = "Western Region"; }
            else if (Region == "C") { wRegion = "Central Region"; }
            DailyIECMWorkPlanReportModel model = otherReportsRepository.GetDailyWorkData(FromDate, ToDate, lstIE, lstCM, AllIEs, ParticularIEs, AllCM, ParticularCMs, ReportType, IEWise, CMWise, Region, SortedIE, visitdate);
            GlobalDeclaration.DailyIECMWorkPlanReport = model;
            model.FromDate = Convert.ToDateTime(FromDate).ToString("dd/MM/yyyy");
            model.ToDate = Convert.ToDateTime(ToDate).ToString("dd/MM/yyyy");
            model.ReportType = ReportType;
            model.Regions = wRegion;
            return PartialView(model);
        }

        public IActionResult PhotoSubmiteedByIE(string CaseNo, string CallRecDT, string CallSno, string BKNO, string SETNO)
        {
            string wRegion = "";
            if (Region == "N") { wRegion = "Northern Region"; }
            else if (Region == "S") { wRegion = "Southern Region"; }
            else if (Region == "E") { wRegion = "Eastern Region"; }
            else if (Region == "W") { wRegion = "Western Region"; }
            else if (Region == "C") { wRegion = "Central Region"; }
            IEICPhotoEnclosedModelReport model = otherReportsRepository.GetDataListReport(CaseNo, CallRecDT, CallSno, BKNO, SETNO, Region);
            model.Regions = wRegion;
            GlobalDeclaration.IEICPhotoEnclosedModel = model;
            return PartialView(model);
        }

        #region Other Event
        [HttpGet]
        public IActionResult GetIE(string CO)
        {
            try
            {
                List<SelectListItem> lstAu = Common.GetControllingSelectedIE(Region, CO);
                return Json(new { status = true, list = lstAu });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "BillingReports", "GetAU", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
        #endregion

        [HttpPost]
        public IActionResult Get_CoIeWiseCalls([FromBody] DTParameters dTParameters)
        {
            DTResult<CoIeWiseCallsListModel> dtResult = new();
            //IE = IE == "" ? null : IE;
            dtResult = otherReportsRepository.GetCoIeWiseCalls(dTParameters);
            var data = dtResult.data.ToList();
            for (int i = 0; i < data.Count(); i++)
            {
                var row = data[i];
                var CallDocPath = env.WebRootPath + Enums.GetEnumDescription(Enums.FolderPath.Vendor) + "/CALLS_DOCUMENTS" + row.CASE_NO + "-" + row.CALL_RECV_DT.Substring(6, 4) + row.CALL_RECV_DT.Substring(3, 2) + row.CALL_RECV_DT.Substring(0, 2) + row.CASE_NO + ".pdf";
                row.IsCallDocument = System.IO.File.Exists(CallDocPath) == true ? true : false;

                if (row.PO_SOURCE != "C")
                {
                    var tifPath = env.WebRootPath + Enums.GetEnumDescription(Enums.FolderPath.CaseNo) + "/" + row.CASE_NO + ".TIF";
                    var pdfPath = env.WebRootPath + Enums.GetEnumDescription(Enums.FolderPath.CaseNo) + "/" + row.CASE_NO + ".PDF";
                    row.IsCaseNoTif = System.IO.File.Exists(tifPath) == true ? true : false;
                    row.IsCaseNoPdf = System.IO.File.Exists(pdfPath) == true ? true : false;
                }

                if (row.CALL_STATUS == "U")
                {
                    string MyFile_ex;
                    var mdt_ex = Common.DateConcate(row.CALL_RECV_DT);
                    MyFile_ex = row.CASE_NO.Trim() + "_" + row.CALL_SNO.Trim() + "_" + mdt_ex;
                    row.MyFile_ex = MyFile_ex;
                    string fpath = env.WebRootPath + Enums.GetEnumDescription(Enums.FolderPath.Lab) + "/" + MyFile_ex + ".PDF";
                    row.IsLabPdf = System.IO.File.Exists(fpath) == true ? true : false;
                }
            }
            dtResult.data = data.AsQueryable();
            return Json(dtResult);
        }

        [HttpPost]
        public async Task<IActionResult> GeneratePDF(string ReportType)
        {
            string htmlContent = string.Empty;

            if (ReportType == "COWISEIE")
            {
                ControllingOfficerIEModel model = GlobalDeclaration.ControllingOfficerIE;
                htmlContent = await this.RenderViewToStringAsync("/Views/OtherReports/CoWiseIE.cshtml", model);
            }
            else if (ReportType == "COIEWiseCalls")
            {
                CoIeWiseCallsModel model = GlobalDeclaration.CoIeWiseCalls;
                htmlContent = await this.RenderViewToStringAsync("/Views/OtherReports/CoIeWiseCalls.cshtml", model);
            }
            else if (ReportType == "C" || ReportType == "I")
            {
                NCRReport model = GlobalDeclaration.NCRReports;
                htmlContent = await this.RenderViewToStringAsync("/Views/OtherReports/NCRCWiseReport.cshtml", model);
            }
            else if (ReportType == "IEWISET")
            {
                IEWiseTrainingReportModel model = GlobalDeclaration.IEWiseTrainingReport;
                htmlContent = await this.RenderViewToStringAsync("/Views/OtherReports/IEWiseTrainingReport.cshtml", model);
            }
            else if (ReportType == "ONGCON")
            {
                OngoingContrcatsReportModel model = GlobalDeclaration.OngoingContrcatsReport;
                htmlContent = await this.RenderViewToStringAsync("/Views/OtherReports/OngoingContractReport.cshtml", model);
            }
            else if (ReportType == "CONTRACT")
            {
                ContractReportModel model = GlobalDeclaration.ContractReport;
                htmlContent = await this.RenderViewToStringAsync("/Views/OtherReports/ContractReport.cshtml", model);
            }
            else if (ReportType == "CLUSVENDOR")
            {
                VendorClusterReportModel model = GlobalDeclaration.VendorClusterReport;
                htmlContent = await this.RenderViewToStringAsync("/Views/OtherReports/VendorClusterIEReport.cshtml", model);
            }
            else if (ReportType == "IEALTER")
            {
                IEAlterMappingReportModel model = GlobalDeclaration.IEAlterMappingReport;
                htmlContent = await this.RenderViewToStringAsync("/Views/OtherReports/IEAlterMappingReport.cshtml", model);
            }
            else if (ReportType == "VENDPER")
            {
                VendorPerformanceReportModel model = GlobalDeclaration.VendorPerformanceReport;
                htmlContent = await this.RenderViewToStringAsync("/Views/OtherReports/VendorPerforeport.cshtml", model);
            }
            else if (ReportType == "VENDFEED")
            {
                VendorFeedbackReportModel model = GlobalDeclaration.VendorFeedbackReport;
                htmlContent = await this.RenderViewToStringAsync("/Views/OtherReports/VendorFeedback.cshtml", model);
            }
            else if (ReportType == "CHECK")
            {
                PeriodWiseChecksheetReportModel model = GlobalDeclaration.PeriodWiseChecksheetReport;
                htmlContent = await this.RenderViewToStringAsync("/Views/OtherReports/PeriodWiseChecksheet.cshtml", model);
            }
            else if (ReportType == "TECH")
            {
                PeriodWiseTechnicalRefReportModel model = GlobalDeclaration.PeriodWiseTechnicalRefReport;
                htmlContent = await this.RenderViewToStringAsync("/Views/OtherReports/PeriodWiseTechRef.cshtml", model);
            }
            else if (ReportType == "U")
            {
                DailyIECMWorkPlanReportModel model = GlobalDeclaration.DailyIECMWorkPlanReport;
                htmlContent = await this.RenderViewToStringAsync("/Views/OtherReports/DailyWorkIECMReport.cshtml", model);
            }
            else if (ReportType == "E")
            {
                DailyIECMWorkPlanReportModel model = GlobalDeclaration.DailyIECMWorkPlanReport;
                htmlContent = await this.RenderViewToStringAsync("/Views/OtherReports/DailyWorkIEExcepReport.cshtml", model);
            }
            else if (ReportType == "IEICPHOTO")
            {
                IEICPhotoEnclosedModelReport model = GlobalDeclaration.IEICPhotoEnclosedModel;
                htmlContent = await this.RenderViewToStringAsync("/Views/OtherReports/PhotoSubmiteedByIE.cshtml", model);
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
