using IBS.Helper;
using IBS.Interfaces;
using IBS.Interfaces.Administration;
using IBS.Models;
using IBS.Repositories.Administration;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers.Administration
{
    public class AdminUserUploadDocController : BaseController
    {
        #region Variables
        private readonly IUploadDocRepository uploaddocRepository;
        private readonly IDocument iDocument;
        private readonly IWebHostEnvironment env;
        private readonly IConfiguration _config;

        #endregion
        public AdminUserUploadDocController(IUploadDocRepository _uploaddocRepository, IDocument _iDocumentRepository, IWebHostEnvironment _environment, IConfiguration configuration)
        {
            uploaddocRepository = _uploaddocRepository;
            iDocument = _iDocumentRepository;
            env = _environment;
            _config = configuration;
        }

       
        public IActionResult UploadDoc(string id)
        {
            //List<IBS_DocumentDTO> lstDocument = iDocument.GetRecordsList((int)Enums.DocumentCategory.UserRegi, id == null ? new string() : id);
            //FileUploaderDTO FileUploaderCOI = new FileUploaderDTO();
            //FileUploaderCOI.Mode = (int)Enums.FileUploaderMode.Add_Edit;
            //FileUploaderCOI.IBS_DocumentList = lstDocument.Where(m => m.ID == (int)Enums.DocumentCategory_CANRegisrtation.Address_Proof_Document).ToList();
            //FileUploaderCOI.OthersSection = false;
            //FileUploaderCOI.MaxUploaderinOthers = 5;
            //FileUploaderCOI.FilUploadMode = (int)Enums.FilUploadMode.Single;
            //ViewBag.objuserDTO = FileUploaderCOI;

            return View();
        }
    }
}
