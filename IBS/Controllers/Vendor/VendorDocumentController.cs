using IBS.Helper;
using IBS.Helpers;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IBS.Controllers.Vendor
{
    public class VendorDocumentController : BaseController
    {
        private readonly IDocument iDocument;
        private readonly IWebHostEnvironment env;
        private readonly IConfiguration _config;
        private readonly IVendorDocumentRepository pVendorDocumentRepository;
        public VendorDocumentController(IDocument _iDocumentRepository, IWebHostEnvironment _environment, IConfiguration configuration, IVendorDocumentRepository _pVendorDocumentRepository)
        {
            iDocument = _iDocumentRepository;
            env = _environment;
            _config = configuration;
            pVendorDocumentRepository = _pVendorDocumentRepository;
        }
        public IActionResult Index(string id)
        {
            VendEquipClbrCertModel model = new();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveVendorDocument(VendEquipClbrCertModel model, IFormCollection FrmCollection)
        {
            try
            {
                int VendCd = Convert.ToInt32(IBS.Helper.SessionHelper.UserModelDTO.UserName);
                string msg = "Vendor Document Inserted Successfully.";
                if (model.EquipClbrCertSno > 0)
                {
                    msg = "Vendor Document Updated Successfully.";
                }
                model.VendCd = VendCd;
                int id = pVendorDocumentRepository.VendorDocumentInsertUpdate(model);
                if (id > 0)
                {
                    #region File Upload Profile Picture
                    if (!string.IsNullOrEmpty(FrmCollection["hdnUploadedDocumentList_tab-1"]))
                    {
                        int[] DocumentIds = { (int)Enums.DocumentCategory_CANRegisrtation.Address_Proof_Document, (int)Enums.DocumentCategory_CANRegisrtation.Inernal_Records };
                        List<APPDocumentDTO> DocumentsList = JsonConvert.DeserializeObject<List<APPDocumentDTO>>(FrmCollection["hdnUploadedDocumentList_tab-1"]);
                        id = DocumentHelper.SaveFiles(Convert.ToString(id), DocumentsList, Enums.GetEnumDescription(Enums.FolderPath.VendorDocument), env, iDocument, "VDInernal_Records", string.Empty, DocumentIds);
                    }
                }
                #endregion
                if (id > 0)
                {
                    return Json(new { status = true, responseText = msg });
                }
                return Json(new { status = false, responseText = "Error" });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "VendorDocument", "SaveVendorDocument", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpPost]
        public IActionResult GetDocument(string DocType)
        {
            try
            {
                string id = "0";
                int VendCd = Convert.ToInt32(IBS.Helper.SessionHelper.UserModelDTO.UserName);
                VendEquipClbrCertModel model = new();
                if (DocType != null)
                {
                    model = pVendorDocumentRepository.FindByID(VendCd, DocType);
                }
                if (model != null)
                {
                    id = Convert.ToString(model.ID);
                }
                else
                {
                    model = new VendEquipClbrCertModel();
                }
                model.DocType = DocType;

                if (DocType == "I")
                {
                    List<IBS_DocumentDTO> lstDocument = iDocument.GetRecordsList((int)Enums.DocumentCategory.VendorDocument, id);
                    FileUploaderDTO FileUploaderCOI = new FileUploaderDTO();
                    FileUploaderCOI.Mode = (int)Enums.FileUploaderMode.Add_Edit;
                    FileUploaderCOI.IBS_DocumentList = lstDocument.Where(m => m.ID == (int)Enums.DocumentCategory_CANRegisrtation.Inernal_Records).ToList();
                    FileUploaderCOI.OthersSection = false;
                    FileUploaderCOI.MaxUploaderinOthers = 5;
                    FileUploaderCOI.FilUploadMode = (int)Enums.FilUploadMode.Single;
                    ViewBag.Inernal_Records = FileUploaderCOI;
                }
                else if (DocType == "F")
                {
                    List<IBS_DocumentDTO> lstFirmDocument = iDocument.GetRecordsList((int)Enums.DocumentCategory.VendorDocument, id);
                    FileUploaderDTO FileUploaderFirm = new FileUploaderDTO();
                    FileUploaderFirm.Mode = (int)Enums.FileUploaderMode.Add_Edit;
                    FileUploaderFirm.IBS_DocumentList = lstFirmDocument.Where(m => m.ID == (int)Enums.DocumentCategory_CANRegisrtation.Firm_Certificate_Like_RDSO_Approval_Type_test_etc).ToList();
                    FileUploaderFirm.OthersSection = false;
                    FileUploaderFirm.MaxUploaderinOthers = 5;
                    FileUploaderFirm.FilUploadMode = (int)Enums.FilUploadMode.Single;
                    ViewBag.Firm_Certificate = FileUploaderFirm;
                }
                else if (DocType == "R")
                {
                    List<IBS_DocumentDTO> lstRaw_MaterialDocument = iDocument.GetRecordsList((int)Enums.DocumentCategory.VendorDocument, id);
                    FileUploaderDTO FileUploaderRaw_Material = new FileUploaderDTO();
                    FileUploaderRaw_Material.Mode = (int)Enums.FileUploaderMode.Add_Edit;
                    FileUploaderRaw_Material.IBS_DocumentList = lstRaw_MaterialDocument.Where(m => m.ID == (int)Enums.DocumentCategory_CANRegisrtation.Raw_Material_Invoice).ToList();
                    FileUploaderRaw_Material.OthersSection = false;
                    FileUploaderRaw_Material.MaxUploaderinOthers = 5;
                    FileUploaderRaw_Material.FilUploadMode = (int)Enums.FilUploadMode.Single;
                    ViewBag.Raw_Material = FileUploaderRaw_Material;
                }
                else if (DocType == "C")
                {
                    List<IBS_DocumentDTO> lstCalibrationDocument = iDocument.GetRecordsList((int)Enums.DocumentCategory.VendorDocumentForCalibrationRecord, id);
                    FileUploaderDTO FileUploaderCalibration = new FileUploaderDTO();
                    FileUploaderCalibration.Mode = (int)Enums.FileUploaderMode.Add_Edit;
                    FileUploaderCalibration.IBS_DocumentList = lstCalibrationDocument.Where(m => m.ID == (int)Enums.DocumentCategory_CANRegisrtation.Calibration_Records).ToList();
                    FileUploaderCalibration.OthersSection = false;
                    FileUploaderCalibration.MaxUploaderinOthers = 5;
                    FileUploaderCalibration.FilUploadMode = (int)Enums.FilUploadMode.Single;
                    ViewBag.Calibration_Records = FileUploaderCalibration;
                }
                if (DocType == "C")
                {
                    return PartialView("_CalibrationRecordsVendDocument", model);
                }
                else
                {
                    return PartialView("_VendDocument", model);
                }

            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "VendorDocument", "GetDocument", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
    }
}
