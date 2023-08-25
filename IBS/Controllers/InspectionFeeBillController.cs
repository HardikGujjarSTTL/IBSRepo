using IBS.Interfaces;
using IBS.Models;
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

        public IActionResult GetBillDetails(string BillNo)
        {
            InspectionFeeBillModel model = inspectionFeeBillRepository.FindByBillNo(BillNo);

            if (model == null)
            {
                AlertDanger("Record not found for the given Bill No.!!!");
                return View("Index");
            }
            return View("BillDetails", model);
        }

    }
}
