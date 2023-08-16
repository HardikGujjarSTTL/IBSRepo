using IBS.Interfaces;
using IBS.Interfaces.IE;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers.IE
{
    public class IEJIRemarksPendingController : BaseController
    {
        #region Variables
        private readonly IIEJIRemarksPendingRepository iejiRepository;
        #endregion

        public IEJIRemarksPendingController(IIEJIRemarksPendingRepository _iejiRepository)
        {
            iejiRepository = _iejiRepository;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
