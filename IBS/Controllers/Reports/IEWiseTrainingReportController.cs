using IBS.Helper;
using IBS.Interfaces.Reports;
using IBS.Models.Reports;
using IBS.Repositories.Reports;
using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp.Media;
using PuppeteerSharp;

namespace IBS.Controllers.Reports
{
    public class IEWiseTrainingReportController : BaseController
    {
        #region Variables
        private readonly IIEWiseTrainingReportRepository iEWiseTrainingReportRepository;
        private readonly IWebHostEnvironment env;
        #endregion
        public IEWiseTrainingReportController(IIEWiseTrainingReportRepository _iEWiseTrainingReportRepository, IWebHostEnvironment _env)
        {
            iEWiseTrainingReportRepository = _iEWiseTrainingReportRepository;
            this.env = _env;
        }
        public IActionResult Index()
        {
            string Region = SessionHelper.UserModelDTO.Region;
            ViewBag.Regions = Region;
            return View();
        }

        public IActionResult Manage(string IENAME, string TrainingArea, string Mechanical, string Electrical, string Civil, string Regular, string Deputaion, string Particularie , string ParticularArea)
        {
            IEWiseTrainingReportModel model = new()
            {
                IENAME = IENAME,
                TrainingArea = TrainingArea,
                Mechanical = Mechanical,
                Electrical = Electrical,
                Civil = Civil,
                Regular = Regular,
                Deputaion = Deputaion,
                Particularie = Particularie,
                ParticularArea = ParticularArea
            };
            model.ReportTitle = "IE Wise Training";
            return View(model);
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
            ViewBag.Regions = wRegion;
            IEWiseTrainingReportModel model = iEWiseTrainingReportRepository.GetIEWiseTrainingDetails( IENAME, TrainingArea, Mechanical, Electrical, Civil, Regular, Deputaion, Particularie, ParticularArea, Region);
            GlobalDeclaration.IEWiseTrainingReport = model;
            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> GeneratePDF()
        {
            string htmlContent = string.Empty;

            IEWiseTrainingReportModel model = GlobalDeclaration.IEWiseTrainingReport;
            htmlContent = await this.RenderViewToStringAsync("/Views/IEWiseTrainingReport/IEWiseTrainingReport.cshtml", model);

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
