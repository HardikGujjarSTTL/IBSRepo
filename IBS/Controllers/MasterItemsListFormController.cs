using IBS.Helper;
using IBS.Helpers;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

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
                model = masterItemsListForm.FindByID(id, Region);
            }
            else
            {
                model.Region = Region;
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
        public IActionResult Manage(MasterItemsListFormModel model, IFormCollection FrmCollection)
        {
            try
            {
                model.Createdby = UserId;
                model.Updatedby = UserId;

                string i = "";
                if (model.ItemCd == null)
                {
                    i = masterItemsListForm.DtlInsertUpdate(model, Region, GetIeCd);
                    AlertAddSuccess("Record Added Successfully.");
                }
                else
                {
                    i = masterItemsListForm.DtlInsertUpdate(model, Region, GetIeCd);
                    AlertAddSuccess("Record Update Successfully.");
                }
                if (i != "" && i != null)
                {
                    if (!string.IsNullOrEmpty(FrmCollection["hdnUploadedDocumentList_tab-1"]))
                    {
                        string ItemCd = i;
                        int[] DocumentIds = { (int)Enums.DocumentCategory_MasterDoc.MasterItemDoc };
                        List<APPDocumentDTO> DocumentsList = JsonConvert.DeserializeObject<List<APPDocumentDTO>>(FrmCollection["hdnUploadedDocumentList_tab-1"]);
                        DocumentHelper.SaveFiles(ItemCd, DocumentsList, Enums.GetEnumDescription(Enums.FolderPath.MasterItemDoc), env, iDocument, "MasterIDoc", ItemCd, DocumentIds);
                    }
                    //return Json(new { status = true, responseText = msg });
                }
                //return RedirectToAction("Index?actiontype=" + model.actiontype);
                return RedirectToAction("Index", new { actiontype = model.actiontype });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "MasterItemsListForm", "Manage", 1, GetIPAddress());
            }
            return View(model);
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
