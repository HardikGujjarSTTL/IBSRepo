using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
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
        public IActionResult AddEFTVoucher(EFTEntryModel model)
        {
            try
            {
                if (model.VCHR_NO != "")
                {
                    string i = EFTEntryRepository.VoucherDetailsSave(model, GetUserInfo.Region.ToString());
                    AlertAddSuccess("Record Updated Successfully.");
                    return RedirectToAction("EFTEntry");
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "EFTEntry", "AddEFTVoucher", 1, GetIPAddress());
            }
            return View(model);
        }

        [Authorization("EFTEntry", "EFTEntry", "view")]

        public IActionResult EFTEntry()
        {
            return View();
        }
    }
}
