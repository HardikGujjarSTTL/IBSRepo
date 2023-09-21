using IBS.Filters;
using IBS.Interfaces.Reports;
using IBS.Interfaces;
using Microsoft.AspNetCore.Mvc;
using IBS.Interfaces.Reports.RealisationPayment;
using IBS.Models;
using IBS.Models.Reports;

namespace IBS.Controllers.Reports.RealisationPayments
{
    [Authorization]
    public class RealisationPaymentController : BaseController
    {
        #region Variables
        private readonly IRealisationPaymentRepository realisationPaymentRepository;
        private readonly IWebHostEnvironment env;
        private readonly IConfiguration _config;
        #endregion

        public RealisationPaymentController(IRealisationPaymentRepository _realisationPaymentRepository, IWebHostEnvironment _environment, IConfiguration configuration)
        {
            realisationPaymentRepository = _realisationPaymentRepository;
            env = _environment;
            _config = configuration;
        }
        public IActionResult Index()
        {            
            return View();
        }

        public IActionResult Manage(string ReportType, DateTime FromDate, DateTime ToDate)
        {
            ManagementReportsModel model = new() { ReportType = ReportType, FromDate = FromDate, ToDate = ToDate };
            if (ReportType == "ONLINENRPAYMENTS") model.ReportTitle = "Summary Online Payment";
            return View(model);
        }

        public IActionResult SummaryOnlinePayment(DateTime FromDate, DateTime ToDate)
        {
            SummaryOnlinePaymentModel model = realisationPaymentRepository.GetSummaryOnlinePayment(FromDate, ToDate, Region);
            return PartialView(model);
        }
    }
}
