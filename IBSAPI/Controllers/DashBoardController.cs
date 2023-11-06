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
        [HttpGet("GetIETotalAssignInspection", Name = "GetIETotalAssignInspection")]
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

        [HttpGet("GetIECompletedInspection", Name = "GetIECompletedInspection")]
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

        [HttpGet("GetIEPendingInspection", Name = "GetIEPendingInspection")]
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
        [HttpGet("GetVendorTotalAssignInspection", Name = "GetVendorTotalAssignInspection")]
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

        [HttpGet("GetVendorCompletedInspection", Name = "GetVendorCompletedInspection")]
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

        [HttpGet("GetVendorPendingInspection", Name = "GetVendorPendingInspection")]
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
        public IActionResult GetClientTotalInspection(string Rly_CD, string RlyNoNType)
        {
            try
            {
                var startDate = Common.GetFinancialYearStartDate();
                var ToDate = DateTime.Today.ToString("dd/MM/yyyy");
                var totalInsp = dashBoardRepository.GetClientTotalInspection(Rly_CD, RlyNoNType, startDate.ToString("dd/MM.yyyy"), ToDate);
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
    }
}
