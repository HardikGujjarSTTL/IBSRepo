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

namespace IBS.Controllers.Client
{
    public class ClientWiseCallStatusController : BaseController
    {
        #region Variables
        private readonly IClientCallStatusRepository ClientCallStatusRepository;
        private readonly IWebHostEnvironment env;
        #endregion
        public ClientWiseCallStatusController(IClientCallStatusRepository _ClientCallStatusRepository, IWebHostEnvironment _env)
        {
            ClientCallStatusRepository = _ClientCallStatusRepository;
            this.env = _env;
        }
        public IActionResult Index()
        {
            ViewBag.Region = Region;
            return View();
        }
        public IActionResult Manage(string ReportType, string FromDate, string ToDate, string ReportStatus)
        {

            ClientCallRptModel model = new()
            {
                ReportType = ReportType,
                FromDate = FromDate,
                ToDate = ToDate,
                CallCode = ReportStatus
            };
            if (ReportType == "ClientWise") model.ReportTitle = "CLIENT WISE CALL DETAILS";
            else if (ReportType == "VendorWise") model.ReportTitle = "VENDOR WISE REJECTION";            
            return View(model);
        }
        public IActionResult ClientCallStatusReport(string FromDate, string ToDate, string ReportStatus)
        {
            ClientCallRptModel model = new ClientCallRptModel();
            try
            {
                ViewBag.From = FromDate;
                ViewBag.To = ToDate;
                string OrgType = OrgnTypeClient;
                string Org = OrganisationClient;
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

                model = ClientCallStatusRepository.GetCallStatusC( FromDate,  ToDate,  ReportStatus, OrgType, Org);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ClientWiseCallStatus", "ClientCallStatusReport", 1, GetIPAddress());
            }
            return PartialView(model);
            //string actionValue = HttpContext.Request.Query["Action"];
            //ViewBag.Action = actionValue;
            
        }
        public IActionResult VendorCallStatusReport(string FromDate, string ToDate, string ReportStatus)
        {
            ClientCallRptModel model = new ClientCallRptModel();
            try
            {
                ViewBag.From = FromDate;
                ViewBag.To = ToDate;
                string OrgType = OrgnTypeClient;
                string Org = OrganisationClient;
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

                model = ClientCallStatusRepository.GetCallStatusR(FromDate, ToDate, ReportStatus, OrgType, Org);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ClientWiseCallStatus", "VendorCallStatusReport", 1, GetIPAddress());
            }
            return PartialView(model);
        }
        //[HttpPost]
        //public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        //{
        //    var Action = dtParameters.AdditionalValues?.GetValueOrDefault("Action");
        //    string OrgType = OrgnTypeClient;
        //    string Org = OrganisationClient;
        //    DTResult<ClientCallRptModel> dTResult = new DTResult<ClientCallRptModel>();
        //    if (Action == "R")
        //    {
        //         dTResult = ClientCallStatusRepository.GetCallStatusR(dtParameters, OrgType, Org);
        //    }
        //    else
        //    {
        //         dTResult = ClientCallStatusRepository.GetCallStatusC(dtParameters, OrgType, Org);
        //    }

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
