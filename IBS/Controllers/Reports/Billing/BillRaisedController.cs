using IBS.DataAccess;
using IBS.Interfaces.Reports.Billing;
using IBS.Interfaces.Vendor;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers.Reports.Billing
{
    public class BillRaisedController : BaseController
    {
        #region Variables
        private readonly IBillRaisedRepository billraisedRepository;
        #endregion
        public BillRaisedController(IBillRaisedRepository _billraisedRepository)
        {
            billraisedRepository = _billraisedRepository;
        }

        public IActionResult Index(string ActionType)
        {
            BillRaisedModel model = new();
            model.Region = Region;
            model.ActionType = ActionType;
            if(ActionType == "SWBills")
            {
                model.Title = "SECTOR WISE BILLING SUMMARY";
            }
            else if (ActionType == "CWBills")
            {
                model.Title = "CLIENT WISE BILLING SUMMARY";
            }
            else if (ActionType == "POCWBills")
            {
                model.Title = "PO/PURCHASER CLIENT WISE BILLING SUMMARY";
            }
            else if (ActionType == "CWCalls")
            {
                model.Title = "SUMMARY OF CLIENT WISE CALLS";
            }
            else if (ActionType == "R")
            {
                model.Title = "CLIENT WISE BILLING & REALISATION";
            }
            else if (ActionType == "RWB")
            {
                model.Title = "REGION WISE BILLING SUMMARY";
            }
            else if (ActionType == "CWR")
            {
                model.Title = "CLIENT WISE REALISATION";
            }
            else if (ActionType == "CRWB")
            {
                model.Title = "CLIENT AND REGION WISE BILLING SUMMARY";
            }
            else if (ActionType == "CDWB")
            {
                model.Title = "CLIENT AND DISCIPLINE WISE BILLING SUMMARY";
            }
            else if (ActionType == "CWOUTS")
            {
                model.Title = "CLIENT WISE OUTSTANDING SUMMARY FOR SERVICE TAX PURPOSE";
            }
            return View(model);
        }

        public IActionResult FetchData(BillRaisedModel model)
        {
            List<BillRaisedModel> billingData = billraisedRepository.GetBillingData(model);
            return Json(billingData);
        }


        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<BillRaisedModel> dTResult = billraisedRepository.GetDataList(dtParameters);
            return Json(dTResult);
        }

    }
}
