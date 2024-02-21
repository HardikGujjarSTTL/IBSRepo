using IBS.Filters;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    [Authorization]
    public class ManPowerQAController : BaseController
    {

        [Authorization("ManPowerQA", "Index", "view")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorization("ManPowerQA", "Index", "edit")]
        public IActionResult Manage()
        {
            return View();
        }
    }
}
