using IBS.Helper;
using IBS.Helpers;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Newtonsoft.Json;

namespace IBS.Controllers
{
    public class FileUploadController : BaseController
    {
        private readonly IDocument iDocument;
        private readonly IWebHostEnvironment env;
        private readonly IConfiguration _config;
        public FileUploadController(IDocument _iDocumentRepository, IWebHostEnvironment _environment, IConfiguration configuration)
        {
            iDocument = _iDocumentRepository;
            env = _environment;
            _config = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Manage(int id)
        {
            //List<IBS_DocumentDTO> lstDocument = iDocument.GetRecordsList((int)Enums.DocumentCategory.UserRegi, new int());
            List<IBS_DocumentDTO> lstDocument = iDocument.GetRecordsList((int)Enums.DocumentCategory.UserRegi, id == 0 ? new int() : (int)id);
            FileUploaderDTO FileUploaderCOI = new FileUploaderDTO();
            FileUploaderCOI.Mode = (int)Enums.FileUploaderMode.Add_Edit;
            FileUploaderCOI.IBS_DocumentList = lstDocument.Where(m => m.ID == (int)Enums.DocumentCategory_CANRegisrtation.Address_Proof_Document).ToList();
            FileUploaderCOI.OthersSection = false;
            FileUploaderCOI.MaxUploaderinOthers = 5;
            FileUploaderCOI.FilUploadMode = (int)Enums.FilUploadMode.Single;
            ViewBag.objuserDTO = FileUploaderCOI;

            List<IBS_DocumentDTO> lstDocumentProfile_Picture = iDocument.GetRecordsList((int)Enums.DocumentCategory.UserRegi, id == 0 ? new int() : (int)id);
            FileUploaderDTO FileUploaderCOIProfile_Picture = new FileUploaderDTO();
            FileUploaderCOIProfile_Picture.Mode = (int)Enums.FileUploaderMode.Add_Edit;
            FileUploaderCOIProfile_Picture.IBS_DocumentList = lstDocumentProfile_Picture.Where(m => m.ID == (int)Enums.DocumentCategory_CANRegisrtation.Profile_Picture).ToList();
            FileUploaderCOIProfile_Picture.OthersSection = false;
            FileUploaderCOIProfile_Picture.MaxUploaderinOthers = 5;
            FileUploaderCOIProfile_Picture.FilUploadMode = (int)Enums.FilUploadMode.Single;
            ViewBag.objProfile_Picture = FileUploaderCOIProfile_Picture;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult FileUploadSave(IFormCollection FrmCollection)
        {
            try
            {
                int id = 0;
                #region File Upload Profile Picture
                if (!string.IsNullOrEmpty(FrmCollection["hdnUploadedDocumentList_tab-1"]))
                {
                    int ResultId = 10;
                    int[] DocumentIds = { (int)Enums.DocumentCategory_CANRegisrtation.Address_Proof_Document, (int)Enums.DocumentCategory_CANRegisrtation.Profile_Picture };
                    List<APPDocumentDTO> DocumentsList = JsonConvert.DeserializeObject<List<APPDocumentDTO>>(FrmCollection["hdnUploadedDocumentList_tab-1"]);
                    id = DocumentHelper.SaveFiles(Convert.ToInt32(ResultId), DocumentsList, Enums.GetEnumDescription(Enums.FolderPath.UserRegistration), env, iDocument, "USER", string.Empty, DocumentIds);

                    //foreach (var lstEducationDetails in DocumentsList)
                    //{
                    //    List<APPDocumentDTO> list = new List<APPDocumentDTO>();
                    //    list.Add(lstEducationDetails);
                    //    id = DocumentHelper.SaveFiles(Convert.ToInt32(ResultId), list, Enums.GetEnumDescription(Enums.FolderPath.TempFilePath), env, iDocument, "USER", string.Empty, lstEducationDetails.Documentid);
                    //    id = DocumentHelper.SaveFiles(Convert.ToInt32(ResultId), list, Enums.GetEnumDescription(Enums.FolderPath.TempFilePath), prefixName);
                    //}
                }
                #endregion
                if (id > 0)
                {
                    return Json(new { status = true, responseText = "Done" });
                }
                return Json(new { status = false, responseText = "Error" });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "FileUpload", "FileUploadSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
    }
}
