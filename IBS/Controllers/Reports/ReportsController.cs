using IBS.Filters;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Interfaces.Reports;
using IBS.Models;
using IBS.Models.Reports;
using IBS.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace IBS.Controllers.Reports
{
    [Authorization]
    public class ReportsController : BaseController
    {
        #region Variables
        private readonly IReportsRepository reportsRepository;
        private readonly IIC_ReceiptRepository iC_ReceiptRepository;
        private readonly IWebHostEnvironment env;
        private readonly IConfiguration _config;
        #endregion

        public ReportsController(IReportsRepository _reportsRepository, IIC_ReceiptRepository _iC_ReceiptRepository, IWebHostEnvironment _environment, IConfiguration configuration)
        {
            reportsRepository = _reportsRepository;
            iC_ReceiptRepository = _iC_ReceiptRepository;
            env = _environment;
            _config = configuration;
        }

        public IActionResult Index()
        {
            ViewBag.Region = GetUserInfo.Region;
            return View();
        }

        public IActionResult Manage(string ReportType, DateTime FromDate, DateTime ToDate, string FDate, string TDate)
        {
            ReportsModel model = new() { ReportType = ReportType, FromDate = FromDate, ToDate = ToDate,FDate = FDate,TDate = TDate };
            if (ReportType == "UNBILLEDIC") model.ReportTitle = "IC RECEIVED IN OFFICE BUT NOT BILLED";
            else if (ReportType == "PendingJICases") model.ReportTitle = "Pending JI Cases";
            else if (ReportType == "IEWorkPlan") model.ReportTitle = "IE DAILY WORK PLAN REPORT";
            else if (ReportType == "CCI") model.ReportTitle = "CONSIGNEE COMPLAINTS";
            else if (ReportType == "BSV") model.ReportTitle = "Summary Of Bank Statement Voucher";
            return View(model);
        }
        public IActionResult Manage7thCopy(string ReportType, string Bk_No, string Set_No_Fr)
        {
            ReportsModel model = new() { ReportType = ReportType, Bk_No = Bk_No, Set_No = Set_No_Fr };
            if (ReportType == "IE7thCopy") model.ReportTitle = "INSPECTION CERTIFICATE BOOK SET 7TH COPY REPORT";
            return View("Manage", model);
        }
        public IActionResult ManageICIssued(string ReportType, string Type, DateTime FromDate, DateTime ToDate)
        {
            ReportsModel model = new() { ReportType = ReportType, Type = Type, FromDate = FromDate, ToDate = ToDate };
            model.ReportTitle = "IC Issued But Not Received in Office";
            return View("Manage", model);
        }
        public IActionResult ManageStausIC(string ReportType, string Type, string IE_CD, string IE_Name, DateTime FromDate, DateTime ToDate)
        {
            var Region = GetUserInfo.Region;
            ReportsModel model = new() { ReportType = ReportType, Type = Type, Ie_Cd = IE_CD, IE_Name = IE_Name, FromDate = FromDate, ToDate = ToDate };
            if (ReportType == "ICStatus") model.ReportTitle = "IC Status";
            return View("Manage", model);
        }

        public IActionResult ManageIEDairy(string ReportType, DateTime FromDate, DateTime ToDate, string OrderByVisit)
        {
            ReportsModel model = new() { ReportType = ReportType, FromDate = FromDate, ToDate = ToDate, OrderByVisit = OrderByVisit };
            if (ReportType == "IEDairy") model.ReportTitle = "IC Status";
            return View("Manage", model);
        }

        #region UnBilled IC //UnBilled IC
        public IActionResult UnBilledIC(DateTime FromDate, DateTime ToDate)
        {
            var wRegion = "";
            var Region = SessionHelper.UserModelDTO.Region;
            if (Region == "N") { wRegion = "Northern Region"; }
            else if (Region == "S") { wRegion = "Southern Region"; }
            else if (Region == "E") { wRegion = "Eastern Region"; }
            else if (Region == "W") { wRegion = "Western Region"; }
            else if (Region == "C") { wRegion = "Central Region"; }
            ICUnbilledModel model = new() { FromDate = FromDate, ToDate = ToDate };
            model.Region = wRegion;
            model.lstICUnBilledList = iC_ReceiptRepository.Get_UnBilled_IC(model.Display_FromDate, model.Display_ToDate, Region);
            GlobalDeclaration.ICUnbilled = model;
            return PartialView(model);
        }
        #endregion
        #region Bank Statement
        public IActionResult BankStatement(string FDate, string TDate)
        {
            RecieptVoucherModel model = new();
            var wRegion = "";
            var Region = SessionHelper.UserModelDTO.Region;
            if (Region == "N") { wRegion = "Northern Region"; }
            else if (Region == "S") { wRegion = "Southern Region"; }
            else if (Region == "E") { wRegion = "Eastern Region"; }
            else if (Region == "W") { wRegion = "Western Region"; }
            else if (Region == "C") { wRegion = "Central Region"; }
           // RecieptVoucherModel model = new() { FromDate = Convert.ToString(FromDate), ToDate = Convert.ToString(ToDate) };
            model.Region = wRegion;
            model = reportsRepository.GetBankStatement(FDate,TDate, Region);
            model.FromDate = FDate;
            model.ToDate = TDate;
            GlobalDeclaration.BankStatement = model;
            return PartialView(model);
        }
        #endregion

        #region IC 7th Copy Report //IC7thCopyReport        
        public IActionResult IE7thCopyReport(string Bk_No, string Set_No)
        {
            var model = reportsRepository.GetIE7thCopyReport(Bk_No, Set_No, GetUserInfo);
            model.UserName = GetUserInfo.Name;
            model.UserID = Convert.ToString(GetUserInfo.UserName);
            GlobalDeclaration.IE7thCopyList = model;
            return PartialView(model);
        }

        public IActionResult Get_Book_Set_Copy([FromBody] DTParameters dTParameters)
        {
            var data = reportsRepository.Get_IE_7thCopyList(dTParameters, GetUserInfo);
            return Json(data);
        }
        #endregion       

        #region IC Issued But Not Received in Office        
        public IActionResult ICIssuedNotReceived(string Type, DateTime FromDate, DateTime ToDate)
        {
            var wRegion = "";
            var Region = SessionHelper.UserModelDTO.Region;
            if (Region == "N") { wRegion = "Northern Region"; }
            else if (Region == "S") { wRegion = "Southern Region"; }
            else if (Region == "E") { wRegion = "Eastern Region"; }
            else if (Region == "W") { wRegion = "Western Region"; }
            else if (Region == "C") { wRegion = "Central Region"; }
            ICIssuedNotReceivedReportModel model = new() { Type = Type, FromDate = FromDate, ToDate = ToDate, Region = wRegion };
            model.ICIssuedNotReceivedList = iC_ReceiptRepository.Get_IC_Issue_Not_Receive(model.Display_FromDate, model.Display_ToDate, GetUserInfo);
            foreach (var row in model.ICIssuedNotReceivedList)
            {
                var tifPath = env.WebRootPath + Enums.GetEnumDescription(Enums.FolderPath.CaseNo) + "/" + row.CASE_NO + ".TIF";
                var pdfPath = env.WebRootPath + Enums.GetEnumDescription(Enums.FolderPath.CaseNo) + "/" + row.CASE_NO + ".PDF";                
                row.IsTIF = System.IO.File.Exists(tifPath) == true ? true : false;
                row.IsPDF = System.IO.File.Exists(pdfPath) == true ? true : false;
            }
            GlobalDeclaration.ICIssuedNotReceivedReport = model;
            return PartialView(model);
        }
        #endregion

        #region Status of IC //IC Status        
        public IActionResult ICStatus(string Type, DateTime FromDate, DateTime ToDate, string IE_CD, string IE_Name)
        {
            var wRegion = "";
            var Region = SessionHelper.UserModelDTO.Region;
            if (Region == "N") { wRegion = "Northern Region"; }
            else if (Region == "S") { wRegion = "Southern Region"; }
            else if (Region == "E") { wRegion = "Eastern Region"; }
            else if (Region == "W") { wRegion = "Western Region"; }
            else if (Region == "C") { wRegion = "Central Region"; }
            ICStatusModel model = reportsRepository.Get_IC_Status(FromDate, ToDate, IE_CD, Region);
            model.Region = wRegion;
            model.Type = Type;
            model.IE_Name = IE_Name;
            GlobalDeclaration.ICStatus = model;
            return PartialView(model);
        }
        #endregion

        #region Pending JI Cases
        public IActionResult Pending_JI_Cases_Partial(DateTime FromDate, DateTime ToDate)
        {
            var IeCd = Convert.ToString(GetUserInfo.IeCd);
            PendingJICasesReportModel model = new();
            model = reportsRepository.Get_Pending_JI_Cases(FromDate, ToDate, IeCd);

            foreach (var row in model.lstPendingJI)
            {
                var casetifpath = env.WebRootPath + Enums.GetEnumDescription(Enums.FolderPath.ComplaintCase) + "/" + row.CASE_NO + "-" + row.BK_NO + "-" + row.SET_NO + ".TIF";
                var casepdfpath = env.WebRootPath + Enums.GetEnumDescription(Enums.FolderPath.ComplaintCase) + "/" + row.CASE_NO + "-" + row.BK_NO + "-" + row.SET_NO + ".PDF";

                var reporttifpath = env.WebRootPath + Enums.GetEnumDescription(Enums.FolderPath.COMPLAINTSREPORT) + "/" + row.CASE_NO + "-" + row.BK_NO + "-" + row.SET_NO + ".TIF";
                var reportpdfpath = env.WebRootPath + Enums.GetEnumDescription(Enums.FolderPath.COMPLAINTSREPORT) + "/" + row.CASE_NO + "-" + row.BK_NO + "-" + row.SET_NO + ".PDF";

                row.IsCaseTIF = System.IO.File.Exists(casetifpath) == true ? true : false;
                row.IsCasePDF = System.IO.File.Exists(casepdfpath) == true ? true : false;

                row.IsReportTIF = System.IO.File.Exists(reporttifpath) == true ? true : false;
                row.IsReportPDF = System.IO.File.Exists(reportpdfpath) == true ? true : false;
            }
            GlobalDeclaration.PendingJICasesReport = model;
            return PartialView(model);
        }
        #endregion


        public IActionResult IEDairy_Partial(DateTime FromDate, DateTime ToDate, string OrderByVisit)
        {
            IEDairyModel model = new();
            model = reportsRepository.Get_IE_Dairy(FromDate, ToDate, "", OrderByVisit, "true", GetUserInfo);
            GlobalDeclaration.IEDairy = model;
            return PartialView(model);
        }

        public IActionResult IEWorkPlan(DateTime FromDate, DateTime ToDate)
        {
            IEWorkPlanModel model = new();
            model = reportsRepository.Get_IE_WorkPlan(FromDate, ToDate, Convert.ToString(GetUserInfo.IeCd), GetUserInfo.Region);
            GlobalDeclaration.IEWorkPlan = model;
            return PartialView(model);
        }

        public IActionResult ConsigneeComplaints(DateTime FromDate, DateTime ToDate)
        {
            ConsigneeComplaintsModel model = new();
            model = reportsRepository.Get_Consignee_Complaints(FromDate, ToDate, Convert.ToString(GetUserInfo.IeCd),GetUserInfo.Region);
            foreach (var row in model.lstConsigneeComplaints)
            {
                var rejecttif = env.WebRootPath + Enums.GetEnumDescription(Enums.FolderPath.RejectionMemo) + "/" + row.CASE_NO + "-" + row.BK_NO + "-" + row.SET_NO + ".TIF";
                var rejectpdf = env.WebRootPath + Enums.GetEnumDescription(Enums.FolderPath.RejectionMemo) + "/" + row.CASE_NO + "-" + row.BK_NO + "-" + row.SET_NO + ".PDF";

                var casetifpath = env.WebRootPath + Enums.GetEnumDescription(Enums.FolderPath.ComplaintCase) + "/" + row.CASE_NO + "-" + row.BK_NO + "-" + row.SET_NO + ".TIF";
                var casepdfpath = env.WebRootPath + Enums.GetEnumDescription(Enums.FolderPath.ComplaintCase) + "/" + row.CASE_NO + "-" + row.BK_NO + "-" + row.SET_NO + ".PDF";

                var reporttifpath = env.WebRootPath + Enums.GetEnumDescription(Enums.FolderPath.COMPLAINTSREPORT) + "/" + row.CASE_NO + "-" + row.BK_NO + "-" + row.SET_NO + ".TIF";
                var reportpdfpath = env.WebRootPath + Enums.GetEnumDescription(Enums.FolderPath.COMPLAINTSREPORT) + "/" + row.CASE_NO + "-" + row.BK_NO + "-" + row.SET_NO + ".PDF";
                
                row.IsRejectionMemoTif = System.IO.File.Exists(rejecttif) == true ? true : false;
                row.IsRejectionMemoPdf = System.IO.File.Exists(rejectpdf) == true ? true : false;

                row.IsComplainCaseTif = System.IO.File.Exists(casetifpath) == true ? true : false;
                row.IsComplainCasePdf = System.IO.File.Exists(casepdfpath) == true ? true : false;

                row.IsComplainReportTif = System.IO.File.Exists(reporttifpath) == true ? true : false;
                row.IsComplainReportPdf = System.IO.File.Exists(reportpdfpath) == true ? true : false;
            }
            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> GeneratePDF(string ReportType)
        {
            string htmlContent = string.Empty;

            if (ReportType == "UNBILLEDIC")
            {
                ICUnbilledModel model = GlobalDeclaration.ICUnbilled;
                htmlContent = await this.RenderViewToStringAsync("/Views/Reports/UnBilledIC.cshtml", model);
            }
            else if (ReportType == "IE7thCopy")
            {
                IE7thCopyListModel model = GlobalDeclaration.IE7thCopyList;
                htmlContent = await this.RenderViewToStringAsync("/Views/Reports/IE7thCopyReport.cshtml", model);
            }
            else if (ReportType == "ICISSUEDNSUB")
            {
                ICIssuedNotReceivedReportModel model = GlobalDeclaration.ICIssuedNotReceivedReport;
                htmlContent = await this.RenderViewToStringAsync("/Views/Reports/ICIssuedNotReceived.cshtml", model);
            }
            else if (ReportType == "ICStatus")
            {
                ICStatusModel model = GlobalDeclaration.ICStatus;
                htmlContent = await this.RenderViewToStringAsync("/Views/Reports/ICStatus.cshtml", model);
            }
            else if (ReportType == "PendingJICases")
            {
                PendingJICasesReportModel model = GlobalDeclaration.PendingJICasesReport;
                htmlContent = await this.RenderViewToStringAsync("/Views/Reports/Pending_JI_Cases_Partial.cshtml", model);
            }
            else if (ReportType == "IEDairy")
            {
                IEDairyModel model = GlobalDeclaration.IEDairy;
                htmlContent = await this.RenderViewToStringAsync("/Views/Reports/IEDairy_Partial.cshtml", model);
            }
            else if (ReportType == "IEWorkPlan")
            {
                IEWorkPlanModel model = GlobalDeclaration.IEWorkPlan;
                htmlContent = await this.RenderViewToStringAsync("/Views/Reports/IEWorkPlan.cshtml", model);
            }
            else if (ReportType == "BSV")
            {
                RecieptVoucherModel model = GlobalDeclaration.BankStatement;
                htmlContent = await this.RenderViewToStringAsync("/Views/Reports/BankStatement.cshtml", model);
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
