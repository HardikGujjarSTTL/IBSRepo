using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace IBS.Controllers
{
    public class DownloadDocumentsController : BaseController
    {
        #region Variables
        private readonly IDownloadDocumentsRepository downloaddocument_adminRepository;

        //public int setMessageID { get; set; }
        #endregion
        public DownloadDocumentsController(IDownloadDocumentsRepository _downloaddocument_adminRepository)
        {
            downloaddocument_adminRepository = _downloaddocument_adminRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<DownloadDocumentsModel> dTResult = downloaddocument_adminRepository.GetMessageList(dtParameters);
            return Json(dTResult);
        }

        [HttpGet]
        public IActionResult GetDocSubType(string DocType)
        {
            try
            {
                List<SelectListItem> DocSubType = Common.GetDocSubType(DocType);
                return Json(new { status = true, list = DocSubType });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "DownloadDocuments", "GetDocType", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

    }
}
