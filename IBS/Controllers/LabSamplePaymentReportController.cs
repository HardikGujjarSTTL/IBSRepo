using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Text.Json;

namespace IBS.Controllers
{
    public class LabSamplePaymentReportController : BaseController
    {
        #region Variables
        private readonly ILabSamplePaymentRptRepository LabSamplePaymentRptRepository;
        #endregion
        public LabSamplePaymentReportController(ILabSamplePaymentRptRepository _LabSamplePaymentRptRepository)
        {
            LabSamplePaymentRptRepository = _LabSamplePaymentRptRepository;
        }
        public IActionResult LabSamplePaymentReport()
        {

            return View();
        }
        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            string Regin = GetRegionCode;
            DTResult<LabSamplePaymentRptModel> dTResult = LabSamplePaymentRptRepository.GetPaymentReport(dtParameters, Regin);
            return Json(dTResult);
        }

    }
}
