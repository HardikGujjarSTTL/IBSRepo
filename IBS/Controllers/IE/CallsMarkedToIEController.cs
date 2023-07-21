using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers.IE
{
    public class CallsMarkedToIEController : BaseController
    {
        #region Variables
        private readonly ICallMarkedToIERepository callmarksRepository;
        public string PType="";
        #endregion

        public CallsMarkedToIEController(ICallMarkedToIERepository _callmarksRepository)
        {
            callmarksRepository = _callmarksRepository;
        }
        public IActionResult CallsMarkedToIE(string type)
        {
            CallsMarkedToIEModel model = new();
            model.PType = type;

            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {

            DTResult<CallsMarkedToIEModel> dTResult = callmarksRepository.GetDataList(dtParameters, GetRegionCode, Convert.ToString(UserId), GetIeCd);
            return Json(dTResult);
        }
    }
}
