using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class DashboardController : BaseController
    {
        private readonly IIEMessageRepository userRepository;
        private readonly IDashboardRepository dashboardRepository;

        public DashboardController(IIEMessageRepository _userRepository, IDashboardRepository _dashboardRepository)
        {
            userRepository = _userRepository;
            dashboardRepository = _dashboardRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Client()
        {
            return View();
        }

        public IActionResult Vendor()
        {
            int Vend_Cd = Convert.ToInt32(SessionHelper.UserModelDTO.UserName.Trim());
            DashboardModel model = dashboardRepository.GetVendorDashBoardCount(Vend_Cd);
            return View(model);
        }

        public IActionResult IE()
        {
            DashboardModel model = dashboardRepository.GetIEDDashBoardCount(SessionHelper.UserModelDTO.IeCd);
            return View(model);
        }

        public IActionResult CM()
        {
            return View();
        }

        public IActionResult IE_Instructions()
        {
            return View();
        }

        public IActionResult CMDAR()
        {
            return View();
        }

        public IActionResult CMDFO()
        {
            return View();
        }

        public IActionResult CMJIIncharge()
        {
            return View();
        }

        public IActionResult LO()
        {
            return View();
        }

        public IActionResult LAB()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<IEMessagesModel> dTResult = userRepository.GetUserList(dtParameters,GetRegionCode);
            return Json(dTResult);
        }
    }
}
