using IBS.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class DailyIEWiseCallsController : BaseController
    {
        private readonly IDailyIEWiseCallsRepository dailyIEWiseCallsRepository;
        public DailyIEWiseCallsController(IDailyIEWiseCallsRepository                                                                                                                                                                                                                                                                    _dailyIEWiseCallsRepository)
        {
            dailyIEWiseCallsRepository = _dailyIEWiseCallsRepository;
        }
        public IActionResult Index()
        {
            ViewBag.Region = Region;
            return View();
        }
    }
}
