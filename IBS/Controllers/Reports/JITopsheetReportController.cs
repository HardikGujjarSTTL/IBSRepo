using IBS.Filters;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Interfaces.Transaction;
using IBS.Models;
using IBS.Repositories.Transaction;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class JITopsheetReportController : BaseController
    {
        #region Variables
        private readonly IJITopsheetReportRepository jITopsheetReportRepository;
        #endregion
        public JITopsheetReportController(IJITopsheetReportRepository _jITopsheetReportRepository)
        {
            jITopsheetReportRepository = _jITopsheetReportRepository;
        }

        [Authorization("JITopsheetReport", "Index", "view")]
        public IActionResult Index()
        {
            string Region = SessionHelper.UserModelDTO.Region;
            string wRegion = "";
            if (Region == "N") { wRegion = "Northern Region"; }
            else if (Region == "S") { wRegion = "Southern Region"; }
            else if (Region == "E") { wRegion = "Eastern Region"; }
            else if (Region == "W") { wRegion = "Western Region"; }
            else if (Region == "C") { wRegion = "Central Region"; }
            ViewBag.Region = wRegion;
            return View();
        }

        public IActionResult Manage(string JISNO)
        {
            ConsigneeComplaints model = new()
            {
                JiSno = JISNO
            };
            model.ReportTitle = "JI Complaint Report";
            return View(model);
        }

        public IActionResult JIComplaintsReport(string JISNO)
        {
            string Region = SessionHelper.UserModelDTO.Region;
            string wRegion = "";
            if (Region == "N") { wRegion = "Northern Region"; }
            else if (Region == "S") { wRegion = "Southern Region"; }
            else if (Region == "E") { wRegion = "Eastern Region"; }
            else if (Region == "W") { wRegion = "Western Region"; }
            else if (Region == "C") { wRegion = "Central Region"; }
            ConsigneeComplaints model = jITopsheetReportRepository.GetComplaintReportDetails(JISNO, Region);
            ViewBag.Regions = wRegion;
            return PartialView(model);
        }

    }
}
