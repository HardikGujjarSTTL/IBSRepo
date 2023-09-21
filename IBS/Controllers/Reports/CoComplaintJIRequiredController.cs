using IBS.Helper;
using IBS.Interfaces;
using IBS.Interfaces.Reports;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers.Reports
{
    public class CoComplaintJIRequiredController : BaseController
    {
        #region Variables
        private readonly ICoComplaintJIRequiredRepository coComplaintJIRequiredRepository;
        #endregion
        public CoComplaintJIRequiredController(ICoComplaintJIRequiredRepository _coComplaintJIRequiredRepository)
        {
            coComplaintJIRequiredRepository = _coComplaintJIRequiredRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Manage(string FinancialYearsText,string FinancialYearsValue)
        {
            JIRequiredReport model = new() { FinancialYearsText = FinancialYearsText, FinancialYearsValue= FinancialYearsValue };
            model.ReportTitle = "JI Complaints Report";
            return View(model);
        }

        public IActionResult JICompReport(string FinancialYearsText, string FinancialYearsValue)
        {
            string Region = SessionHelper.UserModelDTO.Region;
            string wRegion = "";
            if (Region == "N") { wRegion = "Northern Region"; }
            else if (Region == "S") { wRegion = "Southern Region"; }
            else if (Region == "E") { wRegion = "Eastern Region"; }
            else if (Region == "W") { wRegion = "Western Region"; }
            else if (Region == "C") { wRegion = "Central Region"; }
            JIRequiredReport model = coComplaintJIRequiredRepository.GetJIComplaintsList(FinancialYearsText,FinancialYearsValue);
            ViewBag.Financialperiod = FinancialYearsText;
            ViewBag.Regions = wRegion;
            return PartialView(model);
        }
    }
}
