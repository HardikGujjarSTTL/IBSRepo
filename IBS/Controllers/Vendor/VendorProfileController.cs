using IBS.Filters;
using IBS.Helper;
using IBS.Helpers;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IBS.Controllers.Vendor
{
    [Authorization]
    public class VendorProfileController : BaseController
    {
        #region Variables
        private readonly IVendorProfileRepository vendorProfileRepository;
        private readonly IDocument iDocument;
        private readonly IWebHostEnvironment env;
        private readonly IConfiguration _config;
        #endregion
        public VendorProfileController(IVendorProfileRepository _vendorProfileRepository, IDocument _iDocumentRepository, IWebHostEnvironment _environment, IConfiguration configuration)
        {
            vendorProfileRepository = _vendorProfileRepository;
            iDocument = _iDocumentRepository;
            env = _environment;
            _config = configuration;
        }
        [Authorization("VendorProfile", "Index", "view")]
        public IActionResult Index()
        {
            return View();
        }
        [Authorization("VendorProfile", "Index", "view")]
        public IActionResult Manage(int VEND_CD)
        {
            int VendCd = 0;
            if (IBS.Helper.SessionHelper.UserModelDTO.RoleName.ToLower() == "vendor")
            {
                VendCd = Convert.ToInt32(IBS.Helper.SessionHelper.UserModelDTO.UserName);
            }
            if(VEND_CD > 0)
            {
                VendCd = VEND_CD;
            }
            VendorModel model = new();
            if (VendCd > 0)
            {
                model = vendorProfileRepository.FindByID(VendCd);
            }
            List<IBS_DocumentDTO> lstDocument = iDocument.GetRecordsList((int)Enums.DocumentCategory.Vendor, Convert.ToString(VendCd));
            FileUploaderDTO FileUploaderCOI = new FileUploaderDTO();
            if ((VendCd == model.VendCd && IBS.Helper.SessionHelper.UserModelDTO.RoleName.ToLower() == "vendor"))
            {
                FileUploaderCOI.Mode = (int)Enums.FileUploaderMode.View;
            }
            else
            {
                FileUploaderCOI.Mode = (int)Enums.FileUploaderMode.Add_Edit;
            }
            FileUploaderCOI.IBS_DocumentList = lstDocument.Where(m => m.ID == (int)Enums.DocumentCategory_CANRegisrtation.Document_Vendor_manufacturer_created).ToList();
            FileUploaderCOI.OthersSection = false;
            FileUploaderCOI.MaxUploaderinOthers = 5;
            FileUploaderCOI.FilUploadMode = (int)Enums.FilUploadMode.Single;
            ViewBag.Vendor_Manufacturer = FileUploaderCOI;
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<VendorlistModel> dTResult = vendorProfileRepository.GetVendorList(dtParameters);
            return Json(dTResult);
        }
        [Authorization("VendorProfile", "Index", "delete")]
        public IActionResult Delete(int VEND_CD)
        {
            try
            {
                if (vendorProfileRepository.Remove(VEND_CD, UserId))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "VendorProfile", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorization("VendorProfile", "Index", "edit")]
        public IActionResult VendorProfileDetailsSave(VendorModel model, IFormCollection FrmCollection)
        {
            try
            {
                string msg = "";
                int VendCd = 0;
                string userType = IBS.Helper.SessionHelper.UserModelDTO.RoleName.ToLower();
                if (IBS.Helper.SessionHelper.UserModelDTO.RoleName.ToLower() == "vendor")
                {
                    VendCd = Convert.ToInt32(IBS.Helper.SessionHelper.UserModelDTO.UserName);
                }
                if (model.VendCd > 0)
                {
                    msg = "Vendor Updated Successfully.";
                }
                int id = vendorProfileRepository.VendorDetailsInsertUpdate(model, (VendCd == model.VendCd && userType == "vendor") ? true :false);
                if (id > 0)
                {
                    #region File Upload Profile Picture
                    if (!string.IsNullOrEmpty(FrmCollection["hdnUploadedDocumentList_tab-1"]))
                    {
                        int[] DocumentIds = { (int)Enums.DocumentCategory_CANRegisrtation.Address_Proof_Document, (int)Enums.DocumentCategory_CANRegisrtation.Document_Vendor_manufacturer_created };
                        List<APPDocumentDTO> DocumentsList = JsonConvert.DeserializeObject<List<APPDocumentDTO>>(FrmCollection["hdnUploadedDocumentList_tab-1"]);
                        DocumentHelper.SaveFiles(Convert.ToString(id), DocumentsList, Enums.GetEnumDescription(Enums.FolderPath.Vendor), env, iDocument, "USER", string.Empty, DocumentIds);

                    }
                    #endregion
                    return Json(new { status = true, responseText = msg });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "VendorProfile", "VendorProfileDetailsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
    }
}
