using IBS.DataAccess;
using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using IBS.Models.Reports;
using IBS.Repositories;
using IBS.Repositories.Reports;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Drawing;
using System.Text.Json;

namespace IBS.Controllers
{
    public class SummaryVendorWiseInspController : BaseController
    {
        #region Variables
        private readonly ISummaryVendorWiseInspRepository SummaryVendorWiseInspRepository;
        #endregion
        public SummaryVendorWiseInspController(ISummaryVendorWiseInspRepository _SummaryVendorWiseInspRepository)
        {
            SummaryVendorWiseInspRepository = _SummaryVendorWiseInspRepository;
        }

        public IActionResult Index()
        {

            return View();
        }
        public IActionResult Manage(string ReportType, string Month, string Year,string ForGiven,string ReportBasedon,string MaterialValue,string FromDate,string ToDate,string ForParticular,string lstParticular)
        {
            SummaryVendorWiseInspModel model = new() { ReportType = ReportType, Month = Month, Year = Year, FromDt = FromDate, ToDt = ToDate,ForGiven = ForGiven,ReportBasedon = ReportBasedon,MaterialValue = MaterialValue,ForParticular = ForParticular,lstParticular = lstParticular };
            if (ReportType != "") model.ReportTitle = "Summary of Vendor Wise Inspections";
            return View(model);
        }
        public IActionResult SummaryVendorWiseInsp(string ReportType, string Month, string Year, string ForGiven, string ReportBasedon, string MaterialValue, string FromDate, string ToDate, string ForParticular, string lstParticular)
        {
            ViewBag.From = FromDate;
            ViewBag.To = ToDate;
            
            ViewBag.year = Year;
            if(Convert.ToBoolean(ReportBasedon) == true)
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
            SummaryVendorWiseInspModel model = SummaryVendorWiseInspRepository.SummaryVendorWiseInsp( ReportType,  Month,  Year,  ForGiven,  ReportBasedon,MaterialValue,  FromDate,  ToDate,  ForParticular,  lstParticular ,Region);
            return PartialView(model);
        }
        

    }
}
