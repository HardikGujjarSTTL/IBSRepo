using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers.WebsitePages
{
    public class FHomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
