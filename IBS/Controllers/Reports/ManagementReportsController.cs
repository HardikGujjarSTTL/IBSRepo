using IBS.Filters;
using IBS.Interfaces.Reports;
using IBS.Models.Reports;
using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp.Media;
using PuppeteerSharp;
using IBS.Helper;
using IBS.Models;

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

        public IActionResult Manage(string ReportType, DateTime FromDate, DateTime ToDate)
        {
            ManagementReportsModel model = new() { ReportType = ReportType, FromDate = FromDate, ToDate = ToDate };
            if (ReportType == "IE_X") model.ReportTitle = "IE Performance";
            else if (ReportType == "CLUSTER_X") model.ReportTitle = "Cluster Wise Performance Report";
            else if (ReportType == "ICSUBMIT") model.ReportTitle = "IC Submission Report";
            else if (ReportType == "CALLSWITHOUTIC") model.ReportTitle = "Pending IC's Against Calls where Material has been Sccepted or Rejected";
            else if (ReportType == "SUPSURPSUMM") model.ReportTitle = "CO Wise Super Surprise Summary";
            else if (ReportType == "PENDING_CALLS") model.ReportTitle = "Overdue/Pending Calls";
            else if (ReportType == "COUNTIC") model.ReportTitle = "CM and IE wise IC issued but not recieved";
            else if (ReportType == "CALL_DETAILS") model.ReportTitle = "Call Details Dashborad";

            return View(model);
        }

        public IActionResult ManageRWB(string ReportType, string FromYearMonth, string ToYearMonth)
        {
            ManagementReportsModel model = new() { ReportType = ReportType, FromYearMonth = FromYearMonth, ToYearMonth = ToYearMonth };
            if (ReportType == "RWB") model.ReportTitle = "Region Wise Billing Summary";
            return View("Manage", model);
        }

        public IActionResult ManageRWCO(string ReportType, DateTime FromDate, string Outstanding)
        {
            ManagementReportsModel model = new() { ReportType = ReportType, FromDate = FromDate, Outstanding = Outstanding };
            if (ReportType == "R") model.ReportTitle = "Region Wise Comparison of Outstanding";
            return View("Manage", model);
        }

        public IActionResult ManageSuperSurprise(string ReportType, DateTime FromDate, DateTime ToDate, string ParticularCM, string ParticularSector)
        {
            ManagementReportsModel model = new() { ReportType = ReportType, FromDate = FromDate, ToDate = ToDate, ParticularCM = ParticularCM, ParticularSector = ParticularSector };
            if (ReportType == "SUPSUR") model.ReportTitle = "Super Surprise Details";
            return View("Manage", model);
        }

        public IActionResult ManageConsignReject(string ReportType, DateTime FromDate, DateTime ToDate, string Region, string Status)
        {
            ManagementReportsModel model = new() { ReportType = ReportType, FromDate = FromDate, ToDate = ToDate, Region = Region, Status = Status };
            if (ReportType == "CONSIGN_REJECT") model.ReportTitle = "Online Consignee Rejection Report";
            return View("Manage", model);
        }

        public IActionResult ManageOutstandingOverRegion(string ReportType, DateTime FromDate)
        {
            ManagementReportsModel model = new() { ReportType = ReportType, FromDate = FromDate, Region = Region };
            if (ReportType == "X") model.ReportTitle = "Outstanding of One Region Over Other";
            return View("Manage", model);
        }

        public IActionResult ManageClientWiseRejection(string ReportType, DateTime FromDate, DateTime ToDate, string ClientType, string BPORailway)
        {
            ManagementReportsModel model = new() { ReportType = ReportType, FromDate = FromDate, ToDate = ToDate, ClientType = ClientType, BPORailway = BPORailway };
            if (ReportType == "CLIENTWISEREJ") model.ReportTitle = "Rejection Details Client Wise";
            return View("Manage", model);
        }

        public IActionResult ManageNonConformity(string ReportType, string FromYearMonth, string ToYearMonth, int IeCd)
        {
            ManagementReportsModel model = new() { ReportType = ReportType, FromYearMonth = FromYearMonth, ToYearMonth = ToYearMonth, IeCd = IeCd };
            if (ReportType == "NON_CONFORMITY") model.ReportTitle = "Format for Monthly Non Conformity Report";
            return View("Manage", model);
        }

        public IActionResult ManageTentativeInspection(string ReportType, DateTime FromDate, DateTime ToDate, string ParticularCM, string SortedOn)
        {
            ManagementReportsModel model = new() { ReportType = ReportType, FromDate = FromDate, ToDate = ToDate, ParticularCM = ParticularCM, SortedOn = SortedOn };
            if (ReportType == "HIGHVALUE") model.ReportTitle = "Tentative Inspection Fee Wise Pending Call";
            return View("Manage", model);
        }

        public IActionResult ManageCallRemarking(string ReportType, DateTime FromDate, DateTime ToDate, string CallRemarkingDate, string CallsStatus)
        {
            ManagementReportsModel model = new() { ReportType = ReportType, FromDate = FromDate, ToDate = ToDate, CallRemarkingDate = CallRemarkingDate, CallsStatus = CallsStatus };
            if (ReportType == "REMARKING") model.ReportTitle = "Call Remarking Detail";
            return View("Manage", model);
        }

        public IActionResult IEPerformance(DateTime FromDate, DateTime ToDate)
        {
            IEPerformanceModel model = managementReportsRepository.GetIEPerformanceData(FromDate, ToDate, Region);
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

        public IActionResult GetBPORailway(string ClientType)
        {
            return Json(Common.GetBPORailway(ClientType).ToList());
        }
    }
}