﻿using IBS.Interfaces.Reports.Billing;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IBS.Controllers.Reports.Billing
{
    public class BillingReportsController : BaseController
    {
        #region Variables
        private readonly IBillRaisedRepository billraisedRepository;
        #endregion
        public BillingReportsController(IBillRaisedRepository _billraisedRepository)
        {
            billraisedRepository = _billraisedRepository;
        }

        public IActionResult Index(string ActionType)
        {
            BillRaisedModel model = new();
            model.Region = Region;
            model.ActionType = ActionType;
            if (ActionType == "SWBills")
            {
                model.Title = "Sector wise billing summary";
            }
            else if (ActionType == "CWBills")
            {
                model.Title = "Client wise billing summary";
            }
            else if (ActionType == "POCWBills")
            {
                model.Title = "Po/purchaser client wise billing summary";
            }
            else if (ActionType == "CWCalls")
            {
                model.Title = "Summary of client wise calls";
            }
            else if (ActionType == "R")
            {
                model.Title = "Client wise billing & realisation";
            }
            else if (ActionType == "RWB")
            {
                model.Title = "Region wise billing summary";
            }
            else if (ActionType == "CWR")
            {
                model.Title = "Client wise realisation";
            }
            else if (ActionType == "CRWB")
            {
                model.Title = "Client and region wise billing summary";
            }
            else if (ActionType == "CDWB")
            {
                model.Title = "Client and discipline wise billing summary";
            }
            else if (ActionType == "CWOUTS")
            {
                model.Title = "Client wise outstanding summary for service tax purpose";
            }

            else if (ActionType == "RlyBills")
            {
                model.Title = "Railway online bills";
            }
            else if (ActionType == "NSC")
            {
                model.Title = "Bills Not Submitted to CRIS";
            }
            else if (ActionType == "RBNRS")
            {
                model.Title = "Returned Bills yet to be Submitted to CRIS (Under Testing)";
            }

            return View(model);
        }

        public IActionResult BillingClientReport(int FromMn, int FromYr, int ToMn, int ToYr, string ActionType, string rdo)
        {
            BillRaisedModel model = billraisedRepository.GetBillingClient(FromMn, FromYr, ToMn, ToYr, ActionType, rdo, Region);
            return View(model);
        }

        public IActionResult BillingSectorReport(int FromMn, int FromYr, int ToMn, int ToYr, string ActionType, string rdo, string IncRites)
        {
            BillRaisedModel model = billraisedRepository.GetBillingSector(FromMn, FromYr, ToMn, ToYr, ActionType, rdo, Region, IncRites);
            return View(model);
        }

        public IActionResult RailwayOnlineReport(string ClientType, string rdoSummary, string BpoRly, string rdoBpo, int FromMn, int FromYr, DateTime? FromDt, DateTime? ToDt, string ActionType,string chkRegion)
        {
            BillRaisedModel model = billraisedRepository.GetRailwayOnline(ClientType, rdoSummary, BpoRly, rdoBpo, FromMn, FromYr, FromDt, ToDt, ActionType,Region, chkRegion);
            return View(model);
        }

        [HttpGet]
        public IActionResult GetAU(string RlyCd)
        {
            try
            {
                List<SelectListItem> lstAu = Common.GetAUCrisByRlyCd(RlyCd);
                return Json(new { status = true, list = lstAu });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "BillingReports", "GetAU", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        public IActionResult BillsNotCrisReport(DateTime FromDate, DateTime ToDate, string chkRegion,string ClientType, string lstAU, string actiontype,string rdbPRly, string rdbPAU)
        {
            BillRaisedModel model = billraisedRepository.GetBillsNotCris(FromDate, ToDate, chkRegion, ClientType, lstAU, actiontype,Region, rdbPRly, rdbPAU);
            return View(model);
        }

    }
}