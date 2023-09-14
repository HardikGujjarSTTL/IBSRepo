using IBS.Interfaces.Reports.Billing;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

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
    }
}
