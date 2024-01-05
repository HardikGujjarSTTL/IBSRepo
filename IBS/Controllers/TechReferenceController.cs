using IBS.Filters;
using IBS.Helper;
using IBS.Helpers;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IBS.Controllers
{
    [Authorization]
    public class TechReferenceController : BaseController
    {
        #region Variables
        private readonly IDocument iDocument;
        private readonly IWebHostEnvironment env;
        private readonly ITechReferenceRepository techReferenceRepository;
        #endregion
        public TechReferenceController(IDocument _iDocumentRepository, IWebHostEnvironment _environment, ITechReferenceRepository _techReferenceRepository)
        {
            iDocument = _iDocumentRepository;
            env = _environment;
            techReferenceRepository = _techReferenceRepository;
        }
        [Authorization("TechReference", "Index", "view")]
        public IActionResult Index()
        {
            return View();
        }
        [Authorization("TechReference", "Index", "view")]
        public IActionResult Manage(int ID)
        {
            TechReferenceModel model = new();
            string Region = IBS.Helper.SessionHelper.UserModelDTO.Region;
            if (ID > 0)
            {
                model = techReferenceRepository.FindByID(ID);
            }
            List<IBS_DocumentDTO> lstDocumentUpload_Tech_Ref = iDocument.GetRecordsList((int)Enums.DocumentCategory.TechnicalReferences, Convert.ToString(ID));
            FileUploaderDTO FileUploaderUpload_Tech_Ref = new FileUploaderDTO();
            FileUploaderUpload_Tech_Ref.Mode = (int)Enums.FileUploaderMode.Add_Edit;
            FileUploaderUpload_Tech_Ref.IBS_DocumentList = lstDocumentUpload_Tech_Ref.Where(m => m.ID == (int)Enums.DocumentCategory_CANRegisrtation.Upload_Tech_Ref).ToList();
            FileUploaderUpload_Tech_Ref.OthersSection = false;
            FileUploaderUpload_Tech_Ref.MaxUploaderinOthers = 5;
            FileUploaderUpload_Tech_Ref.FilUploadMode = (int)Enums.FilUploadMode.Single;
            ViewBag.Upload_Tech_Ref = FileUploaderUpload_Tech_Ref;

            List<IBS_DocumentDTO> lstDocumenFileUploaderUpload_Tech_Ref_Replyt = iDocument.GetRecordsList((int)Enums.DocumentCategory.TechnicalReferences, Convert.ToString(ID));
            FileUploaderDTO FileUploaderUpload_Tech_Ref_Reply = new FileUploaderDTO();
            FileUploaderUpload_Tech_Ref_Reply.Mode = (int)Enums.FileUploaderMode.Add_Edit;
            FileUploaderUpload_Tech_Ref_Reply.IBS_DocumentList = lstDocumenFileUploaderUpload_Tech_Ref_Replyt.Where(m => m.ID == (int)Enums.DocumentCategory_CANRegisrtation.Upload_Tech_Ref_Reply).ToList();
            FileUploaderUpload_Tech_Ref_Reply.OthersSection = false;
            FileUploaderUpload_Tech_Ref_Reply.MaxUploaderinOthers = 5;
            FileUploaderUpload_Tech_Ref_Reply.FilUploadMode = (int)Enums.FilUploadMode.Single;
            ViewBag.FileUploaderUpload_Tech_Ref_Reply = FileUploaderUpload_Tech_Ref_Reply;
            model.RegionCd = Region;
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<TechReferenceModel> dTResult = techReferenceRepository.GetTechReferenceList(dtParameters, GetRegionCode);
            return Json(dTResult);
        }
        [Authorization("TechReference", "Index", "delete")]
        public IActionResult Delete(int ID)
        {
            try
            {
                if (techReferenceRepository.Remove(ID, UserId))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Contract", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorization("TechReference", "Index", "edit")]
        public IActionResult TechRefDetailsSave(TechReferenceModel model, IFormCollection FrmCollection)
        {
            try
            {
                string Region = IBS.Helper.SessionHelper.UserModelDTO.Region;
                string msg = "TechReference Inserted Successfully.";
                if (model.TechId != null)
                {
                    msg = "TechReference Updated Successfully.";
                    model.Updatedby = UserId;
                }
                model.Createdby = UserId;
                model.RegionCd = Region;
                int id = techReferenceRepository.TechRefDetailsInsertUpdate(model, GetRegionCode);
                if (id > 0)
                {
                    if (!string.IsNullOrEmpty(FrmCollection["hdnUploadedDocumentList_tab-1"]))
                    {
                        string fileName = id.ToString() + "_R";
                        int[] DocumentIds = { (int)Enums.DocumentCategory_CANRegisrtation.Upload_Tech_Ref, (int)Enums.DocumentCategory_CANRegisrtation.Upload_Tech_Ref_Reply };
                        List<APPDocumentDTO> DocumentsList = JsonConvert.DeserializeObject<List<APPDocumentDTO>>(FrmCollection["hdnUploadedDocumentList_tab-1"]);
                        DocumentHelper.SaveFiles(Convert.ToString(id), DocumentsList, Enums.GetEnumDescription(Enums.FolderPath.Tech), env, iDocument, "TechnicalRef", fileName, DocumentIds);
                    }
                    return Json(new { status = true, responseText = msg });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "TechReference", "TechReferenceDetailsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

    }
}
