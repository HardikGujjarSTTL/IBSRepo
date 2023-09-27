using IBS.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class CallMarkedController : BaseController
    {
        private readonly ICallMarkedRepository callmarkedrepository;
        public CallMarkedController(ICallMarkedRepository _callmarkedrepository)
        {
            callmarkedrepository = _callmarkedrepository;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
