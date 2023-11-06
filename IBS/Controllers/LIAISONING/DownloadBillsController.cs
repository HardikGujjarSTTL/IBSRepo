//using CrystalDecisions.CrystalReports.Engine;
//using CrystalDecisions.Shared;
using IBS.DataAccess;
using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using IBS.Models.Reports;
using IBS.Repositories;
using IBS.Repositories.Reports;
using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp.Media;
using PuppeteerSharp;
using System;
using System.Drawing;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using IBS.Interfaces.Reports;
using System.Data;
using OfficeOpenXml;
using System.IO;

namespace IBS.Controllers
{
    public class DownloadBillsController : BaseController
    {
        #region Variables
        private readonly IDownloadBillsRepository DownloadBillsRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        #endregion
        public DownloadBillsController(IDownloadBillsRepository _DownloadBillsRepository, IWebHostEnvironment webHostEnvironment)
        {
            DownloadBillsRepository = _DownloadBillsRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            ViewBag.Region = Region;
            return View();
        }
        
        public IActionResult Manage(string Month, string Year, string FromDate, string ToDate,string RBMonth)
        {

            DownloadBillsModel model = new()
            {
                Month = Month,
                Year = Year,
                FromDate = FromDate,
                ToDate = ToDate,
                RBMonth = RBMonth
            };
            return View(model);
        }
        public IActionResult DownloadBills(string Month, string Year, string FromDate, string ToDate, string RBMonth)
        {
            ViewBag.From = FromDate;
            ViewBag.To = ToDate;
            ViewBag.year = Year;
            if (Month != null)
            {
                string[] monthNames = new string[]
                {
                "January", "February", "March", "April", "May", "June",
                "July", "August", "September", "October", "November", "December"
                };
                string monthName = monthNames[Convert.ToInt32(Month) - 1];
                ViewBag.month = monthName;
            }
            DownloadBillsModel model = new DownloadBillsModel();
            string Region = GetRegionCode;
            string OrgType = OrgnType;
            string Org = Organisation;
            try
            {
                if (Region == "N")
                { ViewBag.Region = "NORTHERN REGION"; }
                else if (Region == "S")
                { ViewBag.Region = "SOUTHERN REGION"; }
                else if (Region == "E")
                { ViewBag.Region = "EASTERN REGION"; }
                else if (Region == "W")
                { ViewBag.Region = "WESTERN REGION"; }
                else if (Region == "C")
                { ViewBag.Region = "CENTRAL REGION"; }

                model = DownloadBillsRepository.GetReturnedBills(Month,Year,FromDate,ToDate,OrgnType,Org, RBMonth);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "DownloadBills", "DownloadBills", 1, GetIPAddress());
            }
            return PartialView(model);
        }
        //[HttpPost]
        //public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        //{
        //    string OrgType = OrgnType;
        //    string Org = Organisation;
        //    DTResult<DownloadBillsModel> dTResult = DownloadBillsRepository.GetReturnedBills(dtParameters, OrgType, Org, _webHostEnvironment);
        //    return Json(dTResult);
        //}
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

            string cssPath = _webHostEnvironment.WebRootPath + "/css/report.css";

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
