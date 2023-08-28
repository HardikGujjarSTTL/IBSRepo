using IBS.DataAccess;
using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Text.Json;

namespace IBS.Controllers
{
    public class LabPaymentsFormController : BaseController
    {
        #region Variables
        private readonly ILabPaymentFormRepository LabPaymentRepository;
        #endregion
        public LabPaymentsFormController(ILabPaymentFormRepository _LabPaymentRepository)
        {
            LabPaymentRepository = _LabPaymentRepository;
        }

        #region Lab Payments Form
        [Authorization("LabPaymentsForm", "LabPaymentForm", "view")]
        public IActionResult LabPaymentForm()
        {

            return View();
        }
        [Authorization("LabPaymentsForm", "LabPaymentForm", "view")]
        public IActionResult LabPayment()
        {

            return View();
        }
        [HttpPost]
        public IActionResult LoadTable(LabPaymentFormModel paymentFormModel, string PaymentID, string PaymentDT, string Lab)
        {
            paymentFormModel.PaymentID = PaymentID;
            paymentFormModel.PaymentDt = PaymentDT;
            paymentFormModel.Lab = Lab;
            paymentFormModel.Regin = GetRegionCode;
            List<LabPaymentFormModel> dTResult = LabPaymentRepository.GetLabPayments(paymentFormModel);
            return Json(dTResult);
        }
        [HttpPost]
        public IActionResult GetPayment(LabPaymentFormModel paymentFormModel, string PaymentID, string Lab)
        {
            paymentFormModel.PaymentID = PaymentID;
            paymentFormModel.Lab = Lab;
            paymentFormModel.Regin = GetRegionCode;
            List<LabPaymentFormModel> dTResult = LabPaymentRepository.GetPayments(paymentFormModel);
            return Json(dTResult);
        }
        [HttpPost]
        [Authorization("LabPaymentsForm", "LabPaymentForm", "add")]
        public JsonResult SavePayment([FromBody] LabPaymentFormModel paymentFormModel)
        {
            paymentFormModel.Regin = GetRegionCode;
            paymentFormModel.UserId = Convert.ToString(UserId);
            bool result;
            result = LabPaymentRepository.SavePayment(paymentFormModel);
            if (result == true)
            {

                //ViewBag.PaymentID = paymentFormModel.PaymentID;
                return Json(new { success = true, message = paymentFormModel.PaymentID });
            }
            else
            {
                return Json(new { success = false, message = paymentFormModel.PaymentID });
            }

        }

        public IActionResult PrintLabPayment(LabPaymentFormModel paymentFormModel, string VOUCHER_NO, string Lab)
        {
            //paymentFormModel.PaymentID = VOUCHER_NO;
            //paymentFormModel.Lab = Lab;
            paymentFormModel.Regin = GetRegionCode;
            if (paymentFormModel.Regin == "N")

                paymentFormModel.Regin = "Northern Region";
            else if (paymentFormModel.Regin == "S")
                paymentFormModel.Regin = "Southern Region";
            else if (paymentFormModel.Regin == "E")
                paymentFormModel.Regin = "Eastern Region";
            else if (paymentFormModel.Regin == "W")
                paymentFormModel.Regin = "Western Region";
            else if (paymentFormModel.Regin == "C")
                paymentFormModel.Regin = "Central Region";
            //List<LabPaymentFormModel> dTResult = LabPaymentRepository.PrintLabPayment(paymentFormModel);
            return View(paymentFormModel);
        }
        [HttpPost]
        public IActionResult PrintLapPayments(LabPaymentFormModel paymentFormModel, string VOUCHER_NO, string Lab)
        {
            paymentFormModel.PaymentID = VOUCHER_NO;
            paymentFormModel.Lab = Lab;
            paymentFormModel.Regin = GetRegionCode;
            LabPaymentFormModel dTResult = LabPaymentRepository.PrintLabPayment(paymentFormModel);
            return Json(dTResult);
        }
        #endregion


    }
}
