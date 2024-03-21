using IBS.Interfaces;
using IBS.Interfaces.Reports.ManPower;
using IBS.Models;
using IBS.Repositories;
using IBS.Repositories.Reports.ManPower;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers.Reports.ManPower
{
    public class ManpowerIEAndCallReportController : BaseController
    {
        private readonly IManpowerMasterDataReportRepository manpowerMasterDataReportRepository;
        public ManpowerIEAndCallReportController(IManpowerMasterDataReportRepository _manpowerMasterDataReportRepository)
        {
            manpowerMasterDataReportRepository = _manpowerMasterDataReportRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult FillDetails(string P_FROMDATE,string P_TODATE)
        {
            try
            {
                List<IEAndCallReport> models = new();
                models = manpowerMasterDataReportRepository.GetIEAndCallReport(P_FROMDATE, P_TODATE);
                return PartialView("_IEAndCallReport", models);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ManpowerIEAndCallReport", "FillDetails", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }


    }
}
