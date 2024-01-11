using IBS.Filters;
using IBS.Helper;
using IBS.Helpers;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IBS.Controllers.Vendor
{
    [Authorization]
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
        [Authorization("VendorDocument", "Index", "view")]
        public IActionResult Index(string id)
        {
            VendEquipClbrCertModel model = new();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorization("VendorDocument", "Index", "view")]
        public IActionResult SaveVendorDocument(VendEquipClbrCertModel model, IFormCollection FrmCollection)
        {
            try
            {
                int srNo = 0;
                int VendCd = Convert.ToInt32(IBS.Helper.SessionHelper.UserModelDTO.UserName);
                string msg = "Vendor Document Inserted Successfully.";
                if (model.EquipClbrCertSno > 0)
                {
                    msg = "Vendor Document Updated Successfully.";
                }
                model.VendCd = VendCd;
                if (model.DocType == "C")
                {
                    srNo = pVendorDocumentRepository.VendorCalibrationRecordsInsertUpdate(model);
                }
                int id = pVendorDocumentRepository.VendorDocumentInsertUpdate(model);
                if (id > 0)
                {
                    #region File Upload Profile Picture
                    string fileName= VendCd + "_" + model.DocType;
                    if (!string.IsNullOrEmpty(FrmCollection["hdnUploadedDocumentList_tab-1"]))
                    {
                        int[] DocumentIds = { (int)Enums.DocumentCategory_CANRegisrtation.Inernal_Records, (int)Enums.DocumentCategory_CANRegisrtation.Firm_Certificate_Like_RDSO_Approval_Type_test_etc, (int)Enums.DocumentCategory_CANRegisrtation.Raw_Material_Invoice, (int)Enums.DocumentCategory_CANRegisrtation.Calibration_Records };
                        List<APPDocumentDTO> DocumentsList = JsonConvert.DeserializeObject<List<APPDocumentDTO>>(FrmCollection["hdnUploadedDocumentList_tab-1"]);
                        id = DocumentHelper.SaveFiles(Convert.ToString(VendCd), DocumentsList, Enums.GetEnumDescription(Enums.FolderPath.VendorDocument), env, iDocument, "VDInernal_Records", fileName, DocumentIds);
                    }
                }
                #endregion
                if ((model.DocType == "C" && srNo > 0) || id > 0)
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
                int maxSrNo = 0;
                int VendCd = Convert.ToInt32(IBS.Helper.SessionHelper.UserModelDTO.UserName);
                VendEquipClbrCertModel model = new();
                if (DocType != null)
                {
                    model = pVendorDocumentRepository.FindByID(VendCd, DocType);
                    maxSrNo = pVendorDocumentRepository.GetmaxSrNo(VendCd, DocType);
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
                    List<IBS_DocumentDTO> lstCalibrationDocument = iDocument.GetRecordsList((int)Enums.DocumentCategory.VendorDocument, id);
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
                    model.EquipClbrCertSno = Convert.ToByte(maxSrNo);
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

        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            int VendCd = Convert.ToInt32(IBS.Helper.SessionHelper.UserModelDTO.UserName);
            DTResult<VendEquipClbrCertListModel> dTResult = pVendorDocumentRepository.GetVendorCalibrationRecordssList(dtParameters, VendCd);
            return Json(dTResult);
        }

        [HttpPost]
        public IActionResult EditVendorCalibration(int VendCd, string DocType, string EquipMkSl, string CalibCertNo, int EquipClbrCertSno)
        {
            try
            {
                string id = "0";
                VendEquipClbrCertModel modelDoc = new();
                VendEquipClbrCertModel model = new();
                if (VendCd > 0 && DocType != null)
                {
                    modelDoc = pVendorDocumentRepository.FindByID(VendCd, DocType);
                    if (modelDoc != null)
                    {
                        id = Convert.ToString(modelDoc.ID);
                    }

                    model = pVendorDocumentRepository.FindVendorCalibrationByID(VendCd, DocType, EquipMkSl, CalibCertNo, EquipClbrCertSno);
                }

                if (DocType == "C")
                {
                    List<IBS_DocumentDTO> lstCalibrationDocument = iDocument.GetRecordsList((int)Enums.DocumentCategory.VendorDocument, id);
                    FileUploaderDTO FileUploaderCalibration = new FileUploaderDTO();
                    FileUploaderCalibration.Mode = (int)Enums.FileUploaderMode.Add_Edit;
                    FileUploaderCalibration.IBS_DocumentList = lstCalibrationDocument.Where(m => m.ID == (int)Enums.DocumentCategory_CANRegisrtation.Calibration_Records).ToList();
                    FileUploaderCalibration.OthersSection = false;
                    FileUploaderCalibration.MaxUploaderinOthers = 5;
                    FileUploaderCalibration.FilUploadMode = (int)Enums.FilUploadMode.Single;
                    ViewBag.Calibration_Records = FileUploaderCalibration;
                }
                return PartialView("_CalibrationRecordsVendDocument", model);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "VendorDocument", "GetDocument", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
    }
}
