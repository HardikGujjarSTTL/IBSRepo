using IBS.Filters;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class ComplaintsJIRequiredReportController : BaseController
    {
        #region Variables
        private readonly IComplaintsJIRequiredReportRepository complaintsJIRequiredReportRepository;
        #endregion

        public ComplaintsJIRequiredReportController(IComplaintsJIRequiredReportRepository _complaintsJIRequiredReportRepository)
        {
            complaintsJIRequiredReportRepository = _complaintsJIRequiredReportRepository;
        }
        [Authorization("ComplaintsJIRequiredReport", "Index", "view")]
        public IActionResult Index()
        {
            string Region = SessionHelper.UserModelDTO.Region;
            string wRegion = "";
            if (Region == "N") { wRegion = "Northern Region"; }
            else if (Region == "S") { wRegion = "Southern Region"; }
            else if (Region == "E") { wRegion = "Eastern Region"; }
            else if (Region == "W") { wRegion = "Western Region"; }
            else if (Region == "C") { wRegion = "Central Region"; }
            ViewBag.Regions = Region;
            return View();
        }

        public ActionResult GetClientType(string Clientwise)
        {
            var json = "";
            try
            {
                json = complaintsJIRequiredReportRepository.GetItems(Clientwise);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ComplaintsJIRequiredReport", "GetClientType", 1, GetIPAddress());
            }
            return Json(json);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            string Region = SessionHelper.UserModelDTO.Region;
            string wRegion = "";
            if (Region == "N") { wRegion = "Northern Region"; }
            else if (Region == "S") { wRegion = "Southern Region"; }
            else if (Region == "E") { wRegion = "Eastern Region"; }
            else if (Region == "W") { wRegion = "Western Region"; }
            else if (Region == "C") { wRegion = "Central Region"; }
            DTResult<JIRequiredReport> dTResult = complaintsJIRequiredReportRepository.GetJIRequiredList(dtParameters, Region);
            return Json(dTResult);
        }
    }
}
