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

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<DownloadInspectionFeeBillModel> dTResult = downloadRepository.GetDataList(dtParameters, UserName);
            return Json(dTResult);
        }
    }
}
