using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Policy;

namespace IBS.Controllers
{
    [Authorization]
    public class DEOCRISPurchesOrderMAWCaseNoController : BaseController
    {
        #region Variables
        private readonly IDEOCRISPurchesOrderWCaseNoRepository purchesorderRepository;
        #endregion
        public DEOCRISPurchesOrderMAWCaseNoController(IDEOCRISPurchesOrderWCaseNoRepository _purchesorderRepository)
        {
            purchesorderRepository = _purchesorderRepository;
        }
        [Authorization("DEOCRISPurchesOrderMAWCaseNo", "Index", "view")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<DEOCRISPurchesOrderMAModel> dTResult = purchesorderRepository.GetDataList(dtParameters, Region);
            return Json(dTResult);
        }

        [Authorization("DEOCRISPurchesOrderMAWCaseNo", "Index", "view")]
        public IActionResult Manage(string Rly, int Makey, byte Slno)
        {
            DEOCRISPurchesOrderMAModel model = new();

            if (Rly != null)
            {
                model = purchesorderRepository.FindByID(Rly, Makey, Slno);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorization("DEOCRISPurchesOrderMAWCaseNo", "Index", "edit")]
        public IActionResult DetailsSave(DEOCRISPurchesOrderMAModel model)
        {
            try
            {
                string msg = "Inserted Successfully.";

                if (model.Rly != null && model.Makey != null && model.Slno != null)
                {
                    msg = "Updated Successfully.";
                    model.ApprovedBy = Convert.ToString(UserId);
                }

                int i = purchesorderRepository.DetailsUpdate(model);
                if (i > 0)
                {
                    return Json(new { status = true, responseText = msg, Id = i });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "CRIS PURCHASE ORDERS REGISTERED", "DetailsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
    }
}
