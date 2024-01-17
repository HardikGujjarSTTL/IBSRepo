using IBS.Interfaces.Reports;
using IBS.Interfaces.Reports.Billing;
using IBS.Models;
using IBS.Repositories.Reports.Billing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace IBS.Controllers.Reports
{
    public class FinanceReportsController : BaseController
    {
        #region Variables
        private readonly IFinanceReportsRepository financeRepository;
        #endregion

        public FinanceReportsController(IFinanceReportsRepository _financeRepository)
        {
            financeRepository = _financeRepository;
        }

        public IActionResult Index(string AccCd)
        {
            FinanceReportModel model = new FinanceReportModel();
            model.Region = Region;
            model.Acc_Cd = AccCd;
            if(AccCd == "9998")
            {
                model.Title = "Miscellenous Adjustments";
            }
            else if (AccCd == "9979")
            {
                model.Title = "Refund";
            }
            return View(model);
        }

        #region Finance Report
        public IActionResult FinanceReport(DateTime? FromDate, DateTime? ToDate, string AccCd)
        {
            FinanceReportModel model = financeRepository.GetFinanceReport(FromDate, ToDate, AccCd, Region);
            return View(model);
        }
        #endregion
    }
}
