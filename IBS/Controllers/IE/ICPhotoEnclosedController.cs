using IBS.Interfaces.IE;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers.IE
{
    public class ICPhotoEnclosedController : BaseController
    {
        #region Variables
        private readonly IICPhotoEnclosedRepository ieRepository;
        #endregion
        public ICPhotoEnclosedController(IICPhotoEnclosedRepository _ieRepository)
        {
            ieRepository = _ieRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<ICPhotoEnclosedModel> dTResult = ieRepository.GetDataList(dtParameters, GetRegionCode, GetIeCd);
            return Json(dTResult);
        }
    }
}
