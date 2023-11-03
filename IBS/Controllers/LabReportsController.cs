﻿using IBS.DataAccess;
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

namespace IBS.Controllers
{
    [Authorization]
    public class LabReportsController : BaseController
    {
        #region Variables
        private readonly ILabReportsRepository LabReportsRepository;
        private readonly IWebHostEnvironment env;
        #endregion
        public LabReportsController(ILabReportsRepository _LabReportsRepository, IWebHostEnvironment _env)
        {
            LabReportsRepository = _LabReportsRepository;
            this.env = _env;
        }

        public IActionResult Index()
        {
            ViewBag.Region = Region;
            return View();
        }
        public IActionResult Manage(string ReportType,string wFrmDtO, string wToDt, string rdbIEWise, string rdbPIE, string rdbVendWise, string rdbPVend, string rdbLabWise, string rdbPLab, string rdbPending, string rdbPaid, string rdbDue, string rdbPartlyPaid, string lstTStatus, string lstIE, string ddlVender, string lstLab, string lstStatus, string rdbrecvdt)
        {
            
            LabReportsModel model = new()
            {
                ReportType = ReportType,
                wFrmDtO = wFrmDtO,
                wToDt = wToDt,
                rdbIEWise = rdbIEWise,
                rdbPIE = rdbPIE,
                rdbVendWise = rdbVendWise,
                rdbPVend = rdbPVend,
                rdbLabWise = rdbLabWise,
                rdbPLab = rdbPLab,
                rdbPending = rdbPending,
                rdbPaid = rdbPaid,
                rdbDue = rdbDue,
                rdbPartlyPaid = rdbPartlyPaid,
                lstTStatus = lstTStatus,
                lstIE = lstIE,
                ddlVender = ddlVender,
                lstLab = lstLab,
                lstStatus = lstStatus,
                rdbrecvdt = rdbrecvdt
            };
            if (ReportType == "LabReg") model.ReportTitle = "LAB REGISTER REPORT";
            else if (ReportType == "LabPer") model.ReportTitle = "LAB PERFORMANCE REPORT";
            else if (ReportType == "LabPos") model.ReportTitle = "LAB POSTING REPORT";
            else if (ReportType == "SummNR") model.ReportTitle = "SUMMARY OF ONLINE PAYMENT IN NR";
            else if (ReportType == "LabInv") model.ReportTitle = "LAB INVOICE REPORT";
            else if (ReportType == "LabInfo") model.ReportTitle = "LAB SAMPLE INFO DETAILS";
            return View(model);
    }
        public IActionResult LabRegisterReport(string ReportType, string wFrmDtO, string wToDt, string rdbIEWise, string rdbPIE, string rdbVendWise, string rdbPVend, string rdbLabWise, string rdbPLab, string rdbPending, string rdbPaid, string rdbDue, string rdbPartlyPaid, string lstTStatus, string lstIE, string ddlVender, string lstLab)
        {
            LabReportsModel model = new LabReportsModel();
            try
            {
                ViewBag.From = wFrmDtO;
                ViewBag.To = wToDt;

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

                model = LabReportsRepository.LabRegisterReport(ReportType, wFrmDtO, wToDt, rdbIEWise, rdbPIE, rdbVendWise, rdbPVend, rdbLabWise, rdbPLab, rdbPending, rdbPaid, rdbDue, rdbPartlyPaid, lstTStatus, lstIE, ddlVender, lstLab, Region);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "LabReports", "LabRegisterReport", 1, GetIPAddress());
            }
            return PartialView(model);
        }

        public IActionResult LabPerformanceReport(string ReportType, string wFrmDtO, string wToDt)
        {
            LabReportsModel model = new LabReportsModel();
            try
            {
                ViewBag.From = wFrmDtO;
                ViewBag.To = wToDt;

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

                model = LabReportsRepository.LabPerformanceReport(ReportType, wFrmDtO, wToDt, Region);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "LabReports", "LabPerformanceReport", 1, GetIPAddress());
            }
            return PartialView(model);
        }

        public IActionResult LabPostingReport(string ReportType, string wFrmDtO, string wToDt)
        {
            LabReportsModel model = new LabReportsModel();
            try
            {
                ViewBag.From = wFrmDtO;
            ViewBag.To = wToDt;

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
            
            model = LabReportsRepository.LabPostingReport(ReportType, wFrmDtO, wToDt, Region);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "LabReports", "LabPostingReport", 1, GetIPAddress());
            }
            return PartialView(model);
        }

        public IActionResult OnlinePaymentReport(string ReportType, string wFrmDtO, string wToDt)
        {
            LabReportsModel model = new LabReportsModel();
            try
            {
                ViewBag.From = wFrmDtO;
            ViewBag.To = wToDt;

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
            model = LabReportsRepository.OnlinePaymentReport(ReportType, wFrmDtO, wToDt, Region);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "LabReports", "OnlinePaymentReport", 1, GetIPAddress());
            }
            return PartialView(model);
        }

        public IActionResult LabInvoiceReport(string ReportType, string wFrmDtO, string wToDt)
        {
            LabReportsModel model = new LabReportsModel();
            try
            {
                ViewBag.From = wFrmDtO;
            ViewBag.To = wToDt;

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
            model = LabReportsRepository.LabInvoiceReport(ReportType, wFrmDtO, wToDt, Region);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "LabReports", "LabInvoiceReport", 1, GetIPAddress());
            }
            return PartialView(model);
        }

        public IActionResult LabSamplePaymentReport(string ReportType, string wFrmDtO, string wToDt, string lstStatus, string rdbrecvdt)
        {
            LabReportsModel model = new LabReportsModel();
            try
            {
                ViewBag.From = wFrmDtO;
            ViewBag.To = wToDt;
            ViewBag.Date = DateTime.Now.ToString("dd/MM/yyyy");
            ViewBag.Time = DateTime.Now.ToShortTimeString();
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
            model = LabReportsRepository.LabSamplePaymentReport(ReportType, wFrmDtO, wToDt,Region,lstStatus, rdbrecvdt);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "LabReports", "LabSamplePaymentReport", 1, GetIPAddress());
            }
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
