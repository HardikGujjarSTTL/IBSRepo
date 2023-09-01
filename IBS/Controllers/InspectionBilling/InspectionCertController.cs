using IBS.Interfaces.InspectionBilling;
using IBS.Interfaces;
using Microsoft.AspNetCore.Mvc;
using IBS.Models;
using IBS.Repositories.Vendor;

namespace IBS.Controllers.InspectionBilling
{
    public class InspectionCertController : BaseController
    {
        #region Variables
        private readonly IInspectionCertRepository inpsRepository;

        #endregion
        public InspectionCertController(IInspectionCertRepository _inpsRepository)
        {
            inpsRepository = _inpsRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<InspectionCertModel> dTResult = inpsRepository.GetDataList(dtParameters, GetRegionCode);
            return Json(dTResult);
        }
    }
}
