﻿using IBS.Filters;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using IBS.Repositories.Vendor;
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
            DashboardModel model = dashboardRepository.GetDashBoardCount(SessionHelper.UserModelDTO.UserID);
            return View(model);
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

        public IActionResult CMGeneral()
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
            DTResult<VenderCallRegisterModel> dTResult = dashboardRepository.GetDataListTotalCallListing(dtParameters, Region);
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
            ViewBag.Types = Type;
            ViewBag.IeCdCode = SessionHelper.UserModelDTO.IeCd;
            return View();
        }

        [HttpPost]
        public IActionResult GetIEDashboardDetailsList([FromBody] DTParameters dtParameters)
        {
            DTResult<DashboardModel> dTResult = dashboardRepository.Get_IE_Dashboard_Details_List(dtParameters);
            return Json(dTResult);
        }
    }
}
