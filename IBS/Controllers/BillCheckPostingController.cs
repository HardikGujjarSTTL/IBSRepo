using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using IBS.Repositories.Vendor;
using iTextSharp.text;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class BillCheckPostingController : BaseController
    {
        private readonly IBillCheckPostingRepository billRepository;

        public BillCheckPostingController(IBillCheckPostingRepository _billRepository)
        {
            billRepository = _billRepository;
        }

        //[Authorization("BillCheckPosting", "Index", "view")]
        public IActionResult Index(string ChqNo, string ChqDt, string BankName, string BillNo, int AmountCleared, string ActionType)
        {
            BillCheckPostingModel model = new BillCheckPostingModel();
            if (ChqNo != null && ChqDt != null && BankName != null && BillNo != null && AmountCleared > 0)
            {
                model = billRepository.FindByID(ChqNo, Convert.ToDateTime(ChqDt), BankName, BillNo, AmountCleared, Region);
            }
            return View(model);
        }

        public IActionResult GetBankDetails(int BankCd, string ChqNo, DateTime ChqDt)
        {
            BillCheckPostingModel dTResult = billRepository.GetBankDetails(BankCd, ChqNo, ChqDt, Region);
            return Json(dTResult);
        }

        public IActionResult GetBillDetails(string BillInvoiceNo, string BillTypes)
        {
            BillCheckPostingModel dTResult = billRepository.GetBillDetails(BillInvoiceNo, BillTypes, Region);
            return Json(dTResult);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<BillCheckPostingModelList> dTResult = billRepository.GetBillList(dtParameters);
            return Json(dTResult);
        }

        [HttpPost]
        public IActionResult BillDetailSave(BillCheckPostingModel model)
        {
            try
            {
                string msg = "Inserted Successfully.";
                int i = billRepository.BillDetailSave(model, UserName.Trim());
                if (i == 1)
                {
                    return Json(new { success = true, responseText = msg, Status = i });
                }
                else if (i == 2)
                {
                    msg = "Suspense Amount of Cheque is Zero or Amount To be Cleared is greater then the(Check Amount - (Amount Adjusted + Amount Transferred)) OR Amount To be Cleared is Greater then the Bill Amount To Recover!!!";
                    return Json(new { success = false, responseText = msg, Status = i });
                }
                else if (i == 3)
                {
                    msg = "Amount To be Cleared is greater then the (Check Amount - (Amount Adjusted + Amount Transferred)) OR Amount To be Cleared is Greater then the Bill Amount To Recover!!!";
                    return Json(new { success = false, responseText = msg, Status = i });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "BillCheckPosting", "BillDetailSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }


    }
}
