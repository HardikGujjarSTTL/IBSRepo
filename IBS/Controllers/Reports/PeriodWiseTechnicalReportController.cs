using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers.Reports
{
    public class PeriodWiseTechnicalReportController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
