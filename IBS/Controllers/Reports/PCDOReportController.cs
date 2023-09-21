using IBS.Helper;
using IBS.Interfaces.Reports;
using IBS.Models;
using IBS.Models.Reports;
using IBS.Repositories.Reports;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers.Reports
{
    public class PCDOReportController : BaseController
    {
        private readonly IPCDOReportRepository pCDOReportController;

        public PCDOReportController(IPCDOReportRepository _pCDOReportController)
        {
            pCDOReportController = _pCDOReportController;
        }

        #region CO Highlight
        public IActionResult COHighlight()
        {
            return View();
        }
        public IActionResult TableCOHighlight(string p_wYrMth, string Year, string Monthtext)
        {
            try
            {
                ViewBag.Title = "Highlights upto the month of : " + Monthtext + " ," + Year + " ---- Status as on : " + DateTime.Now.ToString("dd/MM/yyy") + " - " + DateTime.Now.ToString("hh:mm: tt");
              //List<HighlightReportsModel> model = pCDOReportController.GetHighlightData(p_wYrMth);
                return View();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "PCDOReport", "TableHighlight", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
        #endregion

        #region Highlight
        public IActionResult Highlight()
        {
            return View();
        }
        public IActionResult TableHighlight(string p_wYrMth,string Year, string Monthtext)
        {
            try
            {
                ViewBag.Title = "Highlights for the period : "+ Monthtext +" ,"+ Year + " ---- Status as on : " + DateTime.Now.ToString("dd/MM/yyy")  +" - "+ DateTime.Now.ToString("hh:mm: tt");
                List <HighlightReportsModel> model = pCDOReportController.GetHighlightData(p_wYrMth);
                return View(model);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "PCDOReport", "TableHighlight", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
        #endregion

        #region Financial(Billing)
        public IActionResult FinancialBilling()
        {
            return View();
        }
        public IActionResult TableFinancialBilling(string month, string Year,string Monthtext)
        {
            try
            {
                Int64 result;
                Int64 x = Convert.ToInt64(Year);
                Int64 y = 1;
                //new for year//
                Int64 a = 2;
                Int64 cum_result;
                cum_result = (x - a);
                //
                result = (x - y);
                string z = "04";
                Int64 lst = Convert.ToInt64(month);
                Int64 lstl = lst + 1;
                

                string wYrMth_Past = Convert.ToString(result) + month;
                string CumYrPast, wYrMth, CumYrMth;
                wYrMth = Year + month;
                if (lst == 1 || lst == 2 || lst == 3)
                {
                    CumYrMth = Convert.ToString(result) + z;
                    CumYrPast = Convert.ToString(cum_result) + z;
                }
                else
                {
                    CumYrMth = Year + z;
                    CumYrPast = Convert.ToString(result) + z;
                }
                int dst = 0, dmonth = 0, byear = 0;
                dst = Convert.ToInt16(month);
                byear = Convert.ToInt16(Year);
                dmonth = (dst - 3);
                if (dst == 1 || dst == 2 || dst == 3)
                {
                    dmonth = (dst + 9);
                    byear = (byear - 1);
                }

                ViewBag.Title = "Financial Figure Upto : " + Monthtext + " ," + Year + " ---- Status as on : " + DateTime.Now.ToString("dd/MM/yyy") + " - " + DateTime.Now.ToString("hh:mm: tt");
                List<FinancialBillingModel> model = pCDOReportController.GetFinancialBillingData(dmonth, wYrMth_Past, CumYrPast, wYrMth, CumYrMth, byear);
                return View(model);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "PCDOReport", "TableFinancialBilling", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
        #endregion

        #region Annexure-I Financial(Expenditure & Realization)
        public IActionResult FinancialExpenditureRealization()
        {
            return View();
        }
        public IActionResult TableFinancialExpenditureRealization(string month, string Year, string Monthtext)
        {
            try
            {
                Int64 result;
                Int64 x = Convert.ToInt64(Year);
                Int64 y = 1;
                //new for year//
                Int64 a = 2;
                Int64 cum_result;
                cum_result = (x - a);
                //
                result = (x - y);
                string z = "04";
                Int64 lst = Convert.ToInt64(month);
                Int64 lstl = lst + 1;


                string wYrMth_Past = Convert.ToString(result) + month;
                string CumYrPast, wYrMth, CumYrMth;
                wYrMth = Year + month;
                if (lst == 1 || lst == 2 || lst == 3)
                {
                    CumYrMth = Convert.ToString(result) + z;
                    CumYrPast = Convert.ToString(cum_result) + z;
                }
                else
                {
                    CumYrMth = Year + z;
                    CumYrPast = Convert.ToString(result) + z;
                }
                int dst = 0, dmonth = 0, byear = 0;
                dst = Convert.ToInt16(month);
                byear = Convert.ToInt16(Year);
                dmonth = (dst - 3);
                if (dst == 1 || dst == 2 || dst == 3)
                {
                    dmonth = (dst + 9);
                    byear = (byear - 1);
                }

                ViewBag.Title = "Financial Figure Upto : " + Monthtext + " ," + Year + " ---- Status as on : " + DateTime.Now.ToString("dd/MM/yyy") + " - " + DateTime.Now.ToString("hh:mm: tt");
                FinancialExpenditureRealizationMainModel model = pCDOReportController.GetFinancialExpenditureRealizationData(wYrMth_Past, CumYrPast, wYrMth, CumYrMth, byear);
                return View(model);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "PCDOReport", "TableFinancialExpenditureRealization", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
        #endregion

        #region Annexure-I Financial(Outstanding)
        public IActionResult FinancialOutstanding()
        {
            return View();
        }

        #endregion
    }
}
