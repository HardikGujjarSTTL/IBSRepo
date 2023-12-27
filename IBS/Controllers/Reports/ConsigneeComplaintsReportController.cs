using IBS.Interfaces.Reports;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers.Reports
{
    public class ConsigneeComplaintsReportController : BaseController
    {
        #region Variables
        private readonly IConsigneeComplaintsReportRepository consigneeComplaintsReportRepository;
        private readonly IWebHostEnvironment env;
        public ConsigneeComplaintsReportController(IConsigneeComplaintsReportRepository _consigneeComplaintsReportRepository, IWebHostEnvironment _environment)
        {
            this.consigneeComplaintsReportRepository = _consigneeComplaintsReportRepository;
            env = _environment;
        }
        #endregion

        public IActionResult Index()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        //{
        //    DTResult<ConsigneeComplaintsReportModel> dtResult = consigneeComplaintsReportRepository.Get_Consignee_Complaints(dtParameters, GetUserInfo);
        //    var data = dtResult.data;
        //    foreach (var item in data)
        //    {
        //        var fileName = item.CASE_NO + "-" + item.BK_NO + "-" + item.SET_NO;
        //        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", Enums.GetEnumDescription(Enums.FolderPath.ConsigneeComplaints), fileName);
        //        var tif = Path.Combine(env.WebRootPath, Enums.GetEnumDescription(Enums.FolderPath.ConsigneeComplaints), fileName);
        //        //item.REJECTIONMEMOPATH =
        //    }
        //    return Json(dtResult);
        //}

        public IActionResult Report(string FromDate, string ToDate)
        {
            ConsigneeReportsModel model = new() { ReportType = "", FromDate = Convert.ToDateTime(FromDate), ToDate = Convert.ToDateTime(ToDate) };
            model.ReportTitle = "Consignee Complaints Report";
            ViewBag.FromDate = FromDate;
            ViewBag.ToDate = ToDate;
            return View(model);
        }

        public IActionResult ConsigneeComplaints(string FromDate, string ToDate)
        {
            var data = consigneeComplaintsReportRepository.Get_Consignee_Complaints(FromDate, ToDate, GetUserInfo);
            ViewBag.FromDate = FromDate;
            ViewBag.ToDate = ToDate;
            return PartialView(data);
        }
    }
}
