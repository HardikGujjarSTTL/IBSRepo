﻿using IBS.Filters;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Interfaces.Reports;
using IBS.Models;
using IBS.Models.Reports;
using IBS.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers.Reports
{
    [Authorization]
    public class ReportsController : BaseController
    {
        #region Variables
        private readonly IReportsRepository reportsRepository;
        private readonly IIC_ReceiptRepository iC_ReceiptRepository;
        private readonly IWebHostEnvironment env;
        private readonly IConfiguration _config;
        #endregion

        public ReportsController(IReportsRepository _reportsRepository, IIC_ReceiptRepository _iC_ReceiptRepository, IWebHostEnvironment _environment, IConfiguration configuration)
        {
            reportsRepository = _reportsRepository;
            iC_ReceiptRepository = _iC_ReceiptRepository;
            env = _environment;
            _config = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult IECopy()
        {
            return View();
        }

        public IActionResult Get_Book_Set_Copy([FromBody] DTParameters dTParameters)
        {
            var data = reportsRepository.Get_IE_7thCopyList(dTParameters, GetUserInfo);
            return Json(data);
        }

        public IActionResult IE7thCopy(string Bk_No, string Set_No_Fr)        
        {
            ReportsModel model = new() { ReportType = "IE7thCopy_X", ReportTitle = "INSPECTION CERTIFICATE BOOK SET 7TH COPY REPORT" };
            return View(model);
        }

        public IActionResult Manage(string ReportType, DateTime FromDate, DateTime ToDate)
        {
            ReportsModel model = new() { ReportType = ReportType, FromDate = FromDate, ToDate = ToDate };
            if (ReportType == "UNBILLEDIC") model.ReportTitle = "IC RECEIVED IN OFFICE BUT NOT BILLED";
            return View(model);
        }

        public IActionResult FromToDate()
        {
            var action = Request.Query["actiontype"];
            ViewBag.Action = action;
            var partialView = "../IC_Receipt/IC_Unbilled_Partial";
            if (action == "UNBILLEDIC")
            {
                partialView = "../IC_Receipt/IC_Unbilled_Partial";
            }
            else if (action == "ICISSUEDNSUB")
            {
                partialView = ""; //"../IC_Receipt/IC_Issued_Partial";
            }
            else if (action == "PJI")
            {
                partialView = "../Reports/Pending_JI_Cases_Partial";
            }
            ViewBag.PartialView = partialView;
            ViewBag.RoleName = GetUserInfo.RoleName;
            return View();
        }

        #region Un Billed IC
        public IActionResult ICReceivedNotBilled(DateTime FromDate, DateTime ToDate)
        {
            return PartialView();
        }
        #endregion

        #region IC Issued But Not Received in Office
        public IActionResult ICIssuedNotReceived(string Type, string FromDate, string ToDate)
        {
            ICIssuedNotReceivedReportModel model = new() { ReportType = Type, FromDate = FromDate, ToDate = ToDate };
            model.ReportTitle = "IC Issued But Not Received in Office";
            return View(model);
        }

        public IActionResult IC_Issue(string Type, string FromDate, string ToDate, string ReportTitle)
        {
            var wRegion = "";
            var Region = SessionHelper.UserModelDTO.Region;
            if (Region == "N") { wRegion = "Northern Region"; }
            else if (Region == "S") { wRegion = "Southern Region"; }
            else if (Region == "E") { wRegion = "Eastern Region"; }
            else if (Region == "W") { wRegion = "Western Region"; }
            else if (Region == "C") { wRegion = "Central Region"; }
            ICIssuedNotReceivedReportModel model = new()
            {
                ReportType = Type,
                ReportTitle = ReportTitle,
                FromDate = FromDate,
                ToDate = ToDate
            };
            model.Region = wRegion;
            model.ICIssuedNotReceivedList = iC_ReceiptRepository.Get_IC_Issue_Not_Receive(FromDate, ToDate, GetUserInfo);
            foreach (var row in model.ICIssuedNotReceivedList)
            {
                var tifpath = Path.Combine(env.WebRootPath, "/RBS/CASE_NO/" + row.CASE_NO + ".TIF");
                var pdfpath = Path.Combine(env.WebRootPath, "/RBS/CASE_NO/" + row.CASE_NO + ".PDF");
                row.IsTIF = System.IO.File.Exists(tifpath) == true ? true : false;
                row.IsPDF = System.IO.File.Exists(pdfpath) == true ? true : false;
            }
            return PartialView("../IC_Receipt/IC_Issued_Partial", model);
        }
        #endregion

        public IActionResult Get_Pending_JI_Cases([FromBody] DTParameters dtParameters)
        {
            DTResult<PendingJICasesReportModel> dtList = new();
            try
            {
                var region = GetUserInfo.Region;
                var username = GetUserInfo.UserName;
                var iecd = Convert.ToString(GetUserInfo.IeCd);
                dtList = reportsRepository.Get_Pending_JI_Cases(dtParameters, iecd);

                foreach (var row in dtList.data)
                {
                    //var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "LAB", fileName);
                    var tempcasetifpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "COMPLAINTS_CASES" + row.CASE_NO + "-" + row.BK_NO + "-" + row.SET_NO + ".TIF");
                    var casetifpath = Path.Combine(env.WebRootPath, "/RBS/COMPLAINTS_CASES/" + row.CASE_NO + "-" + row.BK_NO + "-" + row.SET_NO + ".TIF");
                    var casepdfpath = Path.Combine(env.WebRootPath, "/RBS/COMPLAINTS_CASES/" + row.CASE_NO + "-" + row.BK_NO + "-" + row.SET_NO + ".PDF");

                    var reporttifpath = Path.Combine(env.WebRootPath, "/RBS/COMPLAINTS_REPORT/" + row.CASE_NO + "-" + row.BK_NO + "-" + row.SET_NO + ".TIF");
                    var reportpdfpath = Path.Combine(env.WebRootPath, "/RBS/COMPLAINTS_REPORT/" + row.CASE_NO + "-" + row.BK_NO + "-" + row.SET_NO + ".PDF");
                    row.IsCaseTIF = System.IO.File.Exists(casetifpath) == true ? true : false;
                    row.IsCasePDF = System.IO.File.Exists(casepdfpath) == true ? true : false;

                    row.IsReportTIF = System.IO.File.Exists(reporttifpath) == true ? true : false;
                    row.IsReportPDF = System.IO.File.Exists(reportpdfpath) == true ? true : false;
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Reports", "Get_Pending_JI_Cases", 1, GetIPAddress());
            }
            return Json(dtList);
        }

        [Authorization("Reports", "IEDairy", "view")]
        public IActionResult IEDairy()
        {
            var action = Convert.ToString(Request.Query["actiontype"]) == null ? "" : Convert.ToString(Request.Query["actiontype"]);
            ViewBag.Action = action;
            ViewBag.Region = Convert.ToString(GetUserInfo.Region);
            ViewBag.UserName = Convert.ToString(GetUserInfo.UserName);
            ViewBag.IECD = Convert.ToString(GetUserInfo.IeCd);
            return View();
        }

        public IActionResult Get_IEDairy([FromBody] DTParameters dtParameters)
        {
            DTResult<IEDairyModel> dtList = new();
            try
            {
                var region = GetUserInfo.Region;
                var username = GetUserInfo.UserName;
                var roleName = GetUserInfo.RoleName;
                var iecd = Convert.ToString(GetUserInfo.IeCd);
                dtList = reportsRepository.Get_IE_Dairy(dtParameters, GetUserInfo);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Reports", "Get_IEDairy", 1, GetIPAddress());
            }
            return Json(dtList);
        }

    }
}
