using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class InspectionFeeBillController : BaseController
    {
        private readonly IInspectionFeeBillRepository inspectionFeeBillRepository;

        public InspectionFeeBillController(IInspectionFeeBillRepository _inspectionFeeBillRepository)
        {
            this.inspectionFeeBillRepository = _inspectionFeeBillRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult BillDetails(string BillNo)
        {
            InspectionFeeBillModel model = inspectionFeeBillRepository.FindByBillNo(BillNo);

            if (model == null)
            {
                AlertDanger("Record not found for the given Bill No.!!!");
                return View("Index");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTableBillItems([FromBody] DTParameters dtParameters)
        {
            DTResult<BillItemsListModel> dTResult = inspectionFeeBillRepository.GetBillItemsList(dtParameters);
            return Json(dTResult);
        }

        [HttpPost]
        public IActionResult LoadTableChequeDetails([FromBody] DTParameters dtParameters)
        {
            DTResult<ChequeDetailsListModel> dTResult = inspectionFeeBillRepository.GetChequeDetailsList(dtParameters);
            return Json(dTResult);
        }
    }
}
