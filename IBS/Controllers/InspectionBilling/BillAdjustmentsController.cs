using IBS.Interfaces.InspectionBilling;
using Microsoft.AspNetCore.Mvc;
using IBS.Models;

namespace IBS.Controllers.InspectionBilling
{
    public class BillAdjustmentsController : BaseController
    {
        #region Variables
        private readonly IBillAdjustmentsRepository billRepository;

        #endregion
        public BillAdjustmentsController(IBillAdjustmentsRepository _billRepository)
        {
            billRepository = _billRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetBillDetails(string BillNo)
        {
            try
            {
                InspectionCertModel model = new InspectionCertModel();
                if (BillNo != null)
                {
                    model = billRepository.FindByBillDetails(BillNo, Region);
                }
                return PartialView("_BillDetails", model);
                //return View(model);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "BillAdjustments", "GetBillDetails", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpPost]
        public IActionResult EditListDetails(string CaseNo, string CallRecvDt, int CallSno, int ItemSrnoPo)
        {
            try
            {
                InspectionCertModel model = new InspectionCertModel();
                DateTime CrecvDt = Convert.ToDateTime(CallRecvDt);
                if (CaseNo != null && CallRecvDt != null && CallSno > 0 && ItemSrnoPo > 0)
                {
                    model = billRepository.FindByItemID(CaseNo, CrecvDt, CallSno, ItemSrnoPo, Region);
                }
                return PartialView("_EditListDetails", model);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "InspectionCert", "EditListDetails", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
    }
}
