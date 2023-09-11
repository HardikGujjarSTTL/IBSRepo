using IBS.Filters;
using IBS.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class ConsigneeCompPeriodController : BaseController
    {
        private readonly IConsigneeCompPeriodRepository consigneeCompPeriodRepository;
        public ConsigneeCompPeriodController(IConsigneeCompPeriodRepository _consigneeCompPeriodRepository)
        {
            consigneeCompPeriodRepository = _consigneeCompPeriodRepository;
        }
        [Authorization("ConsigneeCompPeriod", "Index", "view")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
