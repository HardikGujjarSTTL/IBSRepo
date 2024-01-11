using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class AllGeneratedBillsController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
