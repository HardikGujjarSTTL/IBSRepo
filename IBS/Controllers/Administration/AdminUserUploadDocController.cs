using IBS.Filters;
using IBS.Helper;
using IBS.Helpers;
using IBS.Interfaces;
using IBS.Interfaces.Administration;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IBS.Controllers.Administration
{
    [Authorization]
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

        [Authorization("AdminUserUploadDoc", "UploadDoc", "view")]
        public IActionResult UploadDoc(string id)
        {
            List<IBS_DocumentDTO> lstDocument = iDocument.GetRecordsList((int)Enums.DocumentCategory.AdminUserUploadDoc, id);
            FileUploaderDTO FileUploaderCOI = new FileUploaderDTO();
            FileUploaderCOI.Mode = (int)Enums.FileUploaderMode.Add_Edit;
            FileUploaderCOI.IBS_DocumentList = lstDocument.Where(m => m.ID == (int)Enums.DocumentCategory_AdminUserUploadDoc.Browse_the_Document_to_Upload).ToList();
            FileUploaderCOI.OthersSection = false;
            FileUploaderCOI.MaxUploaderinOthers = 5;
            FileUploaderCOI.FilUploadMode = (int)Enums.FilUploadMode.Single;
            ViewBag.AdminUserUploadDoc = FileUploaderCOI;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorization("AdminUserUploadDoc", "UploadDoc", "edit")]
        public IActionResult DetailsSave(UploadDocModel model, IFormCollection FrmCollection)
        {
            try
            {
                string msg = "Inserted Successfully.";

                if (model.FileId != null)
                {
                    msg = "Updated Successfully.";
                }

                string i = uploaddocRepository.DetailsUpdate(model);
                if (i != "")
                {
                    #region File Upload Profile Picture
                    if (!string.IsNullOrEmpty(FrmCollection["hdnUploadedDocumentList_tab-1"]))
                    {

                        int[] DocumentIds = { (int)Enums.DocumentCategory_AdminUserUploadDoc.Browse_the_Document_to_Upload };
                        List<APPDocumentDTO> DocumentsList = JsonConvert.DeserializeObject<List<APPDocumentDTO>>(FrmCollection["hdnUploadedDocumentList_tab-1"]);
                        DocumentHelper.SaveFiles(i, DocumentsList, Enums.GetEnumDescription(Enums.FolderPath.AdminUserUploadDoc), env, iDocument, "AdminUserUploadDoc", string.Empty, DocumentIds);

                    }
                    #endregion

                    return Json(new { status = true, responseText = msg, Id = i });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "AdminUserUploadDoc", "DetailsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
    }
}
