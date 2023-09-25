using IBS.Helper;
using IBS.Interfaces.Reports;
using IBS.Models;
using IBS.Models.Reports;
using IBS.Repositories.Reports;
using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp.Media;
using PuppeteerSharp;
using Humanizer;

namespace IBS.Controllers.Reports
{
    public class PCDOReportController : BaseController
    {
        private readonly IPCDOReportRepository pCDOReportController;
        private readonly IWebHostEnvironment env;

        public PCDOReportController(IPCDOReportRepository _pCDOReportController, IWebHostEnvironment env)
        {
            pCDOReportController = _pCDOReportController;
            this.env = env;
        }

        #region CO Highlight
        public IActionResult COHighlight()
        {
            return View();
        }
        public IActionResult TableCOHighlight(string month, string Year, string Monthtext)
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
                string CumYrPast, wYrMth, CumYrMth, bakdate, lstdate, sincedate, todate, priordate;
                wYrMth = Year + month;

                if (lst == 12)
                {
                    lstdate = Year + "01";
                }
                else if (lst == 1 || lst == 2 || lst == 3 || lst == 4 || lst == 5 || lst == 6 || lst == 7 || lst == 8)
                {
                    lstdate = Convert.ToString(result) + "0" + Convert.ToString(lstl);
                }
                else
                {
                    lstdate = Convert.ToString(result) + Convert.ToString(lstl);
                }

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
                ViewBag.Title = "Highlights upto the month of : " + Monthtext + " ," + Year + " ---- Status as on : " + DateTime.Now.ToString("dd/MM/yyy") + " - " + DateTime.Now.ToString("hh:mm: tt");
                COHighlightMainModel model = pCDOReportController.GetCOHighlightData(CumYrMth, wYrMth,byear.ToString(), dmonth, lstdate, CumYrPast, wYrMth_Past);
                return View();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "PCDOReport", "TableCOHighlight", 1, GetIPAddress());
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

        public IActionResult TableFinancialOutstanding(string month, string Year, string Monthtext)
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
                string CumYrPast, wYrMth, CumYrMth, bakdate, lstdate, sincedate, todate, priordate;
                wYrMth = Year + month;

                if (lst == 12)
                {
                    lstdate = Year + "01";
                }
                else if (lst == 1 || lst == 2 || lst == 3 || lst == 4 || lst == 5 || lst == 6 || lst == 7 || lst == 8)
                {
                    lstdate = Convert.ToString(result) + "0" + Convert.ToString(lstl);
                }
                else
                {
                    lstdate = Convert.ToString(result) + Convert.ToString(lstl);
                }

                if (lst == 1 || lst == 2 || lst == 3)
                {
                    CumYrMth = Convert.ToString(result) + z;
                    CumYrPast = Convert.ToString(cum_result) + z;
                    bakdate = Convert.ToString(result) + "03";
                    sincedate = "01." + "04." + Convert.ToString(result);
                    todate = "01." + "04." + Convert.ToString(cum_result) + " to " + "31." + "03." + Convert.ToString(result);
                    priordate = "01." + "04." + Convert.ToString(cum_result);

                }
                else
                {
                    CumYrMth = Year + z;
                    CumYrPast = Convert.ToString(result) + z;
                    bakdate = Year + "03";
                    sincedate = "01." + "04." + Year;
                    todate = "01." + "04." + Convert.ToString(result) + " to " + "31." + "03." + Year;
                    priordate = "01." + "04." + Convert.ToString(result);
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
                ViewBag.Title1 = "1.5 Outstanding prior to 30.09.2021";
                ViewBag.sincedate = sincedate;
                ViewBag.todate = todate;
                ViewBag.priordate = priordate;
                FinancialOutstandingMainModel model = pCDOReportController.GetFinancialOutstandingData(wYrMth_Past, CumYrPast, wYrMth, CumYrMth, bakdate, lstdate);
                return View(model);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "PCDOReport", "TableFinancialOutstanding", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
        #endregion

        #region Annexure-II Business Development - EOI/Priced Offers Sent
        public IActionResult EOIPricedOfferSent()
        {
            return View();
        }
        public IActionResult TableEOIPricedOfferSent(string month, string Year, string Monthtext)
        {
            try
            {
                string wYrMth, wHdr_YrMth;
                wYrMth = Year + month;
                wHdr_YrMth = month + ", " + Year;
                ViewBag.Title = "EOI/Priced offers sent during the month (in Descending order in terms of Inspection Fees) for the Period : " + wHdr_YrMth + " ---- Status as on : " + DateTime.Now.ToString("dd/MM/yyy") + " - " + DateTime.Now.ToString("hh:mm: tt");
                List<EOIPricedOfferSentModel> model = pCDOReportController.GetEOIPricedOfferSentData(wYrMth);
                return View(model);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "PCDOReport", "TableEOIPricedOfferSent", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
        #endregion

        #region Annexure-II Business Development - Contracts Secured
        public IActionResult ContractsSecured()
        {
            return View();
        }
        public IActionResult TableContractsSecured(string month, string Year, string Monthtext)
        {
            try
            {
                string wYrMth, wHdr_YrMth;
                wYrMth = Year + month;
                wHdr_YrMth = month + ", " + Year;
                ViewBag.Title = "Contracts secured during the month (in Descending order in terms of Inspection Fees) for the Period : " + wHdr_YrMth + " ---- Status as on : " + DateTime.Now.ToString("dd/MM/yyy") + " - " + DateTime.Now.ToString("hh:mm: tt");
                List<EOIPricedOfferSentModel> model = pCDOReportController.GetEOIPricedOfferSentData(wYrMth);
                return View(model);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "PCDOReport", "TableContractsSecured", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        #endregion

        #region Annexure-II Business Development - BD Efforts
        public IActionResult BDEfforts()
        {
            return View();
        }
        public IActionResult TableBDEfforts(string month, string Year, string Monthtext)
        {
            try
            {
                string wYrMth, wHdr_YrMth;
                wYrMth = Year + month;
                wHdr_YrMth = month + ", " + Year;
                ViewBag.Title = "BD Efforts for the period : " + wHdr_YrMth + " ---- Status as on : " + DateTime.Now.ToString("dd/MM/yyy") + " - " + DateTime.Now.ToString("hh:mm: tt");
                List<BDEffortsModel> model = pCDOReportController.GetBDEffortsData(wYrMth);
                return View(model);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "PCDOReport", "TableBDEfforts", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        #endregion

        #region Annexure-II Business Development - Previous offers still under consideration
        public IActionResult PreviousOfferSent()
        {
            return View();
        }

        public IActionResult TablePreviousOfferSent(string month, string Year, string Monthtext)
        {
            try
            {
                string wYrMth, wHdr_YrMth;
                wYrMth = Year + month;
                wHdr_YrMth = month + ", " + Year;
                ViewBag.Title = "Previous offers still under consideration, for the Period : " + wHdr_YrMth + " ---- Status as on : " + DateTime.Now.ToString("dd/MM/yyy") + " - " + DateTime.Now.ToString("hh:mm: tt");
                List<EOIPricedOfferSentModel> model = pCDOReportController.GetPreviousOfferSentData(wYrMth);
                return View(model);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "PCDOReport", "TablePreviousOfferSent", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
        #endregion

        #region Annexure-III Progress of Check sheets
        public IActionResult ProgressofChecksheets()
        {
            return View();
        }

        #endregion

        #region Annexure-IV Complaints
        public IActionResult Complaints()
        {
            return View();
        }

        #endregion

        #region Annexure-V Quality of Inspection
        public IActionResult QualityofInspection()
        {
            return View();
        }

        #endregion

        #region Annexure-V Quality of Inspection(Central)
        public IActionResult QualityofInspectionCentral()
        {
            return View();
        }

        #endregion

        #region Annexure-VI Improvement of Quality of Service
        public IActionResult ImprovementInQualityofService()
        {
            return View();
        }

        #endregion

        #region Annexure-VII Outstanding Railways 
        public IActionResult OutstandingRailways()
        {
            return View();
        }

        #endregion

        #region Annexure-VII Outstanding Non-Railways
        public IActionResult OutstandingNonRailways()
        {
            return View();
        }

        #endregion

        #region Annexure-VII Top 5 Outstanding Railway & Non-Railways
        public IActionResult Top5OutstandingRailwayNonRailways()
        {
            return View();
        }

        #endregion
        #region Annexure-VIII Client Contact
        public IActionResult ClientContact()
        {
            return View();
        }

        #endregion

        #region Annexure-VIII DFO Visit
        public IActionResult DFOVisit()
        {
            return View();
        }

        #endregion

        #region Annexure-IX Training
        public IActionResult Training()
        {
            return View();
        }

        #endregion

        #region Annexure-X Technical References
        public IActionResult TechnicalReferences()
        {
            return View();
        }

        #endregion

        #region PCDO Summary
        public IActionResult PCDOSummary()
        {
            return View();
        }

        #endregion



        [HttpPost]
        public async Task<IActionResult> GeneratePDF(string htmlContent)
        {
            await new BrowserFetcher().DownloadAsync();
            await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
                DefaultViewport = null
            });
            await using var page = await browser.NewPageAsync();
            await page.EmulateMediaTypeAsync(MediaType.Screen);
            await page.SetContentAsync(htmlContent);

            string cssPath = env.WebRootPath + "/css/report.css";

            AddTagOptions bootstrapCSS = new AddTagOptions() { Path = cssPath };
            await page.AddStyleTagAsync(bootstrapCSS);

            var pdfContent = await page.PdfStreamAsync(new PdfOptions
            {
                Landscape = true,
                Format = PaperFormat.Letter,
                PrintBackground = true
            });

            await browser.CloseAsync();

            return File(pdfContent, "application/pdf", Guid.NewGuid().ToString() + ".pdf");
        }
    }
}
