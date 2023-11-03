using IBS.DataAccess;
using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using System.Drawing;
using System.Text.Json;

namespace IBS.Controllers
{
    public class LabPaymentsFormController : BaseController
    {
        #region Variables
        private readonly ILabPaymentFormRepository LabPaymentRepository;
        private readonly IWebHostEnvironment env;
        #endregion
        public LabPaymentsFormController(ILabPaymentFormRepository _LabPaymentRepository, IWebHostEnvironment _env)
        {
            LabPaymentRepository = _LabPaymentRepository;
            this.env = _env;
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
        [Authorization("LabPaymentsForm", "LabPaymentForm", "view")]
        public IActionResult LabPaymentEdit(string PaymentID)
        {
            ViewBag.PaymentID = PaymentID;

            return View();
        }
        [HttpPost]
        public IActionResult LoadTableEdit([FromBody] DTParameters dtParameters)
        {
            string Regin = GetRegionCode;
            DTResult<LabPaymentFormModel> dTResult = new DTResult<LabPaymentFormModel>();
            try
            {
                dTResult = LabPaymentRepository.GetPaymentsEdit(dtParameters);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "LabPaymentsForm", "LoadTableEdit", 1, GetIPAddress());
            }
            return Json(dTResult);
        }
        [HttpPost]
        public IActionResult Edit(string PaymentID)
        {
            string Regin = GetRegionCode;
            LabPaymentFormModel dTResult = new LabPaymentFormModel();
            try
            {
                dTResult = LabPaymentRepository.Edit(PaymentID);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "LabPaymentsForm", "Edit", 1, GetIPAddress());
            }
            return Json(dTResult);
        }
        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            string Regin = GetRegionCode;
            DTResult<LabPaymentFormModel> dTResult = new DTResult<LabPaymentFormModel>();
            try
            {
                dTResult = LabPaymentRepository.GetLabPayments(dtParameters, Regin);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "LabPaymentsForm", "LoadTable", 1, GetIPAddress());
            }
            return Json(dTResult);
        }

        //public IActionResult GetPayment(LabPaymentFormModel paymentFormModel, string PaymentID, string Lab)
        //{
        //    paymentFormModel.PaymentID = PaymentID;
        //    paymentFormModel.Lab = Lab;
        //    paymentFormModel.Regin = GetRegionCode;
        //    List<LabPaymentFormModel> dTResult = LabPaymentRepository.GetPayments(paymentFormModel);
        //    return Json(dTResult);
        //}
        [HttpPost]
        public IActionResult GetPayment([FromBody] DTParameters dtParameters)
        {

            string Regin = GetRegionCode;
            DTResult<LabPaymentFormModel> dTResult = new DTResult<LabPaymentFormModel>();
            try
            {
                dTResult = LabPaymentRepository.GetPayments(dtParameters, Regin);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "LabPaymentsForm", "GetPayment", 1, GetIPAddress());
            }
            return Json(dTResult);
        }
        [HttpPost]
        [Authorization("LabPaymentsForm", "LabPaymentForm", "edit")]
        public JsonResult SavePayment([FromBody] LabPaymentFormModel paymentFormModel)
        {
            try
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
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "LabPaymentsForm", "SavePayment", 1, GetIPAddress());
            }
            return Json(false);
        }

        [HttpPost]
        [Authorization("LabPaymentsForm", "LabPaymentForm", "edit")]
        public JsonResult UpdatePayment([FromBody] LabPaymentFormModel paymentFormModel)
        {
            try
            {
                paymentFormModel.Regin = GetRegionCode;
                paymentFormModel.UserId = Convert.ToString(UserId);
                bool result;
                result = LabPaymentRepository.UpdatePayment(paymentFormModel);
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
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "LabPaymentsForm", "UpdatePayment", 1, GetIPAddress());
            }
            return Json(false);
        }

        public IActionResult PrintLabPayment(LabPaymentFormModel paymentFormModel, string VOUCHER_NO, string Lab)
        {
            ViewBag.PaymentID = VOUCHER_NO;
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
            LabPaymentFormModel dTResult = new LabPaymentFormModel();
            try
            {
                paymentFormModel.PaymentID = VOUCHER_NO;
                paymentFormModel.Lab = Lab;
                paymentFormModel.Regin = GetRegionCode;
                dTResult = LabPaymentRepository.PrintLabPayment(paymentFormModel);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "LabPaymentsForm", "PrintLapPayments", 1, GetIPAddress());
            }
            return Json(dTResult);
        }
        [HttpPost]
        public async Task<IActionResult> GeneratePDF(string htmlContent)
        {
            //PendingICAgainstCallsModel _model = JsonConvert.DeserializeObject<PendingICAgainstCallsModel>(TempData[model.ReportType].ToString());
            //htmlContent = await this.RenderViewToStringAsync("/Views/ManagementReports/PendingICAgainstCalls.cshtml", _model);

            await new BrowserFetcher().DownloadAsync();
            await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
                DefaultViewport = null
            });
            await using var page = await browser.NewPageAsync();
            await page.EmulateMediaTypeAsync(MediaType.Screen);
            await page.SetContentAsync(htmlContent);

            string cssPath = env.WebRootPath + "/css/report.css";

            AddTagOptions bootstrapCSS = new AddTagOptions() { Path = cssPath };
            await page.AddStyleTagAsync(bootstrapCSS);

            var pdfContent = await page.PdfStreamAsync(new PdfOptions
            {
                Landscape = true,
                Format = PaperFormat.Letter,
                PrintBackground = true
            });

            await browser.CloseAsync();

            return File(pdfContent, "application/pdf", Guid.NewGuid().ToString() + ".pdf");
        }

        [HttpPost]
        public IActionResult PrintLoadTable([FromBody] DTParameters dtParameters)
        {

            string Regin = GetRegionCode;
            DTResult<LabPaymentFormModel> dTResult = new DTResult<LabPaymentFormModel>();
            try
            {
                dTResult = LabPaymentRepository.PrintLoadTable(dtParameters, Regin);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "LabPaymentsForm", "PrintLoadTable", 1, GetIPAddress());
            }
            return Json(dTResult);
        }
        #endregion


    }
}
