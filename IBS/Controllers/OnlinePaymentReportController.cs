//using CrystalDecisions.CrystalReports.Engine;
//using CrystalDecisions.Shared;
using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Drawing;
using System.Text.Json;

namespace IBS.Controllers
{
    public class OnlinePaymentReportController : BaseController
    {
        #region Variables
        private readonly IOnlinePaymentReportRepository OnlinePaymentReportRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        #endregion
        public OnlinePaymentReportController(IOnlinePaymentReportRepository _OnlinePaymentReportRepository, IWebHostEnvironment webHostEnvironment)
        {
            OnlinePaymentReportRepository = _OnlinePaymentReportRepository;
            _webHostEnvironment = webHostEnvironment;
        }
        
        public IActionResult OnlinePaymentReport()
        {
            
            return View();
        }        
        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            string Regin = GetRegionCode;
            DTResult<OnlinePaymentReportModel> dTResult = OnlinePaymentReportRepository.OnlinePaymentReport(dtParameters, Regin);          
            return Json(dTResult);
        }
        
    }
}
