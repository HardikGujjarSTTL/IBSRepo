using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
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
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<BillRaisedModel> dTResult = billraisedRepository.GetDataList(dtParameters);
            return Json(dTResult);
        }

        public IActionResult BillRaisedReport(int FromMn, int FromYr, int ToMn, int ToYr, string ActionType, string rdo)
        {
            BillRaisedModel model = new();
            model.Region = Region;
            if (ActionType == "SWBills")
                model.Title = "Sector wise billing summary";
            if (ActionType == "CWBills")
                model.Title = "Client wise billing summary";
            if (ActionType == "POCWBills")
                model.Title = "Po/purchaser client wise billing summary";
            if (ActionType == "CWCalls")
                model.Title = "Summary of client wise calls";
            if (ActionType == "R")
                model.Title = "Client wise billing & realisation";
            if (ActionType == "RWB")
                model.Title = "Region wise billing summary";
            if (ActionType == "CWR")
                model.Title = "Client wise realisation";
            if (ActionType == "CRWB")
                model.Title = "Client and region wise billing summary";
            if (ActionType == "CDWB")
                model.Title = "Client and discipline wise billing summary";
            if (ActionType == "CWOUTS")
                model.Title = "Client wise outstanding summary for service tax purpose";

            model.FromMn = FromMn;
            model.FromYr = FromYr;
            model.ToMn = ToMn;
            model.ToYr = ToYr;
            model.BillSummary = rdo;

            List<BillRaisedModel> list = new List<BillRaisedModel>();
            if (FromMn > 0 && FromYr > 0)
            {
                list = billraisedRepository.GetReportList(model);
            }
            return View(list);
        }
    }
}
