using IBS.DataAccess;
using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using IBS.Models.Reports;
using IBS.Repositories;
using IBS.Repositories.Reports;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Drawing;
using System.Text.Json;

namespace IBS.Controllers
{
    public class DetailsofPurchaserWiseInspController : BaseController
    {
        #region Variables
        private readonly IDetailsofPurchaserWiseInspRepository DetailsofPurchaserWiseInspRepository;
        #endregion
        public DetailsofPurchaserWiseInspController(IDetailsofPurchaserWiseInspRepository _DetailsofPurchaserWiseInspRepository)
        {
            DetailsofPurchaserWiseInspRepository = _DetailsofPurchaserWiseInspRepository;
        }

        public IActionResult Index()
        {

            return View();
        }
        public IActionResult Manage(string ReportType, string Month, string Year,string ForGiven,string ReportBasedon,string FromDate,string ToDate,string ForParticular,string lstParticular, string TextPurchaser)
        {
            DetailsofPurchaserWiseInspModel model = new() { ReportType = ReportType, Month = Month, Year = Year, FromDt = FromDate, ToDt = ToDate,ForGiven = ForGiven,ReportBasedon = ReportBasedon,ForParticular = ForParticular,lstParticular = lstParticular,TextPurchase = TextPurchaser };
            if (ReportType != "") model.ReportTitle = "SUMMARY OF INSPECTIONS FOR RTI";
            return View(model);
        }
        public IActionResult SummaryofInsp(string ReportType, string Month, string Year, string ForGiven, string ReportBasedon, string FromDate, string ToDate, string ForParticular, string lstParticular, string TextPurchaser)
        {
            ViewBag.From = FromDate;
            ViewBag.To = ToDate;
            
            ViewBag.year = Year;
            if(Convert.ToBoolean(ReportBasedon) == true)
            {
                ViewBag.ReportBase = "Report Based on IC Date";
            }
            else
            {
                ViewBag.ReportBase = "Report Based on BILL Date";
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
            DetailsofPurchaserWiseInspModel model = DetailsofPurchaserWiseInspRepository.SummaryInsp( ReportType,  Month,  Year,  ForGiven,  ReportBasedon,  FromDate,  ToDate,  ForParticular,  lstParticular ,Region,TextPurchaser);
            return PartialView(model);
        }
        [HttpGet]
        public IActionResult GetPurchaserCd(string Purchaser)
        {
            try
            {
                List<SelectListItem> agencyClient = Common.GetPurchaserCd(Purchaser);
                return Json(new { status = true, list = agencyClient });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "DetailsofPurchaserWiseInsp", "GetPurchaserCd", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

    }
}
