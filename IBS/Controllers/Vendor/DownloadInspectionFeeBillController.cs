using IBS.Interfaces.Vendor;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers.Vendor
{
    public class DownloadInspectionFeeBillController : BaseController
    {
        #region Variables
        private readonly IDownloadInspFeeBillRepository downloadRepository;
        private readonly IConfiguration config;
        #endregion
        public DownloadInspectionFeeBillController(IDownloadInspFeeBillRepository _downloadRepository, IConfiguration _config)
        {
            downloadRepository = _downloadRepository;
            config = _config;
        }

        public IActionResult Index(string CaseNo, string BkNo, string SetNo)
        {
            ViewBag.ReportUrl = config.GetSection("AppSettings")["ReportUrl"];
            DownloadInspectionFeeBillModel model = new();
            if (CaseNo != null && BkNo != null && SetNo != null)
            {
                model.CaseNo = CaseNo;
                model.BkNo = BkNo;
                model.SetNo = SetNo;
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<DownloadInspectionFeeBillModel> dTResult = downloadRepository.GetDataList(dtParameters, UserName);
            return Json(dTResult);
        }
    }
}
