using IBS.Filters;
using IBS.Interfaces.Reports;
using IBS.Models.Reports;
using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp.Media;
using PuppeteerSharp;
using IBS.Helper;
using IBS.Models;
using Newtonsoft.Json;

namespace IBS.Controllers.Reports
{
    [Authorization]
    public class ManagementReportsController : BaseController
    {
        private readonly IManagementReportsRepository managementReportsRepository;
        private readonly IWebHostEnvironment env;

        public ManagementReportsController(IManagementReportsRepository _managementReportsRepository, IWebHostEnvironment _env)
        {
            managementReportsRepository = _managementReportsRepository;
            this.env = _env;
        }

        public IActionResult Index()
        {
            ViewBag.Region = Region;
            return View();
        }

        public IActionResult Manage(string ReportType)
        {
            ManagementReportsModel model = new();

            if (TempData.ContainsKey(ReportType))
            {
                model = JsonConvert.DeserializeObject<ManagementReportsModel>(TempData.Peek(ReportType).ToString());
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult ManageReportData(IFormCollection formCollection)
        {
            ManagementReportsModel model = new();

            if (formCollection.Keys.Contains("hdnReportType")) model.ReportType = formCollection["hdnReportType"];
            model.ReportTitle = EnumUtility<Enums.ManagementReportsTitle>.GetDescriptionByKey(model.ReportType);

            if (model.ReportType == "IE_X" || model.ReportType == "CLUSTER_X" || model.ReportType == "ICSUBMIT" || model.ReportType == "CALLSWITHOUTIC" || model.ReportType == "SUPSURPSUMM" || model.ReportType == "PENDING_CALLS" || model.ReportType == "COUNTIC" || model.ReportType == "CALL_DETAILS")
            {
                if (formCollection.Keys.Contains("hdnFromDate") && !string.IsNullOrEmpty(formCollection["hdnFromDate"])) model.FromDate = Convert.ToDateTime(formCollection["hdnFromDate"]);
                if (formCollection.Keys.Contains("hdnToDate") && !string.IsNullOrEmpty(formCollection["hdnToDate"])) model.ToDate = Convert.ToDateTime(formCollection["hdnToDate"]);
            }
            else if (model.ReportType == "RWB")
            {
                if (formCollection.Keys.Contains("hdnFromYearMonth") && !string.IsNullOrEmpty(formCollection["hdnFromYearMonth"])) model.FromYearMonth = Convert.ToString(formCollection["hdnFromYearMonth"]);
                if (formCollection.Keys.Contains("hdnToYearMonth") && !string.IsNullOrEmpty(formCollection["hdnToYearMonth"])) model.ToYearMonth = Convert.ToString(formCollection["hdnToYearMonth"]);
            }
            else if (model.ReportType == "R")
            {
                if (formCollection.Keys.Contains("hdnFromDate") && !string.IsNullOrEmpty(formCollection["hdnFromDate"])) model.FromDate = Convert.ToDateTime(formCollection["hdnFromDate"]);
                if (formCollection.Keys.Contains("hdnOutstanding") && !string.IsNullOrEmpty(formCollection["hdnOutstanding"])) model.Outstanding = Convert.ToString(formCollection["hdnOutstanding"]);
            }
            else if (model.ReportType == "SUPSUR")
            {
                if (formCollection.Keys.Contains("hdnFromDate") && !string.IsNullOrEmpty(formCollection["hdnFromDate"])) model.FromDate = Convert.ToDateTime(formCollection["hdnFromDate"]);
                if (formCollection.Keys.Contains("hdnToDate") && !string.IsNullOrEmpty(formCollection["hdnToDate"])) model.ToDate = Convert.ToDateTime(formCollection["hdnToDate"]);
                if (formCollection.Keys.Contains("hdnParticularCM") && !string.IsNullOrEmpty(formCollection["hdnParticularCM"])) model.ParticularCM = Convert.ToString(formCollection["hdnParticularCM"]);
                if (formCollection.Keys.Contains("hdnParticularSector") && !string.IsNullOrEmpty(formCollection["hdnParticularSector"])) model.ParticularSector = Convert.ToString(formCollection["hdnParticularSector"]);
            }
            else if (model.ReportType == "CONSIGN_REJECT")
            {
                if (formCollection.Keys.Contains("hdnFromDate") && !string.IsNullOrEmpty(formCollection["hdnFromDate"])) model.FromDate = Convert.ToDateTime(formCollection["hdnFromDate"]);
                if (formCollection.Keys.Contains("hdnToDate") && !string.IsNullOrEmpty(formCollection["hdnToDate"])) model.ToDate = Convert.ToDateTime(formCollection["hdnToDate"]);
                if (formCollection.Keys.Contains("hdnRegion") && !string.IsNullOrEmpty(formCollection["hdnRegion"])) model.Region = Convert.ToString(formCollection["hdnRegion"]);
                if (formCollection.Keys.Contains("hdnStatus") && !string.IsNullOrEmpty(formCollection["hdnStatus"])) model.Status = Convert.ToString(formCollection["hdnStatus"]);
            }
            else if (model.ReportType == "X")
            {
                if (formCollection.Keys.Contains("hdnFromDate") && !string.IsNullOrEmpty(formCollection["hdnFromDate"])) model.FromDate = Convert.ToDateTime(formCollection["hdnFromDate"]);
                model.Region = Region;
            }
            else if (model.ReportType == "CLIENTWISEREJ")
            {
                if (formCollection.Keys.Contains("hdnFromDate") && !string.IsNullOrEmpty(formCollection["hdnFromDate"])) model.FromDate = Convert.ToDateTime(formCollection["hdnFromDate"]);
                if (formCollection.Keys.Contains("hdnToDate") && !string.IsNullOrEmpty(formCollection["hdnToDate"])) model.ToDate = Convert.ToDateTime(formCollection["hdnToDate"]);
                if (formCollection.Keys.Contains("hdnClientType") && !string.IsNullOrEmpty(formCollection["hdnClientType"])) model.ClientType = Convert.ToString(formCollection["hdnClientType"]);
                if (formCollection.Keys.Contains("hdnBPORailway") && !string.IsNullOrEmpty(formCollection["hdnBPORailway"])) model.BPORailway = Convert.ToString(formCollection["hdnBPORailway"]);
            }
            else if (model.ReportType == "NON_CONFORMITY")
            {
                if (formCollection.Keys.Contains("hdnFromYearMonth") && !string.IsNullOrEmpty(formCollection["hdnFromYearMonth"])) model.FromYearMonth = Convert.ToString(formCollection["hdnFromYearMonth"]);
                if (formCollection.Keys.Contains("hdnToYearMonth") && !string.IsNullOrEmpty(formCollection["hdnToYearMonth"])) model.ToYearMonth = Convert.ToString(formCollection["hdnToYearMonth"]);
                if (formCollection.Keys.Contains("hdnIeCd") && !string.IsNullOrEmpty(formCollection["hdnIeCd"])) model.IeCd = Convert.ToInt32(formCollection["hdnIeCd"]);
            }
            else if (model.ReportType == "HIGHVALUE")
            {
                if (formCollection.Keys.Contains("hdnFromDate") && !string.IsNullOrEmpty(formCollection["hdnFromDate"])) model.FromDate = Convert.ToDateTime(formCollection["hdnFromDate"]);
                if (formCollection.Keys.Contains("hdnToDate") && !string.IsNullOrEmpty(formCollection["hdnToDate"])) model.ToDate = Convert.ToDateTime(formCollection["hdnToDate"]);
                if (formCollection.Keys.Contains("hdnParticularCM") && !string.IsNullOrEmpty(formCollection["hdnParticularCM"])) model.ParticularCM = Convert.ToString(formCollection["hdnParticularCM"]);
                if (formCollection.Keys.Contains("hdnSortedOn") && !string.IsNullOrEmpty(formCollection["hdnSortedOn"])) model.SortedOn = Convert.ToString(formCollection["hdnSortedOn"]);
            }
            else if (model.ReportType == "REMARKING")
            {
                if (formCollection.Keys.Contains("hdnFromDate") && !string.IsNullOrEmpty(formCollection["hdnFromDate"])) model.FromDate = Convert.ToDateTime(formCollection["hdnFromDate"]);
                if (formCollection.Keys.Contains("hdnToDate") && !string.IsNullOrEmpty(formCollection["hdnToDate"])) model.ToDate = Convert.ToDateTime(formCollection["hdnToDate"]);
                if (formCollection.Keys.Contains("hdnCallRemarkingDate") && !string.IsNullOrEmpty(formCollection["hdnCallRemarkingDate"])) model.CallRemarkingDate = Convert.ToString(formCollection["hdnCallRemarkingDate"]);
                if (formCollection.Keys.Contains("hdnCallsStatus") && !string.IsNullOrEmpty(formCollection["hdnCallsStatus"])) model.CallsStatus = Convert.ToString(formCollection["hdnCallsStatus"]);
            }

            TempData[model.ReportType] = JsonConvert.SerializeObject(model);
            return RedirectToAction("Manage", new { model.ReportType });

        }

        public IActionResult IEPerformance(DateTime FromDate, DateTime ToDate)
        {
            int IeCd = 0;
            if (SessionHelper.UserModelDTO.RoleName.ToLower() == "inspection engineer (ie)")
            {
                IeCd = SessionHelper.UserModelDTO.IeCd;
            }
            IEPerformanceModel model = managementReportsRepository.GetIEPerformanceData(FromDate, ToDate, Region, IeCd);
            GlobalDeclaration.IEPerformance = model;
            return PartialView(model);
        }

        public IActionResult ClusterPerformance(DateTime FromDate, DateTime ToDate)
        {
            ClusterPerformanceModel model = managementReportsRepository.GetClusterPerformanceData(FromDate, ToDate, Region);
            GlobalDeclaration.ClusterPerformance = model;
            return PartialView(model);
        }

        public IActionResult RegionWiseBillingSummary(string FromYearMonth, string ToYearMonth)
        {
            RWBSummaryModel model = managementReportsRepository.GetRWBSummaryData(FromYearMonth, ToYearMonth);
            GlobalDeclaration.RWBSummary = model;
            return PartialView(model);
        }

        public IActionResult RegionWiseComparisonOutstanding(DateTime FromDate, string Outstanding)
        {
            RWCOModel model = managementReportsRepository.GetRWCOData(FromDate, Outstanding);
            GlobalDeclaration.RWCO = model;
            return PartialView(model);
        }

        public IActionResult ICSubmission(DateTime FromDate, DateTime ToDate)
        {
            ICSubmissionModel model = managementReportsRepository.GetICSubmissionData(FromDate, ToDate, Region);
            GlobalDeclaration.ICSubmission = model;
            return PartialView(model);
        }

        public IActionResult PendingICAgainstCalls(DateTime FromDate, DateTime ToDate)
        {
            PendingICAgainstCallsModel model = managementReportsRepository.GetPendingICAgainstCallsData(FromDate, ToDate, Region);
            GlobalDeclaration.PendingICAgainstCalls = model;
            return PartialView(model);
        }

        public IActionResult SuperSurpriseDetails(DateTime FromDate, DateTime ToDate, string ParticularCM, string ParticularSector)
        {
            SuperSurpriseDetailsModel model = managementReportsRepository.GetSuperSurpriseDetailsData(FromDate, ToDate, Region, ParticularCM, ParticularSector);
            GlobalDeclaration.SuperSurpriseDetails = model;
            return PartialView(model);
        }

        public IActionResult SuperSurpriseSummary(DateTime FromDate, DateTime ToDate)
        {
            SuperSurpriseSummaryModel model = managementReportsRepository.GetSuperSurpriseSummaryData(FromDate, ToDate, Region);
            GlobalDeclaration.SuperSurpriseSummary = model;
            return PartialView(model);
        }

        public IActionResult ConsignReject(DateTime FromDate, DateTime ToDate, string InspRegion, string Status)
        {
            ConsignRejectModel model = managementReportsRepository.GetConsignRejectData(FromDate, ToDate, Region, InspRegion, Status);
            GlobalDeclaration.ConsignReject = model;
            return PartialView(model);
        }

        public IActionResult OutstandingOverRegion(DateTime FromDate)
        {
            OutstandingOverRegionModel model = managementReportsRepository.GetOutstandingOverRegion(FromDate);
            GlobalDeclaration.OutstandingOverRegion = model;
            return PartialView(model);
        }

        public IActionResult ClientWiseRejection(DateTime FromDate, DateTime ToDate, string ClientType, string BPORailway)
        {
            ClientWiseRejectionModel model = managementReportsRepository.GetClientWiseRejection(FromDate, ToDate, ClientType, BPORailway);
            GlobalDeclaration.ClientWiseRejection = model;
            return PartialView(model);
        }

        public IActionResult NonConformity(string FromYearMonth, string ToYearMonth, int IeCd)
        {
            NonConformityModel model = managementReportsRepository.GetNonConformityData(FromYearMonth, ToYearMonth, IeCd);
            GlobalDeclaration.NonConformity = model;
            return PartialView(model);
        }

        public IActionResult PendingCalls()
        {
            PendingCallsModel model = managementReportsRepository.GetPendingCallsData();
            GlobalDeclaration.PendingCalls = model;
            return PartialView(model);
        }

        public IActionResult ICIssuedNotReceived(DateTime FromDate, DateTime ToDate)
        {
            ICIssuedNotReceivedModel model = managementReportsRepository.GetICIssuedNotReceived(FromDate, ToDate, Region);
            GlobalDeclaration.ICIssuedNotReceived = model;
            return PartialView(model);
        }

        public IActionResult TentativeInspectionFeeWisePendingCalls(DateTime FromDate, DateTime ToDate, string ParticularCM, string SortedOn)
        {
            TentativeInspectionFeeWisePendingCallsModel model = managementReportsRepository.GetTentativeInspectionFeeWisePendingCalls(FromDate, ToDate, Region, ParticularCM, SortedOn);
            GlobalDeclaration.TentativeInspectionFeeWisePendingCalls = model;
            return PartialView(model);
        }

        public IActionResult CallRemarking(DateTime FromDate, DateTime ToDate, string CallRemarkingDate, string CallsStatus)
        {
            Models.Reports.CallRemarkingModel model = managementReportsRepository.GetCallRemarkingData(FromDate, ToDate, Region, CallRemarkingDate, CallsStatus);
            GlobalDeclaration.CallRemarking = model;
            return PartialView(model);
        }

        public IActionResult CallDetailsDashborad()
        {
            CallDetailsDashboradModel model = managementReportsRepository.GetCallDetailsDashborad(Region);
            GlobalDeclaration.CallDetailsDashborad = model;
            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> GeneratePDF(string ReportType)
        {
            string htmlContent = string.Empty;

            if (ReportType == "IE_X")
            {
                IEPerformanceModel model = GlobalDeclaration.IEPerformance;
                htmlContent = await this.RenderViewToStringAsync("/Views/ManagementReports/IEPerformance.cshtml", model);
            }
            else if (ReportType == "CLUSTER_X")
            {
                ClusterPerformanceModel model = GlobalDeclaration.ClusterPerformance;
                htmlContent = await this.RenderViewToStringAsync("/Views/ManagementReports/ClusterPerformance.cshtml", model);
            }
            else if (ReportType == "RWB")
            {
                RWBSummaryModel model = GlobalDeclaration.RWBSummary;
                htmlContent = await this.RenderViewToStringAsync("/Views/ManagementReports/RegionWiseBillingSummary.cshtml", model);
            }
            else if (ReportType == "R")
            {
                RWCOModel model = GlobalDeclaration.RWCO;
                htmlContent = await this.RenderViewToStringAsync("/Views/ManagementReports/RegionWiseComparisonOutstanding.cshtml", model);
            }
            else if (ReportType == "ICSUBMIT")
            {
                ICSubmissionModel model = GlobalDeclaration.ICSubmission;
                htmlContent = await this.RenderViewToStringAsync("/Views/ManagementReports/ICSubmission.cshtml", model);
            }
            else if (ReportType == "CALLSWITHOUTIC")
            {
                PendingICAgainstCallsModel model = GlobalDeclaration.PendingICAgainstCalls;
                htmlContent = await this.RenderViewToStringAsync("/Views/ManagementReports/PendingICAgainstCalls.cshtml", model);
            }
            else if (ReportType == "SUPSUR")
            {
                SuperSurpriseDetailsModel model = GlobalDeclaration.SuperSurpriseDetails;
                htmlContent = await this.RenderViewToStringAsync("/Views/ManagementReports/SuperSurpriseDetails.cshtml", model);
            }
            else if (ReportType == "SUPSURPSUMM")
            {
                SuperSurpriseSummaryModel model = GlobalDeclaration.SuperSurpriseSummary;
                htmlContent = await this.RenderViewToStringAsync("/Views/ManagementReports/SuperSurpriseSummary.cshtml", model);
            }
            else if (ReportType == "CONSIGN_REJECT")
            {
                ConsignRejectModel model = GlobalDeclaration.ConsignReject;
                htmlContent = await this.RenderViewToStringAsync("/Views/ManagementReports/ConsignReject.cshtml", model);
            }
            else if (ReportType == "X")
            {
                OutstandingOverRegionModel model = GlobalDeclaration.OutstandingOverRegion;
                htmlContent = await this.RenderViewToStringAsync("/Views/ManagementReports/OutstandingOverRegion.cshtml", model);
            }
            else if (ReportType == "CLIENTWISEREJ")
            {
                ClientWiseRejectionModel model = GlobalDeclaration.ClientWiseRejection;
                htmlContent = await this.RenderViewToStringAsync("/Views/ManagementReports/ClientWiseRejection.cshtml", model);
            }
            else if (ReportType == "NON_CONFORMITY")
            {
                NonConformityModel model = GlobalDeclaration.NonConformity;
                htmlContent = await this.RenderViewToStringAsync("/Views/ManagementReports/NonConformity.cshtml", model);
            }
            else if (ReportType == "PENDING_CALLS")
            {
                PendingCallsModel model = GlobalDeclaration.PendingCalls;
                htmlContent = await this.RenderViewToStringAsync("/Views/ManagementReports/PendingCalls.cshtml", model);
            }
            else if (ReportType == "COUNTIC")
            {
                ICIssuedNotReceivedModel model = GlobalDeclaration.ICIssuedNotReceived;
                htmlContent = await this.RenderViewToStringAsync("/Views/ManagementReports/ICIssuedNotReceived.cshtml", model);
            }
            else if (ReportType == "HIGHVALUE")
            {
                TentativeInspectionFeeWisePendingCallsModel model = GlobalDeclaration.TentativeInspectionFeeWisePendingCalls;
                htmlContent = await this.RenderViewToStringAsync("/Views/ManagementReports/TentativeInspectionFeeWisePendingCalls.cshtml", model);
            }
            else if (ReportType == "REMARKING")
            {
                Models.Reports.CallRemarkingModel model = GlobalDeclaration.CallRemarking;
                htmlContent = await this.RenderViewToStringAsync("/Views/ManagementReports/CallRemarking.cshtml", model);
            }
            else if (ReportType == "CALL_DETAILS")
            {
                CallDetailsDashboradModel model = GlobalDeclaration.CallDetailsDashborad;
                htmlContent = await this.RenderViewToStringAsync("/Views/ManagementReports/CallDetailsDashborad.cshtml", model);
            }

            await new BrowserFetcher().DownloadAsync();
            await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
                DefaultViewport = null,
                //ExecutablePath = "C:\\Program Files (x86)\\Google\\Chrome\\Application\\chrome.exe"
            }).ConfigureAwait(false);

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

        public IActionResult GetBPORailway(string ClientType)
        {
            return Json(Common.GetBPORailway(ClientType).ToList());
        }

    }
}