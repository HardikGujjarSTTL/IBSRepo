using IBS.Filters;
using IBS.Helper;
using IBS.Helpers;
using IBS.Interfaces;
using IBS.Interfaces.Administration;
using IBS.Interfaces.Inspection_Billing;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IBS.Controllers.InspectionBilling
{
    [Authorization]
    public class ICCancellationController : BaseController
    {
        #region Variables
        private readonly IICCancellationRepository iCCancellationRepository;
        private readonly IUploadDocRepository uploaddocRepository;
        private readonly IDocument iDocument;
        private readonly IWebHostEnvironment env;
        #endregion
        public ICCancellationController(IICCancellationRepository _iCCancellationRepository, IUploadDocRepository _uploaddocRepository, IDocument _iDocumentRepository, IWebHostEnvironment _environment)
        {
            iCCancellationRepository = _iCCancellationRepository;
            uploaddocRepository = _uploaddocRepository;
            iDocument = _iDocumentRepository;
            env = _environment;
        }
        [Authorization("ICCancellation", "Index", "view")]
        public IActionResult Index()
        {
            string Region = Convert.ToString(IBS.Helper.SessionHelper.UserModelDTO.Region);
            ViewBag.Region = Region;
            return View();
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            string Region = Convert.ToString(IBS.Helper.SessionHelper.UserModelDTO.Region);
            DTResult<ICCancellationListModel> dTResult = iCCancellationRepository.GetCancellationList(dtParameters, Region);
            return Json(dTResult);
        }

        [Authorization("ICCancellation", "Index", "view")]
        public IActionResult Manage(string REGION, string BK_NO, string SET_NO)
        {
            ICCancellationModel model = new();
            string Region = Convert.ToString(IBS.Helper.SessionHelper.UserModelDTO.Region);
            ViewBag.Region = Region;
            if (REGION != null && BK_NO != "" && SET_NO != "")
            {
                model = iCCancellationRepository.FindByID(REGION, BK_NO, SET_NO);
                model.IsEdit = 1;
            }
            else
            {
                model.IsEdit = 0;
                model.Region = Region;
            }
            List<IBS_DocumentDTO> lstDocument = iDocument.GetRecordsList((int)Enums.DocumentCategory.ICCancellation, model.Id.ToString());
            FileUploaderDTO FileUploaderCOI = new FileUploaderDTO();
            FileUploaderCOI.Mode = (int)Enums.FileUploaderMode.Add_Edit;
            FileUploaderCOI.IBS_DocumentList = lstDocument.Where(m => m.ID == (int)Enums.DocumentICCancellation.FIR_Upload).ToList();
            FileUploaderCOI.OthersSection = false;
            FileUploaderCOI.MaxUploaderinOthers = 5;
            FileUploaderCOI.FilUploadMode = (int)Enums.FilUploadMode.Single;
            ViewBag.FIR_Upload = FileUploaderCOI;
            return View(model);
        }

        [Authorization("ICCancellation", "Index", "delete")]
        public IActionResult Delete(string REGION, string BK_NO, string SET_NO)
        {
            try
            {
                if (iCCancellationRepository.Remove(REGION, BK_NO, SET_NO, UserId))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ICCancellation", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorization("ICCancellation", "Index", "edit")]
        public IActionResult ICCancellationSave(ICCancellationModel model, IFormCollection FrmCollection)
        {
            try
            {
                int id = 0;
                if (model.IsEdit > 0)
                {
                    model.Updatedby = UserId;
                    if (RoleName != "Inspection Engineer (IE)")
                    {
                        model.IsAdmin = true;
                    }
                    id = iCCancellationRepository.ICCancellationSave(model);
                    AlertAddSuccess("IC Cancellation Updated Successfully.");
                }
                else
                {
                    model.Createdby = UserId;
                    if (RoleName == "Inspection Engineer (IE)")
                    {
                        model.Status = false;
                    }
                    else
                    {
                        model.Status = true;
                    }
                    id = iCCancellationRepository.ICCancellationSave(model);
                    AlertAddSuccess("IC Cancellation Inserted Successfully.");
                }
                if (id > 0)
                {
                    if (!string.IsNullOrEmpty(FrmCollection["hdnUploadedDocumentList_tab-1"]))
                    {
                        int[] DocumentIds = { (int)Enums.DocumentICCancellation.FIR_Upload };
                        List<APPDocumentDTO> DocumentsList = JsonConvert.DeserializeObject<List<APPDocumentDTO>>(FrmCollection["hdnUploadedDocumentList_tab-1"]);
                        DocumentHelper.SaveFiles(Convert.ToString(id), DocumentsList, Enums.GetEnumDescription(Enums.FolderPath.ICCancellation), env, iDocument, "ICCancellation", string.Empty, DocumentIds);
                    }
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ICCancellation", "ICCancellationSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
    }
}
