using IBS.Filters;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Interfaces.Reports;
using IBS.Models;
using IBS.Models.Reports;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers.Reports
{
    public class IEICPhotoEnclosedReportController : BaseController
    {
        private readonly IIEICPhotoEnclosedReportRepository iIEICPhotoEnclosedReportRepository;
        public IEICPhotoEnclosedReportController(IIEICPhotoEnclosedReportRepository _iEICPhotoEnclosedReportRepository)
        {
            iIEICPhotoEnclosedReportRepository = _iEICPhotoEnclosedReportRepository;
        }
        [Authorization("IEICPhotoEnclosedReport", "Index", "view")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            string Region = SessionHelper.UserModelDTO.Region;
            DTResult<IEICPhotoEnclosedModelReport> dTResult = iIEICPhotoEnclosedReportRepository.GetDataList(dtParameters,Region);
            return Json(dTResult);
        }
    }
}
