using IBS.Filters;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
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
            string OrgnType = SessionHelper.UserModelDTO.OrgnType.Trim();
            string Organisation = SessionHelper.UserModelDTO.Organisation.Trim();
            DashboardModel model = dashboardRepository.GetClientDashBoardCount(OrgnType, Organisation);
            return View(model);
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
            DashboardModel model = dashboardRepository.GetIEDDashBoardCount(SessionHelper.UserModelDTO.CoCd);
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
        public IActionResult TotalInvoice(int Flag)
        {
            ViewBag.Flag = Flag;
            return View();
        }
        [HttpPost]
        public IActionResult LoadTableInvoice([FromBody] DTParameters dtParameters)
        {
            string Regin = GetRegionCode;
            DTResult<LabReportsModel> dTResult = new DTResult<LabReportsModel>();
            try
            {
                dTResult = dashboardRepository.LoadTableInvoice(dtParameters, Regin);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Dashboard", "LoadTableInvoice", 1, GetIPAddress());
            }
            return Json(dTResult);
        }
        public IActionResult TotalReportUploaded()
        {
            
            return View();
        }
        [HttpPost]
        public IActionResult LoadTableReportU([FromBody] DTParameters dtParameters)
        {
            string Regin = GetRegionCode;
            DTResult<LabSampleInfoModel> dTResult = new DTResult<LabSampleInfoModel>();
            try
            {
                dTResult = dashboardRepository.LoadTableReportU(dtParameters, Regin);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Dashboard", "LoadTableReportU", 1, GetIPAddress());
            }
            return Json(dTResult);
        }
        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<IEMessagesModel> dTResult = userRepository.GetUserList(dtParameters,GetRegionCode);
            return Json(dTResult);
        }
    }
}
