using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class BillFinalisationFormController : BaseController
    {
        private readonly IBillFinalisationFormRepository billFinalisationFormRepository;

        public BillFinalisationFormController(IBillFinalisationFormRepository _billFinalisationFormRepository)
        {
            billFinalisationFormRepository = _billFinalisationFormRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            dtParameters.AdditionalValues.Add("Region", Region);
            DTResult<BillFinalisationFormModel> dTResult = billFinalisationFormRepository.GetBillFinalisationList(dtParameters);
            return Json(dTResult);
        }

        public IActionResult UpdateBillsFinalisation(string[] BillNos)
        {
            billFinalisationFormRepository.UpdateBillFinalisationStatus(BillNos);
            return Json(new { status = true, responseText = "Records Locking Successfully." });
        }
    }
}

