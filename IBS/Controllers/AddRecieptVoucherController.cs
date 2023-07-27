using IBS.Interfaces;
using IBS.Models;
using IBS.Helper;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace IBS.Controllers
{
    public class AddRecieptVoucherController : BaseController
    {
        private readonly IAddRecieptVoucher addVoucherRepository;
        public AddRecieptVoucherController(IAddRecieptVoucher _addVoucherRepository)
        {
            addVoucherRepository = _addVoucherRepository;
        }

        public IActionResult Index()
        {
           
            return View();
        }
       
        public IActionResult VoucherList([FromBody] DTParameters dtParameters)
        {
            DTResult<AddRecieptVoucherModel> dTResult = addVoucherRepository.GetVoucherList( dtParameters);
            return Json(dTResult);
        }

        public IActionResult AddRecieptVoucher(string Vchr_No, string Case_No ,string Chq_no)
        {
            AddRecieptVoucherModel model = new();
            
            
                model = addVoucherRepository.FindByID(Vchr_No, Case_No, Chq_no);
            
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult VoucherDetailsSave(AddRecieptVoucherModel model)
        {
            try
            {
                string msg = "Voucher Inserted Successfully.";

                if (model.VCHR_NO != "")
                {
                    msg = "Voucher Updated Successfully.";
                    
                }
                //model.Createdby = UserId;
                //int i = contractRepository.ContractDetailsInsertUpdate(model);
                int i = addVoucherRepository.VoucherDetailsSave(model,GetUserInfo.Region.ToString());
                if (i > 0)
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

    }
}
