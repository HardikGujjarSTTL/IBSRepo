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
        private readonly IDocument iDocument;
        private readonly IWebHostEnvironment env;
        private readonly IConfiguration _config;

        //public int setMessageID { get; set; }
        #endregion
        public DownloadDocumentsController(IDownloadDocumentsRepository _downloaddocument_adminRepository, IDocument _iDocumentRepository, IWebHostEnvironment _environment, IConfiguration configuration)
        {
            downloaddocument_adminRepository = _downloaddocument_adminRepository;
            iDocument = _iDocumentRepository;
            env = _environment;
            _config = configuration;
        }

        public IActionResult Index()
        {
            var model = new DownloadDocumentsModel();
            model.DocType = "R";


            

            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<DownloadDocumentsModel> dTResult = downloaddocument_adminRepository.GetMessageList(dtParameters);
            //int i=0;
            //if (dTResult.data.Count() > 0)
            //{
            //    for( i = 1; i<= dTResult.data.Count(); i++)
            //    {
            //        List<IBS_DocumentDTO> lstDocument = iDocument.GetRecordsList((int)Enums.DocumentCategory.AdminUserUploadDoc, dTResult.data.);
            //        FileUploaderDTO FileUploaderCOI = new FileUploaderDTO();
            //        FileUploaderCOI.Mode = (int)Enums.FileUploaderMode.Add_Edit;
            //        FileUploaderCOI.IBS_DocumentList = lstDocument.Where(m => m.ID == (int)Enums.DocumentCategory_AdminUserUploadDoc.Browse_the_Document_to_Upload).ToList();
            //        FileUploaderCOI.OthersSection = false;
            //        FileUploaderCOI.MaxUploaderinOthers = 5;
            //        FileUploaderCOI.FilUploadMode = (int)Enums.FilUploadMode.Single;
            //        ViewBag.AdminUserUploadDoc = FileUploaderCOI;
            //    }
            //}
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
