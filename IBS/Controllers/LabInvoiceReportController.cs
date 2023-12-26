//using CrystalDecisions.CrystalReports.Engine;
//using CrystalDecisions.Shared;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class LabInvoiceReportController : BaseController
    {
        #region Variables
        private readonly ILabInvoiceReportRepository LabInvoiceReportRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        #endregion
        public LabInvoiceReportController(ILabInvoiceReportRepository _LabInvoiceReportRepository, IWebHostEnvironment webHostEnvironment)
        {
            LabInvoiceReportRepository = _LabInvoiceReportRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult LabInvoiceReport()
        {

            return View();
        }
        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            string Regin = GetRegionCode;
            DTResult<LabInvoiceReportModel> dTResult = new DTResult<LabInvoiceReportModel>();
            try
            {
                dTResult = LabInvoiceReportRepository.LabInvoiceReport(dtParameters, Regin);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "LabInvoiceReport", "LoadTable", 1, GetIPAddress());
            }
            return Json(dTResult);
        }

    }
}
