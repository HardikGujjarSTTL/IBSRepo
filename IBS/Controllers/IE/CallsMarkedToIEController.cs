using IBS.Filters;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using IBS.Models.Reports;
using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace IBS.Controllers.IE
{
    [Authorization]
    public class CallsMarkedToIEController : BaseController
    {
        #region Variables
        private readonly ICallMarkedToIERepository callmarksRepository;
        public string PType = "";
        private readonly IWebHostEnvironment env;
        #endregion

        public CallsMarkedToIEController(ICallMarkedToIERepository _callmarksRepository, IWebHostEnvironment _env)
        {
            callmarksRepository = _callmarksRepository;
            this.env = _env;
        }
        public IActionResult CallsMarkedToIE(string type)
        {
            CallsMarkedToIEModel model = new();
            string Fpath = $"{Request.Scheme}://{Request.Host}";
            var CaseNoPath = env.WebRootPath + Enums.GetEnumDescription(Enums.FolderPath.CaseNo);
            var LabPath = env.WebRootPath + Enums.GetEnumDescription(Enums.FolderPath.Lab);
            model = callmarksRepository.GetReport(GetIeCd, Convert.ToString(UserId), type);
            model.PType = type;
            model.IeName = IeName;

            model.FilePath1 = Fpath;
            model.FilePath2 = CaseNoPath;
            model.FilePath3 = LabPath;

            GlobalDeclaration.CallsMarked = model;
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {

            DTResult<CallsMarkedToIEModel> dTResult = callmarksRepository.GetDataList(dtParameters, GetRegionCode, Convert.ToString(UserId), GetIeCd);
            return Json(dTResult);
        }

        [HttpPost]
        public async Task<IActionResult> GeneratePDF(string ReportType)
        {
            string htmlContent = string.Empty;

            if (ReportType == "C")
            {
                CallsMarkedToIEModel model = GlobalDeclaration.CallsMarked;
                htmlContent = await this.RenderViewToStringAsync("/Views/CallsMarkedToIE/CallsMarkedToIE.cshtml", model);
            }
            else if (ReportType == "V")
            {
                CallsMarkedToIEModel model = GlobalDeclaration.CallsMarked;
                htmlContent = await this.RenderViewToStringAsync("/Views/CallsMarkedToIE/CallsMarkedToIE.cshtml", model);
            }
            else if (ReportType == "I")
            {
                CallsMarkedToIEModel model = GlobalDeclaration.CallsMarked;
                htmlContent = await this.RenderViewToStringAsync("/Views/CallsMarkedToIE/CallsMarkedToIE.cshtml", model);
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
    }
}
