using IBSAPI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IBSAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class InspectionController : ControllerBase
    {
        #region Varible
        private readonly IInspectionRepository inspectionRepository;
        #endregion
        public InspectionController(IInspectionRepository _inspectionRepository)
        {
            inspectionRepository = _inspectionRepository;
        }

        public IActionResult GetTodayInspection()
        {
            var detail = inspectionRepository.GetToDayInspection();
            if(detail.Count() > 0)
            {

            }
            else
            {

            }
            return Ok();
        }
    }
}
