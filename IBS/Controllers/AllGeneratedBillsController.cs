using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class AllGeneratedBillsController : BaseController
    {
        private readonly IAllGeneratedBillsRepository allGeneratedBillsRepository;

        public AllGeneratedBillsController(IAllGeneratedBillsRepository _allGeneratedBillsRepository)
        {
            allGeneratedBillsRepository = _allGeneratedBillsRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<AllGeneratedBills> dTResult = allGeneratedBillsRepository.GetBillDetails(dtParameters);
            return Json(dTResult);
        }
    }
}
