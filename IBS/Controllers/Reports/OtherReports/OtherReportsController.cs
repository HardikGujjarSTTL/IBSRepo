using IBS.Filters;
using IBS.Helper;
using IBS.Interfaces.Reports;
using IBS.Interfaces.Reports.OtherReports;
using IBS.Models.Reports;
using IBS.Repositories.Reports;
using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp.Media;
using PuppeteerSharp;
using Microsoft.AspNetCore.Mvc.Rendering;
using IBS.Models;
using System.IO;

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
            ViewBag.Region = Region;
            return View();
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
            else if(ReportType == "COIEWiseCalls")
            {
                CoIeWiseCallsModel model = GlobalDeclaration.CoIeWiseCalls;
                htmlContent = await this.RenderViewToStringAsync("/Views/OtherReports/CoIeWiseCalls.cshtml", model);
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
