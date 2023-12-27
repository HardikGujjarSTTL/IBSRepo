using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class LabSearchPaymentController : BaseController
    {
        #region Variables
        private readonly ILabSearchPaymentRepository LabSearchPaymentRepository;
        #endregion
        public LabSearchPaymentController(ILabSearchPaymentRepository _LabSearchPaymentRepository)
        {
            LabSearchPaymentRepository = _LabSearchPaymentRepository;
        }
        //[Authorization("Search", "Index", "view")]
        public IActionResult Index()
        {
            var region = GetUserInfo.Region;
            ViewBag.Region = region;
            return View();
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            var region = GetUserInfo.Region;
            DTResult<SearchPaymentsModel> dTResult = new DTResult<SearchPaymentsModel>();
            try
            {
                dTResult = LabSearchPaymentRepository.GetSearchList(dtParameters);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "LabSearchPayment", "LoadTable", 1, GetIPAddress());
            }
            return Json(dTResult);
        }

    }
}
