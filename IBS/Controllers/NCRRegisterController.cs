using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

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

        public IActionResult Manage(string CaseNo, string BKNo, string SetNo, string NCNO, string Actions)
        {
            if(Actions == "A")
            {
                ViewBag.ShowSaveButton = true;
                ViewBag.ShowNCRNO = false;
                ViewBag.ShowRemarksButton = false;
                ViewBag.ShowNCRButton = true;
            }else if(Actions == "M")
            {
                ViewBag.ShowNCRButton = false;
                ViewBag.ShowNCRNO = true;
                ViewBag.ShowRemarksButton = true;
                ViewBag.ShowSaveButton = false;
            }
            NCRRegister model = new();

            try
            {
                if (CaseNo != null && BKNo != null && SetNo != null || NCNO != null)
                {
                    model = nCRRegisterRepository.FindByIDActionA(CaseNo, BKNo, SetNo,NCNO);
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            return View(model);
        }

    }
}
