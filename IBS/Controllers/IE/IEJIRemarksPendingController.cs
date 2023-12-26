using IBS.Interfaces.IE;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers.IE
{
    public class IEJIRemarksPendingController : BaseController
    {
        #region Variables
        private readonly IIEJIRemarksPendingRepository iejiRepository;
        #endregion

        public IEJIRemarksPendingController(IIEJIRemarksPendingRepository _iejiRepository)
        {
            iejiRepository = _iejiRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<IEJIRemarksPendingModel> dTResult = iejiRepository.GetDataList(dtParameters, GetRegionCode, Convert.ToString(UserId), GetIeCd);
            return Json(dTResult);
        }
    }
}
