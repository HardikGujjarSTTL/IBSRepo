using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers.Reports.ManPower
{
    public class ManpowerNoOfStaffInProjectReportController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
