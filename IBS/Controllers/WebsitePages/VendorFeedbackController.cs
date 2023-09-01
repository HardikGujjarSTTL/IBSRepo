using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers.WebsitePages
{
    public class VendorFeedbackController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
