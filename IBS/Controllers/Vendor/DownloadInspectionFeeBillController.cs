using IBS.Interfaces.Vendor;
using IBS.Models;
using IBS.Repositories.Vendor;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;

namespace IBS.Controllers.Vendor
{
    public class DownloadInspectionFeeBillController : BaseController
    {
        #region Variables
        private readonly IDownloadInspFeeBillRepository downloadRepository;
        #endregion
        public DownloadInspectionFeeBillController(IDownloadInspFeeBillRepository _downloadRepository)
        {
            downloadRepository = _downloadRepository;
        }

        public IActionResult Index(string CaseNo, string BkNo, string SetNo)
        {
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
