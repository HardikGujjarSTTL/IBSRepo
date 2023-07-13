using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    //[Authorize]
    public class DashboardController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
