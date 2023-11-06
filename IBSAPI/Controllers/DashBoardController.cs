using IBSAPI.Interfaces;
using IBSAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IBSAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DashBoardController : ControllerBase
    {
        #region Variables
        private readonly IDashBoardRepository dashBoardRepository;
        private readonly IWebHostEnvironment _env;
        #endregion
        public DashBoardController(IDashBoardRepository _dashBoardRepository, IWebHostEnvironment env)
        {
            dashBoardRepository = _dashBoardRepository;
            _env = env;
        }

        #region IE
        [HttpGet("Get_IE_TotalAssignInspection_Count", Name = "GetIETotalAssignInspection")]
        public IActionResult GetIETotalAssignInspection(int IeCd)
        {
            try
            {
                var startDate = Common.GetFinancialYearStartDate();
                var ToDate = DateTime.Today.ToString("dd/MM/yyyy");
                var totalInsp = dashBoardRepository.GetIETotalAssignInspection(IeCd, startDate.ToString("dd/MM/yyyy"), ToDate);
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                    message = "Data get successfully",
                    data = totalInsp
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "DashBoard_API", "GetIETotalAssignInspection", 1, string.Empty);
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                    message = ex.Message.ToString(),
                };
                return Ok(response);
            }
        }

        [HttpGet("Get_IE_CompletedInspection_Count", Name = "GetIECompletedInspection")]
        public IActionResult GetIECompletedInspection(int IeCd)
        {
            try
            {
                var startDate = Common.GetFinancialYearStartDate();
                var ToDate = DateTime.Today.ToString("dd/MM/yyyy");
                var totalCompletedInsp = dashBoardRepository.GetIECompletedInspection(IeCd, startDate.ToString("dd/MM/yyyy"), ToDate);
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                    message = "Data get successfully",
                    data = totalCompletedInsp
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "DashBoard_API", "GetIECompletedInspection", 1, string.Empty);
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                    message = ex.Message.ToString(),
                };
                return Ok(response);
            }
        }

        [HttpGet("Get_IE_PendingInspection_Count", Name = "GetIEPendingInspection")]
        public IActionResult GetIEPendingInspection(int IeCd)
        {
            try
            {
                var startDate = Common.GetFinancialYearStartDate();
                var ToDate = DateTime.Today.ToString("dd/MM/yyyy");
                var totalPendingInsp = dashBoardRepository.GetIEPendingInspection(IeCd, startDate.ToString("dd/MM/yyyy"), ToDate);
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                    message = "Data get successfully",
                    data = totalPendingInsp
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "DashBoard_API", "GetIEPendingInspection", 1, string.Empty);
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                    message = ex.Message.ToString(),
                };
                return Ok(response);
            }
        }
        #endregion

        #region Vendor
        [HttpGet("Get_Vendor_TotalAssignInspection_Count", Name = "GetVendorTotalAssignInspection")]
        public IActionResult GetVendorTotalAssignInspection(int Vendor_Cd)
        {
            try
            {
                var startDate = Common.GetFinancialYearStartDate();
                var ToDate = DateTime.Today.ToString("dd/MM/yyyy");
                var totalInsp = dashBoardRepository.GetVendorTotalAssignInspection(Vendor_Cd, startDate.ToString("dd/MM/yyyy"), ToDate);
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                    message = "Data get successfully",
                    data = totalInsp
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "DashBoard_API", "GetVendorTotalAssignInspection", 1, string.Empty);
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                    message = ex.Message.ToString(),
                };
                return Ok(response);
            }
        }

        [HttpGet("Get_Vendor_CompletedInspection_Count", Name = "GetVendorCompletedInspection")]
        public IActionResult GetVendorCompletedInspection(int Vendor_Cd)
        {
            try
            {
                var startDate = Common.GetFinancialYearStartDate();
                var ToDate = DateTime.Today.ToString("dd/MM/yyyy");
                var totalCompletedInsp = dashBoardRepository.GetVendorCompletedInspection(Vendor_Cd, startDate.ToString("dd/MM/yyyy"), ToDate);
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                    message = "Data get successfully",
                    data = totalCompletedInsp
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "DashBoard_API", "GetVendorCompletedInspection", 1, string.Empty);
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                    message = ex.Message.ToString(),
                };
                return Ok(response);
            }
        }

        [HttpGet("Get_Vendor_PendingInspection_Count", Name = "GetVendorPendingInspection")]
        public IActionResult GetVendorPendingInspection(int Vendor_Cd)
        {
            try
            {
                var startDate = Common.GetFinancialYearStartDate();
                var ToDate = DateTime.Today.ToString("dd/MM/yyyy");
                var totalPendingInsp = dashBoardRepository.GetVendorPendingInspection(Vendor_Cd, startDate.ToString("dd/MM/yyyy"), ToDate);
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                    message = "Data get successfully",
                    data = totalPendingInsp
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "DashBoard_API", "GetVendorPendingInspection", 1, string.Empty);
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                    message = ex.Message.ToString(),
                };
                return Ok(response);
            }
        }
        #endregion

        #region Client
        [HttpGet("GetClientTotalInspection", Name = "GetClientTotalInspection")]
        public IActionResult GetClientTotalInspection(string Rly_CD, string RlyNoNType)
        {
            try
            {
                var startDate = Common.GetFinancialYearStartDate();
                var ToDate = DateTime.Today.ToString("dd/MM/yyyy");
                var totalInsp = dashBoardRepository.GetClientTotalInspection(Rly_CD, RlyNoNType, startDate.ToString("dd/MM/yyyy"), ToDate);
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                    message = "Data get successfully",
                    data = totalInsp
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "DashBoard_API", "GetClientTotalInspection", 1, string.Empty);
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                    message = ex.Message.ToString(),
                };
                return Ok(response);
            }
        }
        #endregion

        #region CM
        [HttpGet("Get_CM_TotalIE_Count", Name = "GetCMTotalIE")]
        public IActionResult GetCMTotalIE(int CO_CD)
        {
            try
            {
                var totalInsp = dashBoardRepository.Get_CM_Wise_IE(CO_CD);
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                    message = "Data get successfully",
                    data = totalInsp.Count()
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "DashBoard_API", "GetCMTotalIE", 1, string.Empty);
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                    message = ex.Message.ToString(),
                };
                return Ok(response);
            }
        }

        [HttpGet("Get_CM_Wise_IE_List", Name = "GetCMWiseIE")]
        public IActionResult GetCMWiseIE(int CO_CD)
        {
            try
            {
                var totalInsp = dashBoardRepository.Get_CM_Wise_IE(CO_CD);
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                    message = "Data get successfully",
                    data = totalInsp
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "DashBoard_API", "GetCMWiseIE", 1, string.Empty);
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                    message = ex.Message.ToString(),
                };
                return Ok(response);
            }
        }

        [HttpGet("Get_CM_TotalInpspection_Count", Name = "Get_CM_Total_Inpspection_count")]
        public IActionResult Get_CM_Total_Inpspection_count(int CO_CD)
        {
            try
            {
                var startDate = Common.GetFinancialYearStartDate();
                var ToDate = DateTime.Today.ToString("dd/MM/yyyy");
                var totalInsp = dashBoardRepository.Get_CM_TotalInspection(CO_CD, startDate.ToString("dd/MM/yyyy"), ToDate);
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                    message = "Data get successfully",
                    data = totalInsp
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "DashBoard_API", "Get_CM_Total_Inpspection_count", 1, string.Empty);
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                    message = ex.Message.ToString(),
                };
                return Ok(response);
            }
        }

        [HttpGet("Get_CM_PendingInspection_Count", Name = "Get_CM_PendingInspection")]
        public IActionResult Get_CM_PendingInspection(int CO_CD)
        {
            try
            {
                var startDate = Common.GetFinancialYearStartDate();
                var ToDate = DateTime.Today.ToString("dd/MM/yyyy");
                var totalInsp = dashBoardRepository.Get_CM_PendingInspection(CO_CD, startDate.ToString("dd/MM/yyyy"), ToDate);
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                    message = "Data get successfully",
                    data = totalInsp
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "DashBoard_API", "Get_CM_PendingInspection", 1, string.Empty);
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                    message = ex.Message.ToString(),
                };
                return Ok(response);
            }
        }

        [HttpGet("Get_CM_RequestRejectedInspection_Count", Name = "Get_CM_RequestRejectedInspection")]
        public IActionResult Get_CM_RequestRejectedInspection(int CO_CD)
        {
            try
            {
                var startDate = Common.GetFinancialYearStartDate();
                var ToDate = DateTime.Today.ToString("dd/MM/yyyy");
                var totalInsp = dashBoardRepository.Get_CM_RequestRejectedInspection(CO_CD, startDate.ToString("dd/MM/yyyy"), ToDate);
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                    message = "Data get successfully",
                    data = totalInsp
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "DashBoard_API", "Get_CM_RequestRejectedInspection", 1, string.Empty);
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                    message = ex.Message.ToString(),
                };
                return Ok(response);
            }
        }
        #endregion
    }
}
