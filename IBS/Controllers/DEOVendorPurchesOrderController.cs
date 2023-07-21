using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class DEOVendorPurchesOrderController : BaseController
    {
        #region Variables
        private readonly IDEOVendorPurchesOrderRepository deovendorpurchesRepository;
        #endregion

        public DEOVendorPurchesOrderController(IDEOVendorPurchesOrderRepository _deovendorpurchesRepository)
        {
            deovendorpurchesRepository = _deovendorpurchesRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<DEOVendorPurchesOrderModel> dTResult = deovendorpurchesRepository.GetDataList(dtParameters, GetRegionCode);
            return Json(dTResult);
        }

        public IActionResult Manage()
        {
            return View();
        }
    }
}
