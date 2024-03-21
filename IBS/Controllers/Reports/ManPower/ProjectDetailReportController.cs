using IBS.Helper;
using IBS.Interfaces;
using IBS.Interfaces.Reports.ManPower;
using IBS.Models;
using IBS.Repositories;
using IBS.Repositories.Reports.ManPower;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers.Reports.ManPower
{
    public class ProjectDetailReportController : BaseController
    {
        private readonly IManpowerMasterDataReportRepository manpowerMasterDataReportRepository;
        private readonly IDocument iDocument;
        public ProjectDetailReportController(IManpowerMasterDataReportRepository _manpowerMasterDataReportRepository, IDocument _iDocumentRepository)
        {
            manpowerMasterDataReportRepository = _manpowerMasterDataReportRepository;
            iDocument = _iDocumentRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult FillProjectDetails(ProjectModel model)
        {
            try
            {
                ProjectModel models = new();
                if (model.ProjectName != null)
                {
                    models = manpowerMasterDataReportRepository.FindByID(Convert.ToInt32(model.ProjectName));
                }

                List<IBS_DocumentDTO> lstDocumentUpload_Memo = iDocument.GetRecordsList((int)Enums.DocumentCategory.DetailsOfSanctionedFile, Convert.ToString(models != null ? models.Proj_ID : 0));
                FileUploaderDTO FileUploaderUpload_Memo = new FileUploaderDTO();
                FileUploaderUpload_Memo.Mode = (int)Enums.FileUploaderMode.View;
                FileUploaderUpload_Memo.IBS_DocumentList = lstDocumentUpload_Memo.Where(m => m.ID == (int)Enums.DocumentCategory_CANRegisrtation.DetailsOfSanctionedFile).ToList();
                FileUploaderUpload_Memo.OthersSection = false;
                FileUploaderUpload_Memo.MaxUploaderinOthers = 5;
                FileUploaderUpload_Memo.FilUploadMode = (int)Enums.FilUploadMode.Single;
                ViewBag.Details_Of_Sanctioned_File = FileUploaderUpload_Memo;
                return PartialView("_ProjectDetailReport", models);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ProjectDetailReport", "FillProjectDetails", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

    }
}
