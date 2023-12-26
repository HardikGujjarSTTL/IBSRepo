using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class Calls_Marked_ReportController : BaseController
    {
        private readonly ICalls_Marked_ReportRepository callmarkedreportrepository;
        public Calls_Marked_ReportController(ICalls_Marked_ReportRepository _callmarkedreportrepository)
        {
            callmarkedreportrepository = _callmarkedreportrepository;
        }
        public IActionResult Index(string pDtFr, string pDtTo, string pRegion, string pSortKey, int UserID)
        {
            ViewBag.Uname = UserId;
            ViewBag.pDtFr = pDtFr;
            ViewBag.pTTo = pDtTo;
            string wRegion = pDtTo;
            wRegion = GetRegionCode;
            UserID = UserId;
            string wRgn_Name = "";

            if (pSortKey == "V")
            {
                ViewBag.sortOrder = "VENDOR"; // Sort by vendor name
                ViewBag.sortHeader = "Report Sorted on Vendor Name";
            }
            else
            {
                ViewBag.sortOrder = "CALL_MARK_DT"; // Sort by call date
                ViewBag.sortHeader = "Report Sorted on Call Date";
            }


            if (wRegion == "N")
            { wRgn_Name = "NORTHERN REGION"; }
            else if (wRegion == "W")
            { wRgn_Name = "WESTERN REGION"; }
            else if (wRegion == "E")
            { wRgn_Name = "EASTERN REGION"; }
            else if (wRegion == "S")
            { wRgn_Name = "SOUTHERN REGION"; }
            else if (wRegion == "C")
            { wRgn_Name = "CENTRAL REGION"; }
            else
            { wRgn_Name = "--x--"; }
            Calls_Marked_ReportModel dTResult = callmarkedreportrepository.Query1(pDtFr, pDtTo, wRegion, pSortKey, UserID, wRgn_Name);
            return View(dTResult);
        }

    }
}
