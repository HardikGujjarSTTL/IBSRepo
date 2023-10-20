using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers.Reports
{
    public class BPOWiseOutstandingBillsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
