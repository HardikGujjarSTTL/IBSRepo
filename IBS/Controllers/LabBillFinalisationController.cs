using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class LabBillFinalisationController : BaseController
    {
        #region Variables
        private readonly ILabBillFinalisationRepository LabBillFinalisationRepository;
        #endregion
        public LabBillFinalisationController(ILabBillFinalisationRepository _LabBillFinalisationRepository)
        {
            LabBillFinalisationRepository = _LabBillFinalisationRepository;
        }
        [Authorization("LabBillFinalisation", "LabBillFinalisationForm", "view")]
        #region 
        public IActionResult LabBillFinalisationForm()
        {

            return View();
        }
        [HttpPost]
        public IActionResult LoadTable(string FromDate, string ToDate)
        {
            List<LabBillFinalisationModel> dTResult = new List<LabBillFinalisationModel>();
            try
            {
                string Regin = GetRegionCode;
                dTResult = LabBillFinalisationRepository.GetBill(FromDate, ToDate, Regin);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "LabBillFinalisation", "LoadTable", 1, GetIPAddress());
            }
            return Json(dTResult);
        }
        [HttpPost]
        [Authorization("LabBillFinalisation", "LabBillFinalisationForm", "edit")]
        public JsonResult UpdateBill([FromBody] LabBillFinalisationModel LabBillFinalisationModel)
        {

            try
            {
                bool result;
                result = LabBillFinalisationRepository.UpdateBill(LabBillFinalisationModel);
                if (result == true)
                {

                    //ViewBag.PaymentID = paymentFormModel.PaymentID;
                    return Json(true);
                }
                else
                {
                    return Json(false);
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "LabBillFinalisation", "UpdateBill", 1, GetIPAddress());
            }
            return Json(false);

        }
        #endregion


    }
}
