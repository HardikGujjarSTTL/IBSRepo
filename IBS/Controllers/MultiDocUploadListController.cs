using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class MultiDocUploadListController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
