using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Xml.Linq;

namespace IBS.Controllers
{
    public class NCRRegisterController : Controller
    {
        private readonly INCRRegisterRepository nCRRegisterRepository;
        public NCRRegisterController(INCRRegisterRepository _nCRRegisterRepository)
        {
            nCRRegisterRepository = _nCRRegisterRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
           DTResult<NCRRegister> dTResult = nCRRegisterRepository.GetDataList(dtParameters);
           return Json(dTResult);
        }

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
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }

            return View(model.Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveRemarksofNCR(NCRRegister model)
        {
            bool isRadioChecked = bool.Parse(Request.Form["IsRadioChecked"]);
            string extractedText = Request.Form["extractedText"];
            string msg = nCRRegisterRepository.Saveupdate(model, isRadioChecked, extractedText);

            return Json(new { status = true, responseText = msg });
        }

        [HttpPost]
        public IActionResult SaveRemarks([FromBody] List<Remarks> editableColumnDataList)
        {
            foreach (var value in editableColumnDataList)
            {
                var editableColumn1Value = value.EditableColumn1;
                var editableColumn2Value = value.EditableColumn2;

            }
            return Json(new { success = true });
        }

    }
}
