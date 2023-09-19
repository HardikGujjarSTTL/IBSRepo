using IBS.Filters;
using IBS.Interfaces.Reports;
using IBS.Models;
using IBS.Models.Reports;
using IBS.Repositories.Reports;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static IBS.Helper.Enums;

namespace IBS.Controllers.Reports
{
    [Authorization]
    public class RemitanceReportsController : BaseController
    {

        private readonly IRemitanceReportsRepository remitanceReportsRepository;

        public RemitanceReportsController(IRemitanceReportsRepository _remitanceReportsRepository)
        {
            remitanceReportsRepository = _remitanceReportsRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Manage(string ReportType, DateTime FromDate, DateTime ToDate, string AccCode, string RReport)
        {
            RemitanceReportsModel model = new() { ReportType = ReportType, FromDate = FromDate, ToDate = ToDate, AccCode = AccCode, RReport = RReport };
            if (ReportType == "R")
            {
                model.ReportTitle = "Remitance Reports";
                if (RReport == "Report1")
                    model.ReportTitle = "Cheque Wise Remittances for the Period";
                else if (RReport == "Report2")
                    model.ReportTitle = "Bill Wise Remittances for the Period";

            }

            return View(model);
        }

        public IActionResult RemitanceReport(DateTime FromDate, DateTime ToDate, string AccCode)
        {
            RemitanceModel model = remitanceReportsRepository.GetRemitanceReport(FromDate, ToDate, AccCode, Region);
            return PartialView(model);
        }

        public IActionResult BillWiseRemittancesPeriodReport(DateTime FromDate, DateTime ToDate, string AccCode)
        {
            RemitanceModel model = remitanceReportsRepository.GetRemitanceReport(FromDate, ToDate, AccCode, Region);
            return PartialView(model);
        }

        #region Other Event
        [HttpGet]
        public IActionResult GetBPORlyCd(string ClientType)
        {
            try
            {
                List<SelectListItem> lst = Common.GetBPORlyCd(ClientType);
                return Json(new { status = true, list = lst });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "RemitanceReports", "GetBPORlyCd", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpGet]
        public IActionResult GetlstBPOType(string ClientType, string ClientName)
        {
            try
            {
                List<SelectListItem> lst = Common.GetlstBPOType(ClientType, ClientName);
                return Json(new { status = true, list = lst });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "RemitanceReports", "GetlstBPOType", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
        #endregion
    }
}
