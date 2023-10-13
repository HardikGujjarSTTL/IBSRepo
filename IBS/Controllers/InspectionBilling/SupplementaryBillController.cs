using IBS.Interfaces.InspectionBilling;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers.InspectionBilling
{
    public class SupplementaryBillController : BaseController
    {
        #region Variables
        private readonly ISupplementaryBillRepository billRepository;

        #endregion
        public SupplementaryBillController(ISupplementaryBillRepository _billRepository)
        {
            billRepository = _billRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<BillDetailsModel> dTResult = billRepository.GetLoadTable(dtParameters, Region);
            return Json(dTResult);
        }
    }
}
