using IBS.Helper;
using IBS.Helpers;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IBS.Controllers
{
    public class ProjectDetailsController : BaseController
    {
        private readonly IDocument iDocument;
        private readonly IProjectDetailsRepository projectDetailsRepository;
        private readonly IWebHostEnvironment env;
        SessionHelper objSessionHelper = new SessionHelper();

        public ProjectDetailsController(IProjectDetailsRepository _projectDetailsRepository, IDocument _iDocumentRepository, IWebHostEnvironment _environment)
        {
            iDocument = _iDocumentRepository;
            env = _environment;
            projectDetailsRepository = _projectDetailsRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<ProjectModel> dTResult = projectDetailsRepository.GetMasterList(dtParameters);
            return Json(dTResult);
        }

        public IActionResult Manage(int id)
        {
            ProjectModel model = new();
            if (id > 0)
            {
                model = projectDetailsRepository.FindByID(id);
                objSessionHelper.lstProjectDetails = model.lstProjectDetails;
            }
            else
            {
                objSessionHelper.lstProjectDetails = null;
            }

            List<IBS_DocumentDTO> lstDocumentUpload_Memo = iDocument.GetRecordsList((int)Enums.DocumentCategory.DetailsOfSanctionedFile, Convert.ToString(id));
            FileUploaderDTO FileUploaderUpload_Memo = new FileUploaderDTO();
            FileUploaderUpload_Memo.Mode = (int)Enums.FileUploaderMode.Add_Edit;
            FileUploaderUpload_Memo.IBS_DocumentList = lstDocumentUpload_Memo.Where(m => m.ID == (int)Enums.DocumentCategory_CANRegisrtation.DetailsOfSanctionedFile).ToList();
            FileUploaderUpload_Memo.OthersSection = false;
            FileUploaderUpload_Memo.MaxUploaderinOthers = 5;
            FileUploaderUpload_Memo.FilUploadMode = (int)Enums.FilUploadMode.Single;
            ViewBag.Details_Of_Sanctioned_File = FileUploaderUpload_Memo;
            return View(model);
        }

        [HttpPost]
        public IActionResult ProjectSave(ProjectModel model, IFormCollection FrmCollection)
        {
            try
            {
                string msg = "";
                model.Createdby = UserId;
                model.UpdatedBy = UserId;
                List<ProjectDetailsModel> lstProjectDetails = new List<ProjectDetailsModel>();
                if (objSessionHelper.lstProjectDetails != null)
                {
                    model.lstProjectDetails = objSessionHelper.lstProjectDetails;
                }
                int i = projectDetailsRepository.SaveProject(model);
                if (i > 0)
                {
                    if (!string.IsNullOrEmpty(FrmCollection["hdnUploadedDocumentList_tab-1"]))
                    {
                        int[] DocumentIds = { (int)Enums.DocumentCategory_CANRegisrtation.DetailsOfSanctionedFile };
                        List<APPDocumentDTO> DocumentsList = JsonConvert.DeserializeObject<List<APPDocumentDTO>>(FrmCollection["hdnUploadedDocumentList_tab-1"]);
                        DocumentHelper.SaveFiles(Convert.ToString(i), DocumentsList, Enums.GetEnumDescription(Enums.FolderPath.DetailsOfSanctionedFile), env, iDocument, string.Empty, Convert.ToString(i), DocumentIds);
                    }
                }
                msg = "Record Added Successfully.";

                return Json(new { status = true, responseText = msg });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ProjectDetails ", "ProjectSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpPost]
        public IActionResult SaveDetails(ProjectDetailsModel model)
        {
            try
            {
                var DetailsID = 0;
                if (model.ProjId > 0)
                {
                    DetailsID = projectDetailsRepository.SaveDetails(model);
                }
                List<ProjectDetailsModel> lstProjectDetails = objSessionHelper.lstProjectDetails == null ? new List<ProjectDetailsModel>() : objSessionHelper.lstProjectDetails;
                lstProjectDetails.RemoveAll(x => x.DetailID == Convert.ToInt32(model.DetailID));
                if (model.DetailID > 0)
                {
                    model.DetailID = model.DetailID;
                }
                else
                {
                    model.DetailID = lstProjectDetails.Count > 0 ? (lstProjectDetails.OrderByDescending(a => a.DetailID).FirstOrDefault().DetailID) + 1 : 1;
                }
                if (model.ProjId > 0 && DetailsID > 0)
                {
                    model.DetailID = DetailsID;
                }
                lstProjectDetails.Add(model);
                objSessionHelper.lstProjectDetails = lstProjectDetails;
                return Json(new { status = true, responseText = "Project Details Added Successfully." });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ProjectDetails ", "SaveDetails", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpPost]
        public IActionResult LoadProjectDetailsTable([FromBody] DTParameters dtParameters)
        {
            List<ProjectDetailsModel> lstProjectDetails = new List<ProjectDetailsModel>();
            if (objSessionHelper.lstProjectDetails != null)
            {
                lstProjectDetails = objSessionHelper.lstProjectDetails;
            }
            DTResult<ProjectDetailsModel> dTResult = projectDetailsRepository.GetProjectDetailsList(dtParameters, lstProjectDetails);
            return Json(dTResult);
        }

        [HttpGet]
        public IActionResult EditProject(string id)
        {
            try
            {
                ProjectDetailsModel Clster = objSessionHelper.lstProjectDetails.Where(x => x.DetailID == Convert.ToInt32(id)).FirstOrDefault();
                return Json(new { status = true, list = Clster });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ProjectDetails", "EditProject", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpPost]
        public IActionResult DeleteprojectDetail(int DetailID, int ProjID)
        {
            try
            {
                if (ProjID > 0)
                {
                    var res = projectDetailsRepository.DeleteProjectDetails(DetailID, ProjID);
                }
                List<ProjectDetailsModel> lstProjectDetails = objSessionHelper.lstProjectDetails == null ? new List<ProjectDetailsModel>() : objSessionHelper.lstProjectDetails;
                lstProjectDetails.RemoveAll(x => x.DetailID == Convert.ToInt32(DetailID));
                objSessionHelper.lstProjectDetails = lstProjectDetails;
                return Json(new { status = true, responseText = "Project Detail Deleted Successfully" });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ProjectDetails", "DeleteprojectDetail", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpPost]
        public IActionResult DeleteProject(int ID)
        {
            try
            {
                if (ID > 0)
                {
                    var res = projectDetailsRepository.DeleteProject(ID, UserId);
                    if (res > 0)
                    {

                    }
                    return Json(new { status = true, responseText = "Project Deleted Successfully" });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ProjectDetails", "DeleteProject", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
    }
}
