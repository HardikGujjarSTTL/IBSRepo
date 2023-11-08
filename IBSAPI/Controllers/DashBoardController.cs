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
        [HttpGet("Get_IE_Count", Name = "Get_IE")]
        public IActionResult Get_IE(int IeCd)
        {
            try
            {
                var startDate = Common.GetFinancialYearStartDate();
                var ToDate = DateTime.Today.ToString("dd/MM/yyyy");
                var totalInsp = dashBoardRepository.GetIETotalAssignInspection(IeCd, startDate.ToString("dd/MM/yyyy"), ToDate);

                var totalCompletedInsp = dashBoardRepository.GetIECompletedInspection(IeCd, startDate.ToString("dd/MM/yyyy"), ToDate);

                var totalPendingInsp = dashBoardRepository.GetIEPendingInspection(IeCd, startDate.ToString("dd/MM/yyyy"), ToDate);

                var result = new
                {
                    Total_Inspection = totalInsp,
                    Completed_Inspection = totalCompletedInsp,
                    Pending_Inspection = totalPendingInsp
                };

                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                    message = "Data get successfully",
                    data = result
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "DashBoard_API", "Get_IE", 1, string.Empty);
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
        [HttpGet("Get_Vendor_Count", Name = "Get_Vendor")]
        public IActionResult Get_Vendor(int Vendor_Cd)
        {
            try
            {
                var startDate = Common.GetFinancialYearStartDate();
                var ToDate = DateTime.Today.ToString("dd/MM/yyyy");
                var totalInsp = dashBoardRepository.GetVendorTotalAssignInspection(Vendor_Cd, startDate.ToString("dd/MM/yyyy"), ToDate);

                var totalCompletedInsp = dashBoardRepository.GetVendorCompletedInspection(Vendor_Cd, startDate.ToString("dd/MM/yyyy"), ToDate);

                var totalPendingInsp = dashBoardRepository.GetVendorPendingInspection(Vendor_Cd, startDate.ToString("dd/MM/yyyy"), ToDate);

                var result = new
                {
                    Total_Inspection = totalInsp,
                    Completed_Inspection = totalCompletedInsp,
                    Pending_Inspection = totalPendingInsp
                };

                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                    message = "Data get successfully",
                    data = result
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "DashBoard_API", "Get_Vendor", 1, string.Empty);
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
        [HttpGet("Get_CM_Count", Name = "Get_CM")]
        public IActionResult Get_CM(int CO_CD)
        {
            try
            {
                var startDate = Common.GetFinancialYearStartDate();
                var ToDate = DateTime.Today.ToString("dd/MM/yyyy");
                var totalInsp = dashBoardRepository.Get_CM_TotalInspection(CO_CD, startDate.ToString("dd/MM/yyyy"), ToDate);

                var totPendingInsp = dashBoardRepository.Get_CM_PendingInspection(CO_CD, startDate.ToString("dd/MM/yyyy"), ToDate);

                var totRejectedInsp = dashBoardRepository.Get_CM_RequestRejectedInspection(CO_CD, startDate.ToString("dd/MM/yyyy"), ToDate);
                var result = new
                {
                    Total_Inspection = totalInsp,
                    Total_Assign_Inspection = totalInsp,
                    Pending_Inspection = totPendingInsp,
                    Rejected_Inspection = totRejectedInsp
                };

                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                    message = "Data get successfully",
                    data = result
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "DashBoard_API", "Get_CM", 1, string.Empty);
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                    message = ex.Message.ToString(),
                };
                return Ok(response);
            }
        }        

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
        #endregion

        #region Client
        [HttpGet("Get_Client_Count", Name ="Get_Client")]
        public IActionResult Get_Client(string Rly_CD, string Rly_NoNType)
        {
            try
            {
                var startDate = Common.GetFinancialYearStartDate();
                var ToDate = DateTime.Today.ToString("dd/MM/yyyy");
                var totalInsp = dashBoardRepository.GetClientTotalInspection(Rly_CD, Rly_NoNType, startDate.ToString("dd/MM/yyyy"), ToDate);
                var totalCompletedInsp = dashBoardRepository.GetClientCompletedInspection(Rly_CD, Rly_NoNType, startDate.ToString("dd/MM/yyyy"), ToDate);
                var totalPendingInsp = dashBoardRepository.GetClientPendingInspection(Rly_CD, Rly_NoNType, startDate.ToString("dd/MM/yyyy"), ToDate);

                var result = new
                {
                    Total_Inspection = totalInsp,
                    Completed_Inspection = totalCompletedInsp,
                    Pending_Inspection = totalPendingInsp
                };

                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                    message = "Data get successfully",
                    data = result
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "DashBoard_API", "Get_Client", 1, string.Empty);
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
