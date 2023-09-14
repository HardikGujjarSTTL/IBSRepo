using IBS.Helper;
using IBS.Interfaces;
using IBS.Interfaces.Reports;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;
using static IBS.Helper.Enums;

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

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<ConsigneeComplaintsReportModel> dtResult = consigneeComplaintsReportRepository.Get_Consignee_Complaints(dtParameters, GetUserInfo);
            var data = dtResult.data;

            foreach (var item in data)
            {
                var fileName = item.CASE_NO + "-" + item.BK_NO + "-" + item.SET_NO;
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", Enums.GetEnumDescription(Enums.FolderPath.ConsigneeComplaints), fileName);
                var tif = Path.Combine(env.WebRootPath, Enums.GetEnumDescription(Enums.FolderPath.ConsigneeComplaints), fileName);
                //item.REJECTIONMEMOPATH =
            }
            return Json(dtResult);
        }
        [HttpGet]
        public IActionResult Report(string FromDate, string ToDate)
        {
            //string FromDate = null, ToDate = null;
            if (Request.Query["FromDate"] != "")
            {
                FromDate = Convert.ToString(Request.Query["FromDate"]);
            }
            if (Request.Query["ToDate"] != "")
            {
                ToDate = Convert.ToString(Request.Query["ToDate"]);
            }
            ViewBag.FromDate = FromDate;
            ViewBag.ToDate = ToDate;
            DTParameters dtParameters = new DTParameters();
            var dtResult = consigneeComplaintsReportRepository.Get_Consignee_Complaints(dtParameters, FromDate, ToDate, GetUserInfo);
            ViewBag.DataList = dtResult;
            return View(dtResult);
        }
    }
}
