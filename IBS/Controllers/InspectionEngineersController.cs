using IBS.Helper;
using IBS.Helpers;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace IBS.Controllers
{
    public class InspectionEngineersController : BaseController
    {
        private readonly IInspectionEngineers inspectionEngineers;
        private readonly IDocument iDocument;
        private readonly IWebHostEnvironment env;
        private readonly IConfiguration _config;
        SessionHelper objSessionHelper = new SessionHelper();

        public InspectionEngineersController(IInspectionEngineers _inspectionEngineers, IDocument _iDocumentRepository, IWebHostEnvironment _environment, IConfiguration configuration)
        {
            inspectionEngineers = _inspectionEngineers;
            iDocument = _iDocumentRepository;
            env = _environment;
            _config = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Manage(int Id)
        {
            InspectionEngineersModel model = new() { IeRegion = Region };

            if (Id > 0)
            {
                model = inspectionEngineers.FindManageByID(Id);
                objSessionHelper.lstInspectionEClusterModel = model.lstInspectionEClusterModel;
                model.ID = Id;
            }
            else
            {
                model.ID = 0;
                objSessionHelper.lstInspectionEClusterModel = null;
            }

            List<IBS_DocumentDTO> lstDocument = iDocument.GetRecordsList((int)Enums.DocumentCategory.IEFullSignature, Convert.ToString(Id));
            FileUploaderDTO FileUploaderCOI = new FileUploaderDTO();
            FileUploaderCOI.Mode = (int)Enums.FileUploaderMode.Add_Edit;
            FileUploaderCOI.IBS_DocumentList = lstDocument.Where(m => m.ID == (int)Enums.DocumentCategory_AdminUserUploadDoc.IEFullSignature).ToList();
            FileUploaderCOI.OthersSection = false;
            FileUploaderCOI.MaxUploaderinOthers = 5;
            FileUploaderCOI.FilUploadMode = (int)Enums.FilUploadMode.Single;
            ViewBag.IEFullSignature = FileUploaderCOI;

            List<IBS_DocumentDTO> lstDocumentSignature = iDocument.GetRecordsList((int)Enums.DocumentCategory.IEInitials, Convert.ToString(Id));
            FileUploaderDTO FileUploaderCOISignature = new FileUploaderDTO();
            FileUploaderCOISignature.Mode = (int)Enums.FileUploaderMode.Add_Edit;
            FileUploaderCOISignature.IBS_DocumentList = lstDocumentSignature.Where(m => m.ID == (int)Enums.DocumentCategory_AdminUserUploadDoc.IEInitials).ToList();
            FileUploaderCOISignature.OthersSection = false;
            FileUploaderCOISignature.MaxUploaderinOthers = 5;
            FileUploaderCOISignature.FilUploadMode = (int)Enums.FilUploadMode.Single;
            ViewBag.IEInitials = FileUploaderCOISignature;

            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<InspectionEngineersModel> dTResult = inspectionEngineers.GetInspectionEngineersList(dtParameters, Region);
            return Json(dTResult);
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
                if (objSessionHelper.lstInspectionEClusterModel != null)
                {
                    model.lstInspectionEClusterModel = objSessionHelper.lstInspectionEClusterModel;
                }

                model.Createdby = UserId;
                model.UserId = Convert.ToString(UserId);
                model.IeRegion = Region;
                string i = inspectionEngineers.DetailsInsertUpdate(model);
                if (i != "Exists")
                {
                    #region File Upload Profile Picture
                    if (!string.IsNullOrEmpty(FrmCollection["hdnUploadedDocumentList_tab-1"]))
                    {

                        int[] DocumentIds = { (int)Enums.DocumentCategory_AdminUserUploadDoc.IEFullSignature };
                        List<APPDocumentDTO> DocumentsList = JsonConvert.DeserializeObject<List<APPDocumentDTO>>(FrmCollection["hdnUploadedDocumentList_tab-1"]);
                        DocumentHelper.SaveFiles(i, DocumentsList, Enums.GetEnumDescription(Enums.FolderPath.IEFullSignature), env, iDocument, "IEFullSignature", i, DocumentIds);

                        int[] DocumentIds2 = { (int)Enums.DocumentCategory_AdminUserUploadDoc.IEInitials };
                        DocumentHelper.SaveFiles(i, DocumentsList, Enums.GetEnumDescription(Enums.FolderPath.IEInitials), env, iDocument, "IEInitials", i, DocumentIds2);
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

        [HttpPost]
        public IActionResult SaveIECluster(InspectionEngineersListModel model)
        {
            try
            {
                List<InspectionEngineersListModel> lstInspectionEClusterModel = objSessionHelper.lstInspectionEClusterModel == null ? new List<InspectionEngineersListModel>() : objSessionHelper.lstInspectionEClusterModel;
                lstInspectionEClusterModel.RemoveAll(x => x.In_ID == Convert.ToInt32(model.In_ID));
                if (model.In_ID > 0)
                {
                    model.In_ID = model.In_ID;
                }
                else
                {
                    model.In_ID = lstInspectionEClusterModel.Count > 0 ? (lstInspectionEClusterModel.OrderByDescending(a => a.In_ID).FirstOrDefault().In_ID) + 1 : 1;
                }
                lstInspectionEClusterModel.Add(model);
                objSessionHelper.lstInspectionEClusterModel = lstInspectionEClusterModel;
                return Json(new { status = true, responseText = "Cluster Added Successfully." });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "InspectionEngineers ", "SaveIECluster", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpPost]
        public IActionResult LoadClusterTable([FromBody] DTParameters dtParameters)
        {
            List<InspectionEngineersListModel> lstInspectionEClusterModel = new List<InspectionEngineersListModel>();
            if (objSessionHelper.lstInspectionEClusterModel != null)
            {
                lstInspectionEClusterModel = objSessionHelper.lstInspectionEClusterModel;
            }

            DTResult<InspectionEngineersListModel> dTResult = inspectionEngineers.GetClusterValueList(dtParameters, lstInspectionEClusterModel);
            return Json(dTResult);
        }

        [HttpGet]
        public IActionResult EditIECluster(string id)
        {
            try
            {
                InspectionEngineersListModel Clster = objSessionHelper.lstInspectionEClusterModel.Where(x => x.In_ID == Convert.ToInt32(id)).FirstOrDefault();
                return Json(new { status = true, list = Clster });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "InspectionEngineers", "EditIECluster", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpPost]
        public IActionResult DeleteIECluster(string id)
        {
            try
            {
                List<InspectionEngineersListModel> lstInspectionEClusterModel = objSessionHelper.lstInspectionEClusterModel == null ? new List<InspectionEngineersListModel>() : objSessionHelper.lstInspectionEClusterModel;
                lstInspectionEClusterModel.RemoveAll(x => x.In_ID == Convert.ToInt32(id));
                objSessionHelper.lstInspectionEClusterModel = lstInspectionEClusterModel;
                return Json(new { status = true, responseText = "Cluster Deleted Successfully" });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "InspectionEngineers", "DeleteIECluster", 1, GetIPAddress());
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
                List<SelectListItem> ClusterLst = Common.GetClusterByIE(GetRegionCode, IeDepartment);
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
                string MCode = inspectionEngineers.GetMatch(IeCd, Region);
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

        [HttpGet]
        public IActionResult GetUserID(string IeEmpNo)
        {
            try
            {
                string UserId = inspectionEngineers.GetUserID(IeEmpNo);
                if (UserId == "1")
                {
                    return Json(new { status = true, responseText = "Already Existing!!!." });
                }
                else
                {
                    return Json(new { status = false, responseText = "" });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "InspectionEngineers", "GetUserID", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
    }
}
