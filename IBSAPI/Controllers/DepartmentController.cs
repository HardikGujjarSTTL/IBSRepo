using IBSAPI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IBSAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        #region Variables
        private readonly IDepartmentRepository departmentRepository;
        #endregion
        public DepartmentController(IDepartmentRepository _departmentRepository)
        {
            departmentRepository = _departmentRepository;
        }

        [HttpGet("GetDeparmentList", Name = "GetDeparmentList")]
        public IActionResult GetDeparmentList()
        {
            var deptList = departmentRepository.GetDepartmentList();
            if (deptList.Count() > 0)
            {
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                    message = "Data get successfully",
                    data = deptList
                };
                return Ok(response);
            }
            else
            {
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                    message = "No Date Found",
                };
                return Ok(response);
            }
        }
    }
}
