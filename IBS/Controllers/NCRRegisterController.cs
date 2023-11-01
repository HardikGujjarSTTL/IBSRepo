using IBS.Filters;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Protocol.Core.Types;
using System.Xml.Linq;

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
            ViewBag.Regions = Region;
            return View();
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
           DTResult<NCRRegister> dTResult = nCRRegisterRepository.GetDataList(dtParameters);
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
                }
                else if (Actions == "M")
                {
                    ViewBag.ShowNCRButton = false;
                    ViewBag.ShowNCRNO = true;
                    ViewBag.ShowRemarksButton = true;
                    ViewBag.ShowSaveButton = false;
                }

                model = nCRRegisterRepository.FindByIDActionA(CaseNo, BKNo, SetNo, NC_NO);

                ViewBag.JsonData = model.JsonData;
                ViewBag.JsonData1 = model.msg;
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "NCRRegister", "Manage", 1, GetIPAddress());
            }

            return View(model.Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorization("NCRRegister", "Index", "edit")]
        public IActionResult SaveUpdateNCR(NCRRegister model)
        {
            string msg = "";
            try
            {
                bool isRadioChecked = bool.Parse(Request.Form["IsRadioChecked"]);
                string extractedText = Request.Form["extractedText"];
                msg = nCRRegisterRepository.Saveupdate(model, isRadioChecked, extractedText);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "NCRRegister", "SaveUpdateNCR", 1, GetIPAddress());
            }
            return Json(new { status = true, responseText = msg, redirectToIndex = true, alertMessage = msg });
        }

        [HttpPost]
        [Authorization("NCRRegister", "Index", "edit")]
        public IActionResult SaveRemarks(string NCNO,string UserID, [FromForm] List<Remarks> model)
        {
            string msg = "";
            try
            {
                msg = nCRRegisterRepository.SaveRemarks(NCNO, UserID, model);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "NCRRegister", "SaveRemarks", 1, GetIPAddress());
            }
            return Json(new { status = true, responseText = msg, redirectToIndex = true, alertMessage = msg });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SendToIE(NCRRegister nCRRegister)
        {
            bool msg = nCRRegisterRepository.SendEmail(nCRRegister);
            return Json(new { status = true, responseText = msg, redirectToIndex = true, alertMessage = msg });
        }

    }
}
