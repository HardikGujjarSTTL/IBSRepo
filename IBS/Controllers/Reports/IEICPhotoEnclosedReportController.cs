using IBS.Filters;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Interfaces.Reports;
using IBS.Models;
using IBS.Models.Reports;
using IBS.Repositories;
using IBS.Repositories.Reports;
using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp.Media;
using PuppeteerSharp;

namespace IBS.Controllers.Reports
{
    public class IEICPhotoEnclosedReportController : BaseController
    {
        private readonly IIEICPhotoEnclosedReportRepository iIEICPhotoEnclosedReportRepository;
        private readonly IWebHostEnvironment env;

        public IEICPhotoEnclosedReportController(IIEICPhotoEnclosedReportRepository _iEICPhotoEnclosedReportRepository, IWebHostEnvironment _env)
        {
            iIEICPhotoEnclosedReportRepository = _iEICPhotoEnclosedReportRepository;
            this.env = _env;
        }

        [Authorization("IEICPhotoEnclosedReport", "Index", "view")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            string Region = SessionHelper.UserModelDTO.Region;
            DTResult<IEICPhotoEnclosedModelReport> dTResult = iIEICPhotoEnclosedReportRepository.GetDataList(dtParameters,Region);
            return Json(dTResult);
        }

        public IActionResult Manage(string CaseNo, string CallRecDT, string CallSno, string BKNO, string SETNO)
        {
            IEICPhotoEnclosedModelReport model = new()
            {
                CaseNo = CaseNo,
                CallRecDT = CallRecDT,
                CallSno = CallSno,
                BKNO = BKNO,
                SETNO = SETNO,
            };
            model.ReportTitle = "PHOTOS SUBMITTED BY IE OF INSPECTIONS";

            return View(model);
        }

        public IActionResult PhotoSubmiteedByIE(string CaseNo, string CallRecDT, string CallSno, string BKNO, string SETNO)
        {
            string wRegion = "";
            if (Region == "N") { wRegion = "Northern Region"; }
            else if (Region == "S") { wRegion = "Southern Region"; }
            else if (Region == "E") { wRegion = "Eastern Region"; }
            else if (Region == "W") { wRegion = "Western Region"; }
            else if (Region == "C") { wRegion = "Central Region"; }
            ViewBag.Regions = wRegion;
            IEICPhotoEnclosedModelReport model = iIEICPhotoEnclosedReportRepository.GetDataListReport(CaseNo, CallRecDT, CallSno, BKNO, SETNO,Region);
            GlobalDeclaration.IEICPhotoEnclosedModel = model;
            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> GeneratePDF()
        {
            string htmlContent = string.Empty;

            IEICPhotoEnclosedModelReport model = GlobalDeclaration.IEICPhotoEnclosedModel;
            htmlContent = await this.RenderViewToStringAsync("/Views/IEICPhotoEnclosedReport/PhotoSubmiteedByIE.cshtml", model);

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
