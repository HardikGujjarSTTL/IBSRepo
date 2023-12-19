using IBS.Interfaces.WebsitePages;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers.WebsitePages
{
    public class OnlinePaymentGatewayController : BaseController
    {
        #region Variables
        private readonly IOnlinePaymentGatewayRepository onlinePaymentGatewayRepository;
        #endregion
        public OnlinePaymentGatewayController(IOnlinePaymentGatewayRepository _onlinePaymentGatewayRepository)
        {
            onlinePaymentGatewayRepository = _onlinePaymentGatewayRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult VerifyPayment(OnlinePaymentGateway model)
        {
            model = onlinePaymentGatewayRepository.VerifyByCaseNo(model);

            if (model.AlertMsg != null)
            {
                AlertAlreadyExist(model.AlertMsg);
                return View("Index", model);
            }

            return Json(model);
        }

    }
}
