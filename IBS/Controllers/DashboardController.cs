using IBS.Filters;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using IBS.Repositories.Vendor;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;

namespace IBS.Controllers
{
    [Authorization]
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
            if (SessionHelper.UserModelDTO.RoleName.ToLower() == "admin")
            {
                DashboardModel model = dashboardRepository.GetDashBoardCount(SessionHelper.UserModelDTO.Region, SessionHelper.UserModelDTO.RoleName.ToLower());
                return View(model);
            }
            else if (SessionHelper.UserModelDTO.RoleName.ToLower() == "inspection engineer (ie)")
            {
                return RedirectToAction("IE", "Dashboard");
            }
            else if (SessionHelper.UserModelDTO.RoleName.ToLower() == "vendor")
            {
                return RedirectToAction("Vendor", "Dashboard");
            }
            else if (SessionHelper.UserModelDTO.RoleName.ToLower() == "client")
            {
                return RedirectToAction("Client", "Dashboard");
            }
            else if (SessionHelper.UserModelDTO.RoleName.ToLower() == "liasioning officer (lo)")
            {
                return RedirectToAction("LO", "Dashboard");
            }
            else if (SessionHelper.UserModelDTO.RoleName.ToLower() == "lab user")
            {
                return RedirectToAction("LAB", "Dashboard");
            }
            else if (SessionHelper.UserModelDTO.RoleName.ToLower() == "cm-call desk incharge")
            {
                return RedirectToAction("CM", "Dashboard");
            }
            else if (SessionHelper.UserModelDTO.RoleName.ToLower() == "cm-d&a incharge")
            {
                return RedirectToAction("CMDAR", "Dashboard");
            }
            else if (SessionHelper.UserModelDTO.RoleName.ToLower() == "cm-dfo")
            {
                return RedirectToAction("CMDFO", "Dashboard");
            }
            else if (SessionHelper.UserModelDTO.RoleName.ToLower() == "cm-ji incharge")
            {
                return RedirectToAction("CMJIIncharge", "Dashboard");
            }
            else if (SessionHelper.UserModelDTO.RoleName.ToLower() == "cm-general")
            {
                return RedirectToAction("CMGeneral", "Dashboard");
            }
            else
            {
                DashboardModel model = dashboardRepository.GetDashBoardCount(SessionHelper.UserModelDTO.Region, SessionHelper.UserModelDTO.RoleName.ToLower());
                return View(model);
            }
        }

        public IActionResult Client()
        {
            string RegionCode = SessionHelper.UserModelDTO.Region;
            string OrgnType = SessionHelper.UserModelDTO.OrgnType.Trim();
            string Organisation = SessionHelper.UserModelDTO.Organisation.Trim();
            string RoleName = SessionHelper.UserModelDTO.RoleName.Trim().ToLower();
            DashboardModel model = dashboardRepository.GetClientDashBoardCount(OrgnType, Organisation, RegionCode, RoleName);
            return View(model);
        }

        public IActionResult Vendor()
        {
            string RegionCode = SessionHelper.UserModelDTO.Region;
            int Vend_Cd = Convert.ToInt32(SessionHelper.UserModelDTO.UserName.Trim());
            string RoleName = SessionHelper.UserModelDTO.RoleName.Trim().ToLower();
            DashboardModel model = dashboardRepository.GetVendorDashBoardCount(Vend_Cd, RegionCode, RoleName);
            return View(model);
        }

        public IActionResult IE()
        {
            string RegionCode = SessionHelper.UserModelDTO.Region;
            string RoleName = SessionHelper.UserModelDTO.RoleName.Trim().ToLower();
            DashboardModel model = dashboardRepository.GetIEDDashBoardCount(SessionHelper.UserModelDTO.IeCd, RegionCode, RoleName);
            return View(model);
        }

        #region CM
        public IActionResult CM()
        {
            DashboardModel model = dashboardRepository.GetCMDashBoardCount(SessionHelper.UserModelDTO.CoCd);
            return View(model);
        }

        public IActionResult AwaitingForCaseNo()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoadAwaitingForCaseNoTable([FromBody] DTParameters dtParameters)
        {
            DTResult<PO_MasterModel> dTResult = dashboardRepository.GetPOMasterList(dtParameters);
            return Json(dTResult);
        }

        #endregion
        public IActionResult IE_Instructions()
        {
            return View();
        }

        public IActionResult CMDAR()
        {
            DashboardModel model = dashboardRepository.GetCMDARDashBoard(SessionHelper.UserModelDTO.CoCd);
            return View(model);
        }

        public IActionResult CMDFO()
        {
            DashboardModel model = dashboardRepository.GetCMDFODashBoard(SessionHelper.UserModelDTO.CoCd);
            return View(model);
        }

        public IActionResult CMJIIncharge()
        {
            DashboardModel model = dashboardRepository.GetCMJIDDashBoard(SessionHelper.UserModelDTO.CoCd);
            return View(model);
        }

        public IActionResult CMGeneral()
        {
            DashboardModel model = dashboardRepository.GetCMGeneralDashBoard(SessionHelper.UserModelDTO.CoCd);
            return View(model);
        }

        public IActionResult LO()
        {
            DashboardModel model = dashboardRepository.GetLODashBoardCount(SessionHelper.UserModelDTO.UserName.Trim());
            return View(model);
        }

        public IActionResult LAB()
        {
            int userid = UserId;
            string Regin = GetRegionCode;
            DashboardModel model = dashboardRepository.GetDashBoardLabCount(userid, Regin);
            return View(model);
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
            int userid = UserId;
            DTResult<DashboardLabData> dTResult = new DTResult<DashboardLabData>();
            try
            {
                dTResult = dashboardRepository.LoadTableInvoice(dtParameters, Regin, UserId);
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
            DTResult<IEMessagesModel> dTResult = userRepository.GetUserList(dtParameters, GetRegionCode);
            return Json(dTResult);
        }

        public IActionResult TotalCallListing()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoadDTotalCallListing([FromBody] DTParameters dtParameters)
        {
            DTResult<AdminCountListing> dTResult = dashboardRepository.GetDataListTotalCallListing(dtParameters, Region);
            return Json(dTResult);
        }

        public IActionResult CallDeskInfo()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CallDeskInfoListing([FromBody] DTParameters dtParameters)
        {
            DTResult<VenderCallRegisterModel> dTResult = dashboardRepository.GetDataCallDeskInfoListing(dtParameters, Region);
            return Json(dTResult);
        }

        public IActionResult IEPerCM()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetIEPerCM([FromBody] DTParameters dtParameters)
        {
            DTResult<IE_Per_CM_Model> dTResult = dashboardRepository.Get_CM_Wise_IE_Detail(dtParameters);
            return Json(dTResult);
        }

        public IActionResult IE_Dashboard_Detail(string Type)
        {
            DashboardModel model = new();
            model.ActionType = Type;
            ViewBag.IeCdCode = SessionHelper.UserModelDTO.IeCd;
            return View(model);
        }

        [HttpPost]
        public IActionResult GetIEDashboardDetailsList([FromBody] DTParameters dtParameters)
        {
            DTResult<IEList> model = dashboardRepository.Get_IE_Dashboard_Details_List(dtParameters);
            return Json(model);
        }
        [HttpGet]
        public IActionResult NOOfRegisterCount()
        {
            LabSampleInfoModel model = new LabSampleInfoModel();

            try
            {
                string Regin = GetRegionCode;
                model = dashboardRepository.GetNOOfRegisterCount(Regin);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Dashboard", "NOOfRegisterCount", 1, GetIPAddress());
            }
            return Json(model);
        }

        public IActionResult VendorDetail(string Type)
        {
            ViewBag.Type = Type;
            return View();
        }

        [HttpPost]
        public IActionResult LoadVendorDetail([FromBody] DTParameters dtParameters)
        {
            DTResult<VendorDetailListModel> dTResult = dashboardRepository.GetDataVendorListing(dtParameters, GetUserInfo.UserName);
            return Json(dTResult);
        }

        public IActionResult Dashboard_Admin_ViewAll_List(string Type)
        {
            DashboardModel model = new();
            model.ActionType = Type;
            return View();
        }

        [HttpPost]
        public IActionResult LoadDashboard_Admin_ViewAll_List([FromBody] DTParameters dtParameters)
        {
            string RegionCode = SessionHelper.UserModelDTO.Region;
            DTResult<AdminViewAllList> dTResult = dashboardRepository.Dashboard_Admin_ViewAll_List(dtParameters, RegionCode);
            return Json(dTResult);
        }

        public IActionResult Dashboard_Vendor_ViewAll_List(string Type)
        {
            DashboardModel model = new();
            model.ActionType = Type;
            return View();
        }

        [HttpPost]
        public IActionResult LoadDashboard_Vendor_ViewAll_List([FromBody] DTParameters dtParameters)
        {
            string RegionCode = SessionHelper.UserModelDTO.Region;
            int Vend_Cd = Convert.ToInt32(SessionHelper.UserModelDTO.UserName.Trim());
            DTResult<VendorViewAllList> dTResult = dashboardRepository.Dashboard_Vendor_ViewAll_List(dtParameters, RegionCode, Vend_Cd);
            return Json(dTResult);
        }

        public IActionResult Dashboard_IE_ViewAll_List(string Type)
        {
            DashboardModel model = new();
            model.ActionType = Type;
            return View();
        }

        [HttpPost]
        public IActionResult LoadDashboard_IE_ViewAll_List([FromBody] DTParameters dtParameters)
        {
            string RegionCode = SessionHelper.UserModelDTO.Region;
            int IE_CD = SessionHelper.UserModelDTO.IeCd;
            DTResult<IEViewAllList> dTResult = dashboardRepository.Dashboard_IE_ViewAll_List(dtParameters, IE_CD, RegionCode);
            return Json(dTResult);
        }

        public IActionResult LoCallListing()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetLoCallListing([FromBody] DTParameters dtParameters)
        {
            DTResult<LoListingModel> dTResult = dashboardRepository.GetLoCallListingDetails(dtParameters, UserName.Trim());
            return Json(dTResult);
        }
        
        public IActionResult CMDARListing()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetCMDARListing([FromBody] DTParameters dtParameters)
        {
            DTResult<CMDARListing> dTResult = dashboardRepository.CMDARListing(dtParameters);
            return Json(dTResult);
        }

        public IActionResult Dashboard_Client_ViewAll_List(string Type)
        {
            CLientViewAllList model = new();
            model.ActionType = Type;
            return View();
        }

        [HttpPost]
        public IActionResult LoadDashboard_Client_ViewAll_List([FromBody] DTParameters dtParameters)
        {
            string RegionCode = SessionHelper.UserModelDTO.Region;
            string OrgnType = SessionHelper.UserModelDTO.OrgnType.Trim();
            string Organisation = SessionHelper.UserModelDTO.Organisation.Trim();
            DTResult<CLientViewAllList> dTResult = dashboardRepository.Dashboard_Client_ViewAll_List(dtParameters, RegionCode, OrgnType, Organisation);
            return Json(dTResult);
        }

        public IActionResult Dashboard_Client_List()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoadDashboard_Client_List([FromBody] DTParameters dtParameters)
        {
            string OrgnType = SessionHelper.UserModelDTO.OrgnType.Trim();
            string Organisation = SessionHelper.UserModelDTO.Organisation.Trim();
            DTResult<AdminCountListing> dTResult = dashboardRepository.Dashboard_Client_List(dtParameters, Region, OrgnType, Organisation);
            return Json(dTResult);
        }

        public IActionResult CMDFO_List(string Type)
        {
            CMDFOListing model = new();
            model.ActionType = Type;
            return View();
        }

        [HttpPost]
        public IActionResult LoadCMDFO_List([FromBody] DTParameters dtParameters)
        {
            DTResult<CMDFOListing> dTResult = dashboardRepository.CMDFO_List(dtParameters);
            return Json(dTResult);
        }
        
        public IActionResult Dashboard_CMGeneral_ViewAll_List()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoadDashboard_CMGeneral_ViewAll_List([FromBody] DTParameters dtParameters)
        {
            string COCD = SessionHelper.UserModelDTO.CoCd.ToString();
            DTResult<AdminCountListing> dTResult = dashboardRepository.Dashboard_CMGeneral_ViewAll_List(dtParameters, COCD);
            return Json(dTResult);
        }
        
        public IActionResult Dashboard_CMDFO_ViewAll_List(string Type)
        {
            AdminViewAllList model = new();
            model.ActionType = Type;
            return View();
        }

        [HttpPost]
        public IActionResult LoadDashboard_CMDFO_ViewAll_List([FromBody] DTParameters dtParameters)
        {
            DTResult<AdminViewAllList> dTResult = dashboardRepository.Dashboard_CMDFO_ViewAll_List(dtParameters);
            return Json(dTResult);
        }
    }
}
