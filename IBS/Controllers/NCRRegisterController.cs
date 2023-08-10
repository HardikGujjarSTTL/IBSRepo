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

        public IActionResult Manage(string CASE_NO, string BK_NO, string SET_NO)
        {
            NCRRegister model = new();

            try
            {
            //    if (CASE_NO != null && BK_NO != null && SET_NO != null)
            //    {
            //        model = consigneeComplaints.FindByID(CASE_NO, BK_NO, SET_NO);
            //    }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            return View(model);
        }

    }
}
