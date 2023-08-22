using IBS.DataAccess;
using IBS.Helper;
using IBS.Helpers;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Drawing;

namespace IBS.Controllers
{
    public class MasterItemsListFormController : BaseController
    {
        #region Variables
        private readonly IMasterItemsListForm masterItemsListForm;
        private readonly IDocument iDocument;
        private readonly IWebHostEnvironment env;
        private readonly IConfiguration _config;
        #endregion
        public MasterItemsListFormController(IMasterItemsListForm _masterItemsListForm, IDocument _iDocumentRepository, IWebHostEnvironment _environment, IConfiguration configuration)
        {
            masterItemsListForm = _masterItemsListForm;
            iDocument = _iDocumentRepository;
            env = _environment;
            _config = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Manage(string id)
        {
            List<IBS_DocumentDTO> lstDocument = iDocument.GetRecordsList((int)Enums.DocumentCategory.MasterItemDoc, id);
            FileUploaderDTO FileUploaderCOI = new FileUploaderDTO();
            FileUploaderCOI.Mode = (int)Enums.FileUploaderMode.Add_Edit;
            FileUploaderCOI.IBS_DocumentList = lstDocument.Where(m => m.ID == (int)Enums.DocumentCategory_MasterDoc.MasterItemDoc).ToList();
            FileUploaderCOI.OthersSection = false;
            FileUploaderCOI.MaxUploaderinOthers = 5;
            FileUploaderCOI.FilUploadMode = (int)Enums.FilUploadMode.Single;
            ViewBag.MasterItemDoc = FileUploaderCOI;

            MasterItemsListFormModel model = new();
            if (id != null)
            {
                model = masterItemsListForm.FindByID(id, GetRegionCode);
            }
            else
            {
                model.Region = GetRegionCode;
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<MasterItemsListFormModel> dTResult = masterItemsListForm.GetMasterItemsListFormList(dtParameters);
            return Json(dTResult);
        }

        public IActionResult Delete(String id)
        {
            try
            {
                if (masterItemsListForm.Remove(id, UserId))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "MasterItemsListForm", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MasterItemsListFormDetailsSave(MasterItemsListFormModel model, IFormCollection FrmCollection)
        {
            try
            {
                string msg = "Inserted Successfully...";
                if (model.ItemCd != null)
                {
                    msg = "Updated Successfully...";
                    model.Updatedby = UserId;
                }
                model.Createdby = UserId;
                string i = masterItemsListForm.MasterItemsListFormInsertUpdate(model, GetRegionCode, GetIeCd);
                if (i != "" && i != null)
                {
                    if (!string.IsNullOrEmpty(FrmCollection["hdnUploadedDocumentList_tab-1"]))
                    {
                        string ItemCd = i;
                        int[] DocumentIds = { (int)Enums.DocumentCategory_MasterDoc.MasterItemDoc };
                        List<APPDocumentDTO> DocumentsList = JsonConvert.DeserializeObject<List<APPDocumentDTO>>(FrmCollection["hdnUploadedDocumentList_tab-1"]);
                        DocumentHelper.SaveFiles(ItemCd, DocumentsList, Enums.GetEnumDescription(Enums.FolderPath.MasterItemDoc), env, iDocument, "MasterIDoc", string.Empty, DocumentIds);
                    }
                    return Json(new { status = true, responseText = msg });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "MasterItemsListForm", "MasterItemsListFormDetailsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        public IActionResult GetCOData()
        {
            try
            {
                List<SelectListItem> contacts = Common.GetCOData(GetRegionCode);
                return Json(new { status = true, list = contacts });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "IE Dropdown Bind", "MasterItemsListForm", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        public IActionResult GetIEData()
        {
            try
            {
                List<SelectListItem> contacts = Common.GetIEData(GetRegionCode);
                return Json(new { status = true, list = contacts });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "IE Dropdown Bind", "MasterItemsListForm", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
    }
}
