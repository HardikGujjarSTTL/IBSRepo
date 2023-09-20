using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers.Reports
{
    public class HighValueInspecReportController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
