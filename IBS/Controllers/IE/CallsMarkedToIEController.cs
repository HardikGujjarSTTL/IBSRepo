using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using IBS.Models.Reports;
using IBS.Repositories.Reports;
using Microsoft.AspNetCore.Mvc;
using IBS.Helper;

namespace IBS.Controllers.IE
{
    public class CallsMarkedToIEController : BaseController
    {
        #region Variables
        private readonly ICallMarkedToIERepository callmarksRepository;
        public string PType = "";
        private readonly IWebHostEnvironment env;
        #endregion

        public CallsMarkedToIEController(ICallMarkedToIERepository _callmarksRepository, IWebHostEnvironment _env)
        {
            callmarksRepository = _callmarksRepository;
            this.env = _env;
        }
        public IActionResult CallsMarkedToIE(string type)
        {
            CallsMarkedToIEModel model = new();
            string Fpath = $"{Request.Scheme}://{Request.Host}";
            var CaseNoPath = env.WebRootPath + Enums.GetEnumDescription(Enums.FolderPath.CaseNo);
            var LabPath = env.WebRootPath + Enums.GetEnumDescription(Enums.FolderPath.Lab);
            model = callmarksRepository.GetReport(GetIeCd, Convert.ToString(UserId), type);
            model.PType = type;
            model.IeName = IeName;

            model.FilePath1 = Fpath;
            model.FilePath2 = CaseNoPath;
            model.FilePath3 = LabPath;

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
