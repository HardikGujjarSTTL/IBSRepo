using IBS.Filters;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class MultiDocUploadListController : BaseController
    {
        [Authorization("MultiDocUploadList", "Index", "view")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
