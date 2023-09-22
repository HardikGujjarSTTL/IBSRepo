using IBS.Helper;
using IBS.Interfaces;
using IBS.Interfaces.Reports;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp.Media;
using PuppeteerSharp;

namespace IBS.Controllers.Reports
{
    public class CoComplaintJIRequiredController : BaseController
    {
        #region Variables
        private readonly ICoComplaintJIRequiredRepository coComplaintJIRequiredRepository;
        private readonly IWebHostEnvironment env;
        #endregion
        public CoComplaintJIRequiredController(ICoComplaintJIRequiredRepository _coComplaintJIRequiredRepository, IWebHostEnvironment _env)
        {
            coComplaintJIRequiredRepository = _coComplaintJIRequiredRepository;
            this.env = _env;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Manage(string FinancialYearsText,string FinancialYearsValue)
        {
            JIRequiredReport model = new() { FinancialYearsText = FinancialYearsText, FinancialYearsValue= FinancialYearsValue };
            model.ReportTitle = "JI Complaints Report";
            return View(model);
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
            JIRequiredReport model = coComplaintJIRequiredRepository.GetJIComplaintsList(FinancialYearsText,FinancialYearsValue);
            ViewBag.Financialperiod = FinancialYearsText;
            ViewBag.Regions = wRegion;
            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> GeneratePDF(string htmlContent)
        {
            //PendingICAgainstCallsModel _model = JsonConvert.DeserializeObject<PendingICAgainstCallsModel>(TempData[model.ReportType].ToString());
            //htmlContent = await this.RenderViewToStringAsync("/Views/ManagementReports/PendingICAgainstCalls.cshtml", _model);

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
