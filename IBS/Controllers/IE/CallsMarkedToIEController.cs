using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using IBS.Models.Reports;
using IBS.Repositories.Reports;
using Microsoft.AspNetCore.Mvc;
using static IBS.Helper.Enums;

namespace IBS.Controllers.IE
{
    public class CallsMarkedToIEController : BaseController
    {
        #region Variables
        private readonly ICallMarkedToIERepository callmarksRepository;
        public string PType = "";
        #endregion

        public CallsMarkedToIEController(ICallMarkedToIERepository _callmarksRepository)
        {
            callmarksRepository = _callmarksRepository;
        }
        public IActionResult CallsMarkedToIE(string type)
        {
            CallsMarkedToIEModel model = new();
            model = callmarksRepository.GetReport(GetIeCd, Convert.ToString(UserId), type);
            model.PType = type;
            model.IeName = IeName;

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
