using IBS.Interfaces;
using IBS.Interfaces.Reports.ManPower;
using IBS.Models;
using IBS.Repositories;
using IBS.Repositories.Reports.ManPower;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers.Reports.ManPower
{
    public class ManpowerMasterDataReportController : Controller
    {
        private readonly IManpowerMasterDataReportRepository manpowerMasterDataReportRepository;
        public ManpowerMasterDataReportController(IManpowerMasterDataReportRepository _manpowerMasterDataReportRepository)
        {
            manpowerMasterDataReportRepository = _manpowerMasterDataReportRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<ManpowerModel> dTResult = manpowerMasterDataReportRepository.GetManpowerMasterReportData(dtParameters);
            return Json(dTResult);
        }
    }
}
