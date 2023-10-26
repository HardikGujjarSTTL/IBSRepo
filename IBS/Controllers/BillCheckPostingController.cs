using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
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
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetBankDetails(string BankName, string ChqNo, DateTime ChqDt)
        {
            try
            {
                BillCheckPostingModel model = new BillCheckPostingModel();
                if (BankName != null)
                {
                    model = billRepository.FindByID(BankName, ChqNo, ChqDt, Region);
                }
                return View(model);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "BillCheckPosting", "GetBankDetails", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

    }
}
