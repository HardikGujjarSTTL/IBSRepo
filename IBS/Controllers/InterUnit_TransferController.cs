using IBS.Filters;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    [Authorization]
    public class InterUnit_TransferController : BaseController
    {
        private readonly IInterUnit_TransferRepository interunittransferrepository;
        SessionHelper objSessionHelper = new SessionHelper();
        public InterUnit_TransferController(IInterUnit_TransferRepository _interunittransferrepository)
        {
            interunittransferrepository = _interunittransferrepository;
        }

        [Authorization("InterUnit_Transfer", "Index", "view")]

        public IActionResult Index()
        {
            return View();
        }
       
        [Authorization("InterUnit_Transfer", "Index", "view")]
        public IActionResult Manage(string Bank, string ChqNo, string ChqDT)
        {
            string Bank_CD = "", Chq_No = "", ChqDate = "";
            //if (formCollection.Keys.Contains("hdnBank")) { Bank = formCollection["hdnBank"]; }
            //if (formCollection.Keys.Contains("hdnChequeNo")) { ChqNo = formCollection["hdnChequeNo"]; }
            //if (formCollection.Keys.Contains("hdnChequeDate")) { ChqDate = formCollection["hdnChequeDate"]; }            
            InterUnit_TransferModel data = interunittransferrepository.Get_Inter_Unit_Transfer(Bank, ChqNo, ChqDT, Region);
            if (Region == "N") { data.Region_ID = "3007"; }
            else if (Region == "E") { data.Region_ID = "3008"; }
            else if (Region == "S") { data.Region_ID = "3009"; }
            else if (Region == "W") { data.Region_ID = "3006"; }
            else if (Region == "C") { data.Region_ID = "3066"; }
            return View(data);
        }

        [HttpPost]
        public IActionResult ManageData(IFormCollection formCollection)
        {
            string Bank = "", ChqNo = "", ChqDate = "";
            if (formCollection.Keys.Contains("hdnBank")) { Bank = formCollection["hdnBank"]; }
            if (formCollection.Keys.Contains("hdnChequeNo")) { ChqNo = formCollection["hdnChequeNo"]; }
            if (formCollection.Keys.Contains("hdnChequeDate")) { ChqDate = formCollection["hdnChequeDate"]; }
            var data = interunittransferrepository.Get_Inter_Unit_Transfer(Bank, ChqNo, ChqDate, Region);
            return View("Manage", data);
        }


        [HttpPost]
        public IActionResult LoadUnitTransferTable([FromBody] DTParameters dtParameters)
        {
            List<InterUnitTransferRegionModel> lstInterUnitTransferRegionModel = new List<InterUnitTransferRegionModel>();
            if (objSessionHelper.lstInterUnitTransferRegionModel != null)
            {
                lstInterUnitTransferRegionModel = objSessionHelper.lstInterUnitTransferRegionModel;
            }

            DTResult<InterUnitTransferRegionModel> dTResult = interunittransferrepository.GetInterUnitTransferRegion(dtParameters, lstInterUnitTransferRegionModel);
            return Json(dTResult);
        }

        [HttpGet]
        public IActionResult EditInterUnitTransfer(string id)
        {
            try
            {
                InterUnitTransferRegionModel InUniTrans = objSessionHelper.lstInterUnitTransferRegionModel.Where(x => x.ID == Convert.ToInt32(id)).FirstOrDefault();
                return Json(new { status = true, list = InUniTrans });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "InterUnit_Transfer", "EditInterUnitTransfer", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }        
    }
}
