using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers.Reports.ManPower
{
    public class ManpowerNoOfStaffInQAReportController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
