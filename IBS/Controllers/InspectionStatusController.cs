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

namespace IBS.Controllers
{
    [Authorization]
    public class InspectionStatusController : BaseController
    {
        #region Variables
        private readonly IInspectionStatusRepository InspectionStatusRepository;
        private readonly IWebHostEnvironment env;
        #endregion
        public InspectionStatusController(IInspectionStatusRepository _InspectionStatusRepository, IWebHostEnvironment _env)
        {
            InspectionStatusRepository = _InspectionStatusRepository;
            this.env = _env;
        }

        public IActionResult Index()
        {
            ViewBag.Region = Region;
            return View();
        }
        public IActionResult Manage(string ReportType, string Month, string Year,string ForGiven,string ReportBasedon,string MaterialValue,string FromDate,string ToDate,string ForParticular,string lstParticular)
        {
            SummaryConsigneeWiseInspModel model = new() { ReportType = ReportType, Month = Month, Year = Year, FromDt = FromDate, ToDt = ToDate,ForGiven = ForGiven,ReportBasedon = ReportBasedon,MaterialValue = MaterialValue,ForParticular = ForParticular,lstParticular = lstParticular };
            if (ReportType == "ConInsp") model.ReportTitle = "Summary of Consignee Wise Inspections";
            else if (ReportType == "VenInsp") model.ReportTitle = "Summary of Vendor Wise Inspections";
            return View(model);
        }
        public IActionResult SummaryConsigneeWiseInsp(string ReportType, string Month, string Year, string ForGiven, string ReportBasedon, string MaterialValue, string FromDate, string ToDate, string ForParticular, string lstParticular)
        {
            ViewBag.From = FromDate;
            ViewBag.To = ToDate;
            ViewBag.ReportType = ReportType;
            
            ViewBag.year = Year;
            if(Convert.ToBoolean(ReportBasedon) == true)
            {
                ViewBag.ReportBase = "Report Based on IC Date & Sorted on Consignee";
            }
            else
            {
                ViewBag.ReportBase = "Report Based on BILL Date & Sorted on Consignee";
            }
            if (Convert.ToBoolean(MaterialValue) == true)
            {
                if (Convert.ToBoolean(ReportBasedon) == true)
                {
                    ViewBag.ReportBase = "Report Based on IC Date & Sorted on Material Value Descending";
                }
                else
                {
                    ViewBag.ReportBase = "Report Based on BILL Date & Sorted on Material Value Descending";
                }
                    
            }
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
            if (Convert.ToBoolean(ForGiven) == true)
            {
                ViewBag.ForGiven = "Yes";
            }

            string Region = GetRegionCode;
            if(Region == "N")
            { ViewBag.Region = "NORTHERN REGION"; }
            else if(Region == "S")
            { ViewBag.Region = "SOUTHERN REGION"; }
            else if (Region == "E")
            { ViewBag.Region = "EASTERN REGION"; }
            else if (Region == "W")
            { ViewBag.Region = "WESTERN REGION"; }
            else if (Region == "C")
            { ViewBag.Region = "CENTRAL REGION"; }
            // DTResult<SummaryConsigneeWiseInspModel> dTResult = SummaryConsigneeWiseInspRepository.SummaryConsigneeWiseInsp(dtParameters, Regin);
            SummaryConsigneeWiseInspModel model = InspectionStatusRepository.SummaryConsigneeWiseInsp( ReportType,  Month,  Year,  ForGiven,  ReportBasedon,MaterialValue,  FromDate,  ToDate,  ForParticular,  lstParticular ,Region);
            return PartialView(model);
        }

        public IActionResult SummaryVendorWiseInsp(string ReportType, string Month, string Year, string ForGiven, string ReportBasedon, string MaterialValue, string FromDate, string ToDate, string ForParticular, string lstParticular)
        {
            ViewBag.From = FromDate;
            ViewBag.To = ToDate;

            ViewBag.year = Year;
            if (Convert.ToBoolean(ReportBasedon) == true)
            {
                ViewBag.ReportBase = "Report Based on IC Date & Sorted on Vendor";
            }
            else
            {
                ViewBag.ReportBase = "Report Based on BILL Date & Sorted on Vendor";
            }
            if (Convert.ToBoolean(MaterialValue) == true)
            {
                if (Convert.ToBoolean(ReportBasedon) == true)
                {
                    ViewBag.ReportBase = "Report Based on IC Date & Sorted on Material Value Descending";
                }
                else
                {
                    ViewBag.ReportBase = "Report Based on BILL Date & Sorted on Material Value Descending";
                }

            }
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
            if (Convert.ToBoolean(ForGiven) == true)
            {
                ViewBag.ForGiven = "Yes";
            }

            string Region = GetRegionCode;
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
            // DTResult<SummaryConsigneeWiseInspModel> dTResult = SummaryConsigneeWiseInspRepository.SummaryConsigneeWiseInsp(dtParameters, Regin);
            SummaryVendorWiseInspModel model = InspectionStatusRepository.SummaryVendorWiseInsp(ReportType, Month, Year, ForGiven, ReportBasedon, MaterialValue, FromDate, ToDate, ForParticular, lstParticular, Region);
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
