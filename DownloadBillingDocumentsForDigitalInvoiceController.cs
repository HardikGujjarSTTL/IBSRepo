using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Drawing;

namespace IBS.Controllers.IE
{
    public class DownloadBillingDocumentsForDigitalInvoiceController : BaseController
    {
        private readonly IDownload_billing downloadBillingDocumentsforDigitalInvoiceRepository;
        public DownloadBillingDocumentsForDigitalInvoiceController(IDownload_billing _downloadBillingDocumentsforDigitalInvoiceRepository)
        {
            downloadBillingDocumentsforDigitalInvoiceRepository = _downloadBillingDocumentsforDigitalInvoiceRepository;
        }
        public IActionResult DownloadBillingDocumentsForDigitalInvoiceMaster()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<DownloadBillingDocumentsForDigitalInvoiceModel> dTResult = downloadBillingDocumentsforDigitalInvoiceRepository.GetDownloadBillingList(dtParameters);
            return Json(dTResult);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DetailsInsertUpdate(DownloadBillingDocumentsForDigitalInvoiceModel model)
        {
            try
            {
                string msg = "Inserted Successfully.";

                if (model.FortheMonth != null)
                {
                    msg = "Updated Successfully.";
                    model.ForGivenPeriod = Convert.ToString(UserId);
                }

                int i = downloadBillingDocumentsforDigitalInvoiceRepository.DownloadBillingInsertUpdate(model);
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

        public IActionResult Allow_Old_Bill_DateManage(string ForGivenPeriod)
        {
            DownloadBillingDocumentsForDigitalInvoiceModel model = new();

            if (ForGivenPeriod != null)
            {
                model = downloadBillingDocumentsforDigitalInvoiceRepository.FindByID(Convert.ToInt32(ForGivenPeriod));
            }

            return View(model);
        }
    }
}



  


