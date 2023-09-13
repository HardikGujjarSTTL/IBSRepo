using IBS.Helper;
using IBS.Interfaces;
using IBS.Interfaces.Reports;
using IBS.Models;
using IBS.Repositories;
using IBS.Repositories.Reports;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IBS.Controllers.Reports.Queries
{
    public class PurchaseOrdersofSpecificValuesController : BaseController
    {
        private readonly IPurchaseOrdersofSpecificValuesRepository purchaseOrdersofSpecificValuesRepository;

        public PurchaseOrdersofSpecificValuesController(IPurchaseOrdersofSpecificValuesRepository _purchaseOrdersofSpecificValuesRepository)
        {
            purchaseOrdersofSpecificValuesRepository = _purchaseOrdersofSpecificValuesRepository;
        }
        public IActionResult DetailedReport()
        {
            return View();
        }
        public IActionResult TableDetailed(string p_wAgency, string p_frmDt, string p_toDt, string p_SelCriteria, string p_wClient, string p_wFrmAmt, string p_wToAmt)
        {
            try
            {
                string Region = SessionHelper.UserModelDTO.Region;
                string Regiontext = "";
                if (Region == "N")
                    Regiontext = "Northern Region";
                if (Region == "S")
                    Regiontext = "Southern Region";
                if (Region == "E")
                    Regiontext = "Eastern Region";
                if (Region == "W")
                    Regiontext = "Westrern Region";
                if (Region == "C")
                    Regiontext = "Central Region";
                if (Region == "Q")
                    Regiontext = "QA Corporate";

                string wPoValue = "";
                if (Convert.ToInt32(p_wFrmAmt) > 0)
                { wPoValue = "between " + p_wFrmAmt + " and " + p_wToAmt; }
                else
                { wPoValue = " less than or equal to " + p_wToAmt; }

                ViewBag.Region = Regiontext;
                ViewBag.Title = p_wClient + " (" + Regiontext + ") Purchase Orders of Value " + wPoValue + " for the Period : " + p_frmDt + " TO " + p_toDt;
                List<PurchaseOrdersofSpecificValueModel> model = purchaseOrdersofSpecificValuesRepository.GetDataList(p_wAgency, p_frmDt, p_toDt, p_SelCriteria, p_wClient, p_wFrmAmt, p_wToAmt, Region);
                //return PartialView("_TableDetailed", model);
                return View(model);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "PurchaseOrdersofSpecificValues", "TableDetailed", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        public IActionResult SummaryReport()
        {
            return View();
        }

        public IActionResult TableSummary(string p_wAgency, string p_frmDt, string p_toDt, string p_SelCriteria, string p_wFrmAmt, string p_wToAmt)
        {
            try
            {
                string Region = SessionHelper.UserModelDTO.Region;
                string Regiontext = "";
                if (Region == "N")
                    Regiontext = "Northern Region";
                if (Region == "S")
                    Regiontext = "Southern Region";
                if (Region == "E")
                    Regiontext = "Eastern Region";
                if (Region == "W")
                    Regiontext = "Westrern Region";
                if (Region == "C")
                    Regiontext = "Central Region";
                if (Region == "Q")
                    Regiontext = "QA Corporate";

                string wlAgency = "";

                if (p_wAgency == "R")
                {
                    wlAgency = "Railway";
                }
                else if (p_wAgency == "P")
                {
                    wlAgency = "Private";
                }
                else if (p_wAgency == "U")
                {
                    wlAgency = "PSU";
                }
                else if (p_wAgency == "F")
                {
                    wlAgency = "Foreign Railway";
                }
                else if (p_wAgency == "S")
                {
                    wlAgency = "State Government";
                }

                string wPoValue = "";
                if (Convert.ToInt32(p_wFrmAmt) > 0)
                { wPoValue = "between " + p_wFrmAmt + " and " + p_wToAmt; }
                else
                { wPoValue = " less than or equal to " + p_wToAmt; }

                ViewBag.Region = Regiontext;
                ViewBag.Title = wlAgency + " Purchase Orders of Value " + wPoValue + " for the Period : " + p_frmDt + " TO " + p_toDt;
                List<PurchaseOrdersofSummaryModel> model = purchaseOrdersofSpecificValuesRepository.GetSummaryDataList(p_wAgency, p_frmDt, p_toDt, p_SelCriteria,  p_wFrmAmt, p_wToAmt, Region);
                return View(model);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "PurchaseOrdersofSpecificValues", "TableSummary", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

    }
}
