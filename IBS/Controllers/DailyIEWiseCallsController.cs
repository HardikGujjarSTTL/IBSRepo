using IBS.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class DailyIEWiseCallsController : BaseController
    {
        private readonly IDailyIEWiseCallsRepository dailyIEWiseCallsRepository;
        private readonly IConfiguration config;
        public DailyIEWiseCallsController(IDailyIEWiseCallsRepository _dailyIEWiseCallsRepository, IConfiguration _config)
        {
            dailyIEWiseCallsRepository = _dailyIEWiseCallsRepository;
            config = _config;
        }
        public IActionResult Index()
        {
            ViewBag.ReportUrl = config.GetSection("AppSettings")["ReportUrl"];
            ViewBag.Region = Region;
            return View();
        }
    }
}
