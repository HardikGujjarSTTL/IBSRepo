using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class TDSEntryController : BaseController
    {

        private readonly ITDSEntryRepository tdsentryrepository;
        public TDSEntryController(ITDSEntryRepository _tdsentryrepository)
        {
            tdsentryrepository = _tdsentryrepository;
        }

        [HttpPost]
        public IActionResult GetValue(string txtBNO)
        {
            txtBNO = Request.Form["BILL_NO"];
            string region = GetRegionCode;
            TDSEntryModel dTResult = tdsentryrepository.GetTextboxValues(txtBNO, region);
            return Json(dTResult);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorization("TDSEntry", "TDSEntry", "edit")]
        public IActionResult TDSEntry(TDSEntryModel model)
        {
            try
            {
                string msg = "Record Inserted Successfully.";

                if (model.BILL_NO != "")
                {
                    msg = "Record Updated Successfully.";

                }
                //model.Createdby = UserId;
                //int i = contractRepository.ContractDetailsInsertUpdate(model);
                string i = tdsentryrepository.TDSdetailSave(model);
                if (i != "" || i != "")
                {
                    return Json(new { status = true, responseText = msg });
                }
            }

            catch (Exception ex)
            {
                // Common.AddException(ex.ToString(), ex.Message.ToString(), "Contract", "ContractDetailsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
