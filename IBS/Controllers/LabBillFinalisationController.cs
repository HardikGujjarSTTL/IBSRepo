using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Text.Json;

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

        #region 
        public IActionResult LabBillFinalisationForm()
        {

            return View();
        }
        [HttpPost]
        public IActionResult LoadTable(string FromDate, string ToDate)
        {
            
            string Regin = GetRegionCode;
            List<LabBillFinalisationModel> dTResult = LabBillFinalisationRepository.GetBill(FromDate,ToDate,Regin);
            return Json(dTResult);
        }
        [HttpPost]
        public JsonResult UpdateBill([FromBody] LabBillFinalisationModel LabBillFinalisationModel)
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
        #endregion


    }
}
