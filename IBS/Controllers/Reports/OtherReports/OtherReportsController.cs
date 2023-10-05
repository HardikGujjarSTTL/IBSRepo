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
using Newtonsoft.Json;

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
                if (formCollection.Keys.Contains("hdniecmname") && !string.IsNullOrEmpty(formCollection["hdniecmname"])) model.IEName = Convert.ToString(formCollection["hdniecmname"]);
                if (formCollection.Keys.Contains("hdnCOName") && !string.IsNullOrEmpty(formCollection["hdnCOName"])) model.COName = Convert.ToString(formCollection["hdnCOName"]);
            }else if(model.ReportType == "IEWISET")
            {
                if (formCollection.Keys.Contains("hdnIENAME") && !string.IsNullOrEmpty(formCollection["hdnIENAME"])) model.IEName = Convert.ToString(formCollection["hdnIENAME"]);
                if (formCollection.Keys.Contains("hdnTrainingArea") && !string.IsNullOrEmpty(formCollection["hdnTrainingArea"])) model.TrainingArea = Convert.ToString(formCollection["hdnTrainingArea"]);
                if (formCollection.Keys.Contains("hdnMechanical") && !string.IsNullOrEmpty(formCollection["hdnMechanical"])) model.Mechanical = Convert.ToString(formCollection["hdnMechanical"]);
                if (formCollection.Keys.Contains("hdnElectrical") && !string.IsNullOrEmpty(formCollection["hdnElectrical"])) model.Electrical = Convert.ToString(formCollection["hdnElectrical"]);
                if (formCollection.Keys.Contains("hdnCivil") && !string.IsNullOrEmpty(formCollection["hdnCivil"])) model.Civil = Convert.ToString(formCollection["hdnCivil"]);
                if (formCollection.Keys.Contains("hdnRegular") && !string.IsNullOrEmpty(formCollection["hdnRegular"])) model.Regular = Convert.ToString(formCollection["hdnRegular"]);
                if (formCollection.Keys.Contains("hdnDeputaion") && !string.IsNullOrEmpty(formCollection["hdnDeputaion"])) model.Deputaion = Convert.ToString(formCollection["hdnDeputaion"]);
                if (formCollection.Keys.Contains("hdnParticularie") && !string.IsNullOrEmpty(formCollection["hdnParticularie"])) model.Particularie = Convert.ToString(formCollection["hdnParticularie"]);
                if (formCollection.Keys.Contains("hdnParticularArea") && !string.IsNullOrEmpty(formCollection["hdnParticularArea"])) model.ParticularArea = Convert.ToString(formCollection["hdnParticularArea"]);
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
            else if(ReportType == "C" || ReportType == "I")
            {
                NCRReport model = GlobalDeclaration.NCRReports;
                htmlContent = await this.RenderViewToStringAsync("/Views/OtherReports/NCRCWiseReport.cshtml", model);
            } else if(ReportType == "IEWISET")
            {
                IEWiseTrainingReportModel model = GlobalDeclaration.IEWiseTrainingReport;
                htmlContent = await this.RenderViewToStringAsync("/Views/OtherReports/IEWiseTrainingReport.cshtml", model);
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
