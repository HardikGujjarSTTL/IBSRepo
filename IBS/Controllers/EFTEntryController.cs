using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class EFTEntryController : BaseController
    {
        private readonly IEFTEntryRepository EFTEntryRepository;
        public EFTEntryController(IEFTEntryRepository _EFTEntryRepository)
        {
            EFTEntryRepository = _EFTEntryRepository;
        }

        [Authorization("EFTEntry", "EFTEntry", "view")]
        public IActionResult ListVoucher([FromBody] DTParameters dtParameters)
        {
            DTResult<EFTEntryModel> dTResult = EFTEntryRepository.GetVoucherList(dtParameters);
            return Json(dTResult);
        }

        [Authorization("EFTEntry", "EFTEntry", "view")]
        public IActionResult AddEFTVoucher(string VCHR_NO, int BANK_CD, string CHQ_NO, string CHQ_DT)
        {
            EFTEntryModel model = new();

            if (VCHR_NO != "" && VCHR_NO != null)
            {
                model = EFTEntryRepository.FindByID(VCHR_NO, BANK_CD, CHQ_NO, CHQ_DT);
            }
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorization("EFTEntry", "EFTEntry", "edit")]

        public IActionResult VoucherDetailsSave(EFTEntryModel model)
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
                string i = EFTEntryRepository.VoucherDetailsSave(model, GetUserInfo.Region.ToString());
                if (i != "" || i != null)
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

        [Authorization("EFTEntry", "EFTEntry", "view")]

        public IActionResult EFTEntry()
        {
            return View();
        }
    }
}
