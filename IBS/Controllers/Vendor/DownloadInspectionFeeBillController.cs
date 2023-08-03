using IBS.Interfaces.Vendor;
using Microsoft.AspNetCore.Mvc;

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
    }
}
