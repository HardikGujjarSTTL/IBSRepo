using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using IBS.Helper;
using Newtonsoft.Json;
using IBS.Helpers;

namespace IBS.Controllers
{
    public class InspectionEngineersController : BaseController
    {
        #region Variables
        private readonly IInspectionEngineers inspectionEngineers;
        private readonly IDocument iDocument;
        private readonly IWebHostEnvironment env;
        private readonly IConfiguration _config;
        #endregion
        public InspectionEngineersController(IInspectionEngineers _inspectionEngineers, IDocument _iDocumentRepository, IWebHostEnvironment _environment, IConfiguration configuration)
        {
            inspectionEngineers = _inspectionEngineers;
            iDocument = _iDocumentRepository;
            env = _environment;
            _config = configuration;
        }
        public IActionResult Index(int IeCd)
        {
            InspectionEngineersModel model = new();
            model.IeRegion = GetRegionCode;
            if (IeCd > 0)
            {
                model = inspectionEngineers.FindByID(IeCd);
            }

            return View(model);
        }

        public IActionResult Manage(int IeCd, string ActionType)
        {
            InspectionEngineersModel model = new();

            List<IBS_DocumentDTO> lstDocument = iDocument.GetRecordsList((int)Enums.DocumentCategory.IEFullSignature, Convert.ToString(IeCd));
            FileUploaderDTO FileUploaderCOI = new FileUploaderDTO();
            FileUploaderCOI.Mode = (int)Enums.FileUploaderMode.Add_Edit;
            FileUploaderCOI.IBS_DocumentList = lstDocument.Where(m => m.ID == (int)Enums.DocumentCategory_AdminUserUploadDoc.IEFullSignature).ToList();
            FileUploaderCOI.OthersSection = false;
            FileUploaderCOI.MaxUploaderinOthers = 5;
            FileUploaderCOI.FilUploadMode = (int)Enums.FilUploadMode.Single;
            ViewBag.IEFullSignature = FileUploaderCOI;

            List<IBS_DocumentDTO> lstDocumentSignature = iDocument.GetRecordsList((int)Enums.DocumentCategory.IEInitials, Convert.ToString(IeCd));
            FileUploaderDTO FileUploaderCOISignature = new FileUploaderDTO();
            FileUploaderCOISignature.Mode = (int)Enums.FileUploaderMode.Add_Edit;
            FileUploaderCOISignature.IBS_DocumentList = lstDocumentSignature.Where(m => m.ID == (int)Enums.DocumentCategory_AdminUserUploadDoc.IEInitials).ToList();
            FileUploaderCOISignature.OthersSection = false;
            FileUploaderCOISignature.MaxUploaderinOthers = 5;
            FileUploaderCOISignature.FilUploadMode = (int)Enums.FilUploadMode.Single;
            ViewBag.IEInitials = FileUploaderCOISignature;

            model.IeRegion = GetRegionCode;
            if (IeCd > 0)
            {
                model = inspectionEngineers.FindManageByID(IeCd, ActionType, GetRegionCode);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<InspectionEngineersModel> dTResult = inspectionEngineers.GetInspectionEngineersList(dtParameters);
            return Json(dTResult);
        }
        public IActionResult Delete(int id)
        {
            try
            {
                if (inspectionEngineers.Remove(id, UserId))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "InspectionEngineers", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DetailsSave(InspectionEngineersModel model, IFormCollection FrmCollection)
        {
            try
            {
                string msg = "Inserted Successfully.";

                if (model.IeCd > 0)
                {
                    msg = "Your Record Has Been Updated And IE Password Has Been Reset To His Employee No. so, Plz Inform Him!!!";
                    model.Updatedby = UserId;
                    model.UserId = Convert.ToString(UserId);
                }
                model.Createdby = UserId;
                model.UserId = Convert.ToString(UserId);
                string i = inspectionEngineers.DetailsInsertUpdate(model);
                if (i != "Exists")
                {
                    #region File Upload Profile Picture
                    if (!string.IsNullOrEmpty(FrmCollection["hdnUploadedDocumentList_tab-1"]))
                    {

                        int[] DocumentIds = { (int)Enums.DocumentCategory_AdminUserUploadDoc.IEFullSignature };
                        List<APPDocumentDTO> DocumentsList = JsonConvert.DeserializeObject<List<APPDocumentDTO>>(FrmCollection["hdnUploadedDocumentList_tab-1"]);
                        DocumentHelper.SaveFiles(i, DocumentsList, Enums.GetEnumDescription(Enums.FolderPath.IEFullSignature), env, iDocument, "IEFullSignature", string.Empty, DocumentIds);

                        int[] DocumentIds2 = { (int)Enums.DocumentCategory_AdminUserUploadDoc.IEInitials };
                        DocumentHelper.SaveFiles(i, DocumentsList, Enums.GetEnumDescription(Enums.FolderPath.IEInitials), env, iDocument, "IEInitials", string.Empty, DocumentIds2);
                    }
                    #endregion

                    return Json(new { status = true, responseText = msg });
                }
                else if (i == "0")
                {
                    msg = "No Record Found!!! The Record has been  Deleted by other User While you were Modifying the Data";
                    return Json(new { status = false, responseText = msg });
                }
                else
                {
                    msg = "IE with Same Employee Short Name or Employee No. Already Exists!!!";
                    return Json(new { status = false, responseText = msg });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "InspectionEngineers", "DetailsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpGet]
        public IActionResult GetIeCity(int IeCityId)
        {
            try
            {
                List<SelectListItem> agencyClient = Common.GetIeCity(IeCityId);
                return Json(new { status = true, list = agencyClient });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "InspectionEngineers", "GetIeCity", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpGet]
        public IActionResult GetClusterByIE(string IeDepartment)
        {
            try
            {
                List<SelectListItem> ClusterLst = Common.GetClusterByIE(GetRegionCode,IeDepartment);
                return Json(new { status = true, list = ClusterLst });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "InspectionEngineers", "GetIeCity", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpGet]
        public IActionResult GetMatch(int IeCd)
        {
            try
            {
                string MCode = inspectionEngineers.GetMatch(IeCd, GetRegionCode);
                return Json(new { status = true, MCode = MCode });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "InspectionEngineers", "GetMatch", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpPost]
        public IActionResult DeleteIe(int IeCd)
        {
            try
            {
                string i = "";
                string msg = "Delete Successfully.";
                if (IeCd > 0)
                {
                    i = inspectionEngineers.DeleteIe(IeCd);
                }
                if (i != "")
                {
                    return Json(new { status = true, responseText = msg, Id = i });
                }
                else
                {
                    msg = "This Call cannot be deleted. because IC is present for this call!!!";
                    return Json(new { status = false, responseText = msg, Id = i });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "VenderCallRegisterModel", "DetailsDelete", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
    }
}
