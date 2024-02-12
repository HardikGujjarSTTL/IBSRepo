using IBS.Filters;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IBS.Controllers
{
    public class NCRRegisterController : BaseController
    {
        private readonly INCRRegisterRepository nCRRegisterRepository;
        public NCRRegisterController(INCRRegisterRepository _nCRRegisterRepository)
        {
            nCRRegisterRepository = _nCRRegisterRepository;
        }
        [Authorization("NCRRegister", "Index", "view")]
        public IActionResult Index()
        {
            string Region = SessionHelper.UserModelDTO.Region;
            int? IE = SessionHelper.UserModelDTO.IeCd == 0 ? (int?)null : SessionHelper.UserModelDTO.IeCd;
            int? CoCd = SessionHelper.UserModelDTO.CoCd == 0 ? (int?)null : SessionHelper.UserModelDTO.CoCd;
            ViewBag.IeCd = IE;
            ViewBag.Co_Cd = CoCd;
            ViewBag.Regions = Region;
            return View();
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<NCRRegister> dTResult = nCRRegisterRepository.GetDataList(dtParameters,Region);
            foreach (var item in dTResult.data)
            {
                item.IeCd = SessionHelper.UserModelDTO.IeCd == 0 ? null : SessionHelper.UserModelDTO.IeCd;
            }
            return Json(dTResult);
        }

        [Authorization("NCRRegister", "Index", "view")]
        public IActionResult Manage(string CaseNo, string BKNo, string SetNo, string NC_NO, string Actions)
        {
            NCRRegister model = new();
            try
            {
                if (Actions == "A")
                {
                    ViewBag.ShowSaveButton = true;
                    ViewBag.ShowNCRNO = false;
                    ViewBag.ShowRemarksButton = false;
                    ViewBag.ShowNCRButton = true;
                    ViewBag.Showradio = true;
                }
                else if (Actions == "M")
                {
                    ViewBag.ShowNCRButton = true;
                    ViewBag.Showradio = false;
                    ViewBag.ShowNCRNO = true;
                    ViewBag.ShowRemarksButton = true;
                    ViewBag.ShowSaveButton = false;
                }

                if (Actions == "M" && CaseNo != null && CaseNo != "")
                {
                    ViewBag.ShowNCRButton = true;
                    ViewBag.Showradio = false;
                }

                model = nCRRegisterRepository.FindByIDActionA(CaseNo, BKNo, SetNo, NC_NO, Actions);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "NCRRegister", "Manage", 1, GetIPAddress());
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTableRemarks([FromBody] DTParameters dtParameters)
        {
            DTResult<Remarks> dTResult = nCRRegisterRepository.GetRemarks(dtParameters);
            return Json(dTResult);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorization("NCRRegister", "Index", "edit")]
        public IActionResult SaveUpdateNCR(NCRRegister model)
        {
            try
            {
                bool isRadioChecked = bool.Parse(Request.Form["IsRadioChecked"]);
                string extractedText = Request.Form["extractedText"];
                model = nCRRegisterRepository.Saveupdate(model, isRadioChecked, extractedText);
                AlertUpdateSuccess("Record Save Successfully!!");
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "NCRRegister", "SaveUpdateNCR", 1, GetIPAddress());
            }
            return Json(new { status = true, responseText = model, redirectToIndex = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorization("NCRRegister", "Index", "edit")]
        public IActionResult SaveMoreNC(NCRRegister model)
        {
            string msg = "";
            try
            {
                string extractedText = Request.Form["extractedText"];
                model = nCRRegisterRepository.SaveMoreNC(model, extractedText);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "NCRRegister", "SaveMoreNC", 1, GetIPAddress());
            }
            return Json(new { status = true, responseText = model.NC_NO, redirectToIndex = true, alertMessage = msg });
        }

        [HttpPost]
        [Authorization("NCRRegister", "Index", "edit")]
        public IActionResult SaveRemarks(string NCNO, string UserID, [FromForm] List<Remarks> model)
        {
            string msg = "";
            try
            {
                msg = nCRRegisterRepository.SaveRemarks(NCNO, UserID, model);
                AlertAddSuccess("Remarks Save Succesfully!!!");
                return Json(new { status = true, redirectToIndex = true });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "NCRRegister", "SaveRemarks", 1, GetIPAddress());
            }
            return Json(new { status = false, redirectToIndex = false });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SendToIE(NCRRegister nCRRegister)
        {
            bool msg = nCRRegisterRepository.SendEmail(nCRRegister);
            return Json(new { status = true, responseText = msg, redirectToIndex = true, alertMessage = msg });
        }

        [HttpPost]
        public IActionResult GetNCRCode(string NCRClass)
        {
            List<SelectListItem> NCRCode = new();
            try
            {
                NCRCode = nCRRegisterRepository.GetNcrCd(NCRClass);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "NCRRegister", "GetNCRCode", 1, GetIPAddress());
            }
            return Json(NCRCode);
        }

        [HttpPost]
        public IActionResult GetItem(string CaseNo, string BKNo, string SetNo)
        {
            var json = "";
            try
            {
                json = nCRRegisterRepository.GetItems(CaseNo, BKNo, SetNo);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "NCRRegister", "GetItem", 1, GetIPAddress());
            }
            return Json(json);
        }

        [HttpPost]
        public IActionResult GetQtyByItem(string CaseNo, string CALLRECVDT, string CALLSNO, string ItemSno)
        {
            var json = "";
            try
            {
                json = nCRRegisterRepository.GetQtyByItems(CaseNo, CALLRECVDT, CALLSNO, ItemSno);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "NCRRegister", "GetQtyByItem", 1, GetIPAddress());
            }
            return Json(json);
        }

    }
}
