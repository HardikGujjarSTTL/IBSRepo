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
        public IActionResult SaveRemarksofNCR(NCRRegister models,string serializedModel)
        {
            NCRRegister modelss = JsonConvert.DeserializeObject<NCRRegister>(serializedModel);
            string msg = "Remarks Inserted Successfully.";
            //foreach (var model in models)
            //{
            //    int i = nCRRegisterRepository.SaveRemarks(model);
            //}
            int i = nCRRegisterRepository.Saveupdate(models);
            return Json(new { status = true, responseText = msg });
        }

    }
}
