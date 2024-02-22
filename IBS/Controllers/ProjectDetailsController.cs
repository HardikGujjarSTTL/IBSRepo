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
            List<IBS_DocumentDTO> lstDocumentUpload_Memo = iDocument.GetRecordsList((int)Enums.DocumentCategory.DetailsOfSanctionedFile, Convert.ToString(0));
            FileUploaderDTO FileUploaderUpload_Memo = new FileUploaderDTO();
            FileUploaderUpload_Memo.Mode = (int)Enums.FileUploaderMode.Add_Edit;
            FileUploaderUpload_Memo.IBS_DocumentList = lstDocumentUpload_Memo.Where(m => m.ID == (int)Enums.DocumentCategory_CANRegisrtation.Details_Of_Sanctioned_File).ToList();
            FileUploaderUpload_Memo.OthersSection = false;
            FileUploaderUpload_Memo.MaxUploaderinOthers = 5;
            FileUploaderUpload_Memo.FilUploadMode = (int)Enums.FilUploadMode.Single;
            ViewBag.Details_Of_Sanctioned_File = FileUploaderUpload_Memo;
            return View();
        }

        [HttpPost]
        public IActionResult ProjectDetailsSave(ProjectDetails model, IFormCollection FrmCollection)
        {
            try
            {
                List<APPDocumentDTO> DocumentsList = new List<APPDocumentDTO>();
                if (FrmCollection != null && FrmCollection["UploadeFile"].Count > 0)
                {
                    DocumentsList = JsonConvert.DeserializeObject<List<APPDocumentDTO>>(FrmCollection["UploadeFile"]);
                }
                model.Createdby = UserId;
                model.UpdatedBy = UserId;
                List<ProjectDetails> lstProjectDetails = new List<ProjectDetails>();
                if (objSessionHelper.lstProjectDetails != null)
                {
                    lstProjectDetails = objSessionHelper.lstProjectDetails;
                }
                int i = projectDetailsRepository.SaveProductDetailsList(model, lstProjectDetails);

                if (!string.IsNullOrEmpty(FrmCollection["UploadeFile"]))
                {
                    int[] DocumentIds = { (int)Enums.DocumentCategory_CANRegisrtation.Details_Of_Sanctioned_File };
                    DocumentHelper.SaveFiles(Convert.ToString(i), DocumentsList, Enums.GetEnumDescription(Enums.FolderPath.DetailsOfSanctionedFile), env, iDocument, string.Empty, Convert.ToString(i), DocumentIds);
                }
                return Json(new { status = true, redirectToIndex = true, responseText = "Product Details Added Successfully." });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ProjectDetails ", "SaveDetails", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpPost]
        public IActionResult SaveDetails(ProjectDetails model)
        {
            try
            {
                List<ProjectDetails> lstProjectDetails = objSessionHelper.lstProjectDetails == null ? new List<ProjectDetails>() : objSessionHelper.lstProjectDetails;
                lstProjectDetails.RemoveAll(x => x.In_ID == Convert.ToInt32(model.In_ID));
                if (model.In_ID > 0)
                {
                    model.In_ID = model.In_ID;
                }
                else
                {
                    model.In_ID = lstProjectDetails.Count > 0 ? (lstProjectDetails.OrderByDescending(a => a.In_ID).FirstOrDefault().In_ID) + 1 : 1;
                }
                lstProjectDetails.Add(model);
                objSessionHelper.lstProjectDetails = lstProjectDetails;
                return Json(new { status = true,  responseText = "Product Details Added Successfully." });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ProjectDetails ", "SaveDetails", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            List<ProjectDetails> lstProjectDetails = new List<ProjectDetails>();
            if (objSessionHelper.lstProjectDetails != null)
            {
                lstProjectDetails = objSessionHelper.lstProjectDetails;
            }

            DTResult<ProjectDetails> dTResult = projectDetailsRepository.GetProductDetailsList(dtParameters, lstProjectDetails);
            return Json(dTResult);
        }

        [HttpGet]
        public IActionResult EditProject(string id)
        {
            try
            {
                ProjectDetails Clster = objSessionHelper.lstProjectDetails.Where(x => x.In_ID == Convert.ToInt32(id)).FirstOrDefault();
                return Json(new { status = true, list = Clster });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ProjectDetails", "EditProject", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpPost]
        public IActionResult DeleteprojectDetail(string id)
        {
            try
            {
                List<ProjectDetails> lstProjectDetails = objSessionHelper.lstProjectDetails == null ? new List<ProjectDetails>() : objSessionHelper.lstProjectDetails;
                lstProjectDetails.RemoveAll(x => x.In_ID == Convert.ToInt32(id));
                objSessionHelper.lstProjectDetails = lstProjectDetails;
                return Json(new { status = true, responseText = "Project Detail Deleted Successfully" });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ProjectDetails", "DeleteprojectDetail", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
    }
}
