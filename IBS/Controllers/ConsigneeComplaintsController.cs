using IBS.Filters;
using IBS.Helper;
using IBS.Helpers;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IBS.Controllers
{
    public class ConsigneeComplaintsController : BaseController
    {
        private readonly IDocument iDocument;
        private readonly IWebHostEnvironment env;
        private readonly IConsigneeComplaintsRepository consigneeComplaints;
        public ConsigneeComplaintsController(IDocument _iDocumentRepository, IWebHostEnvironment _environment, IConsigneeComplaintsRepository _ConsigneeComplaintsRepository)
        {
            iDocument = _iDocumentRepository;
            env = _environment;
            consigneeComplaints = _ConsigneeComplaintsRepository;
        }
        [Authorization("ConsigneeComplaints", "Index", "view")]
        public IActionResult Index()
        {
            return View();
        }
        [Authorization("ConsigneeComplaints", "Index", "view")]
        public IActionResult Manage(string CASE_NO, string BK_NO, string SET_NO, string ComplaintId)
        {
            ConsigneeComplaints model = new();

            try
            {
                List<IBS_DocumentDTO> lstDocumentUpload_Memo = iDocument.GetRecordsList((int)Enums.DocumentCategory.ConsigneeComplaints, Convert.ToString(model.ComplaintId));
                FileUploaderDTO FileUploaderUpload_Memo = new FileUploaderDTO();
                FileUploaderUpload_Memo.Mode = (int)Enums.FileUploaderMode.Add_Edit;
                FileUploaderUpload_Memo.IBS_DocumentList = lstDocumentUpload_Memo.Where(m => m.ID == (int)Enums.DocumentCategory_CANRegisrtation.Upload_Rejection_Memo).ToList();
                FileUploaderUpload_Memo.OthersSection = false;
                FileUploaderUpload_Memo.MaxUploaderinOthers = 5;
                FileUploaderUpload_Memo.FilUploadMode = (int)Enums.FilUploadMode.Single;
                ViewBag.Upload_Rejection_Memo = FileUploaderUpload_Memo;

                List<IBS_DocumentDTO> lstDocumentUpload_Tech_Ref = iDocument.GetRecordsList((int)Enums.DocumentCategory.ConsigneeComplaints, Convert.ToString(model.ComplaintId));
                FileUploaderDTO FileUploaderUpload_Tech_Ref = new FileUploaderDTO();
                FileUploaderUpload_Tech_Ref.Mode = (int)Enums.FileUploaderMode.Add_Edit;
                FileUploaderUpload_Tech_Ref.IBS_DocumentList = lstDocumentUpload_Tech_Ref.Where(m => m.ID == (int)Enums.DocumentCategory_CANRegisrtation.Upload_Tech_Ref1).ToList();
                FileUploaderUpload_Tech_Ref.OthersSection = false;
                FileUploaderUpload_Tech_Ref.MaxUploaderinOthers = 5;
                FileUploaderUpload_Tech_Ref.FilUploadMode = (int)Enums.FilUploadMode.Single;
                ViewBag.Upload_Tech_Ref = FileUploaderUpload_Tech_Ref;

                List<IBS_DocumentDTO> lstDocumentUpload_Ji_Case = iDocument.GetRecordsList((int)Enums.DocumentCategory.ConsigneeComplaints, Convert.ToString(model.ComplaintId));
                FileUploaderDTO FileUploaderUpload_Ji_Case = new FileUploaderDTO();
                FileUploaderUpload_Ji_Case.Mode = (int)Enums.FileUploaderMode.Add_Edit;
                FileUploaderUpload_Ji_Case.IBS_DocumentList = lstDocumentUpload_Ji_Case.Where(m => m.ID == (int)Enums.DocumentCategory_CANRegisrtation.Upload_JI_Case).ToList();
                FileUploaderUpload_Ji_Case.OthersSection = false;
                FileUploaderUpload_Ji_Case.MaxUploaderinOthers = 5;
                FileUploaderUpload_Ji_Case.FilUploadMode = (int)Enums.FilUploadMode.Single;
                ViewBag.Upload_JI_Case = FileUploaderUpload_Ji_Case;

                List<IBS_DocumentDTO> lstDocumentUpload_Ji_report = iDocument.GetRecordsList((int)Enums.DocumentCategory.ConsigneeComplaints, Convert.ToString(model.ComplaintId));
                FileUploaderDTO FileUploaderUpload_Ji_report = new FileUploaderDTO();
                FileUploaderUpload_Ji_report.Mode = (int)Enums.FileUploaderMode.Add_Edit;
                FileUploaderUpload_Ji_report.IBS_DocumentList = lstDocumentUpload_Ji_report.Where(m => m.ID == (int)Enums.DocumentCategory_CANRegisrtation.Upload_JI_Report).ToList();
                FileUploaderUpload_Ji_report.OthersSection = false;
                FileUploaderUpload_Ji_report.MaxUploaderinOthers = 5;
                FileUploaderUpload_Ji_report.FilUploadMode = (int)Enums.FilUploadMode.Single;
                ViewBag.Upload_JI_Report = FileUploaderUpload_Ji_report;
                if (ComplaintId != "" && ComplaintId != null)
                {
                    model = consigneeComplaints.FindByCompID(ComplaintId);
                    ViewBag.Showcomplaint = true;
                }
                if (CASE_NO != null && BK_NO != null && SET_NO != null)
                {
                    model = consigneeComplaints.FindByID(CASE_NO, BK_NO, SET_NO);
                    ViewBag.Showcomplaint = false;
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult GetConsData([FromBody] DTParameters dtParameters)
        {
            DTResult<ConsigneeComplaints> dTResult = consigneeComplaints.GetDataListConsignee(dtParameters);
            return Json(dTResult);
        }
        [HttpPost]
        public IActionResult GetCompdata([FromBody] DTParameters dtParameters)
        {
            DTResult<ConsigneeComplaints> dTResult = consigneeComplaints.GetDataListComplaint(dtParameters);
            return Json(dTResult);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorization("ConsigneeComplaints", "Index", "edit")]
        public IActionResult ComplaintsDetailsSave(ConsigneeComplaints model)
        {
            try
            {
                model.UserId = Convert.ToString(UserId);
                string msg = "Complaints Inserted Successfully.";

                if (model.ComplaintId != null && model.ComplaintId != "")
                {
                    msg = "Complaints Updated Successfully.";
                }
                string i = consigneeComplaints.ComplaintsDetailsInsertUpdate(model);
                if (i != "")
                {
                    return Json(new { status = true, responseText = msg, redirectToIndex = true });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ConsigneeComplaints", "ComplaintsDetailsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorization("ConsigneeComplaints", "Index", "edit")]
        public IActionResult ComplaintSaveChoice(ConsigneeComplaints model)
        {
            try
            {
                string msg = "";
                model.UserId = Convert.ToString(UserId);
                if (model.ComplaintId != null && model.ComplaintId != "")
                {
                    msg = "Data Updated Successfully.";
                }
                string i = consigneeComplaints.JIChoice(model);
                if (i != "")
                {
                    return Json(new { status = true, responseText = msg, redirectToIndex = true });
                }
                else
                {
                    msg = "Invalid Selection.\\n\\n Valid options are --> [Yes / No] ";
                    return Json(new { status = false, responseText = msg });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ConsigneeComplaints", "ComplaintSaveChoice", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorization("ConsigneeComplaints", "Index", "edit")]
        public IActionResult ComplaintCancelJI(ConsigneeComplaints model)
        {
            try
            {
                string msg = "";
                model.UserId = Convert.ToString(UserId);
                if (model.ComplaintId != null && model.ComplaintId != "")
                {
                    msg = "JI Cancel Successfully.";
                }
                string i = consigneeComplaints.CancelJI(model);
                if (i != "")
                {
                    return Json(new { status = true, responseText = msg, redirectToIndex = true });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ConsigneeComplaints", "ComplaintCancelJI", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorization("ConsigneeComplaints", "Index", "edit")]
        public IActionResult JIOutCome(ConsigneeComplaints model, IFormCollection FrmCollection)
        {
            try
            {
                model.UserId = Convert.ToString(UserId);
                string msg = "";

                if (model.ComplaintId != null && model.ComplaintId != "")
                {
                    msg = "Data Save Successfully.";
                }
                string i = consigneeComplaints.JIOutCome(model);
                if (i != "")
                {
                    var FileName = model.CASE_NO + "-" + model.BK_NO + "-" + model.SET_NO;
                    if (!string.IsNullOrEmpty(FrmCollection["hdnUploadedDocumentList_tab-1"]))
                    {
                        int[] DocumentIds = { (int)Enums.DocumentCategory_CANRegisrtation.Upload_JI_Report };
                        List<APPDocumentDTO> DocumentsList = JsonConvert.DeserializeObject<List<APPDocumentDTO>>(FrmCollection["hdnUploadedDocumentList_tab-1"]);
                        if (DocumentsList[0].DocName == "Upload JI Report")
                        {
                            DocumentHelper.SaveFiles(Convert.ToString(model.ComplaintId), DocumentsList, Enums.GetEnumDescription(Enums.FolderPath.COMPLAINTSREPORT), env, iDocument, string.Empty, FileName, DocumentIds);
                        }
                    }
                    return Json(new { status = true, responseText = msg, redirectToIndex = true });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ConsigneeComplaints", "ComplaintCancelJI", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorization("ConsigneeComplaints", "Index", "edit")]
        public IActionResult FinalDisposal(ConsigneeComplaints model, IFormCollection FrmCollection)
        {
            try
            {
                string msg = "";
                model.UserId = Convert.ToString(UserId);
                if (model.ComplaintId != null && model.ComplaintId != "")
                {
                    msg = "Data Save Successfully.";
                }
                string i = consigneeComplaints.FinalDisposal(model);
                if (i != "")
                {
                    if (!string.IsNullOrEmpty(FrmCollection["hdnUploadedDocumentList_tab-1"]))
                    {
                        var FileName = model.CASE_NO + "-" + model.BK_NO + "-" + model.SET_NO;
                        int[] DocumentIds = { (int)Enums.DocumentCategory_CANRegisrtation.Upload_Tech_Ref1 };
                        List<APPDocumentDTO> DocumentsList = JsonConvert.DeserializeObject<List<APPDocumentDTO>>(FrmCollection["hdnUploadedDocumentList_tab-1"]);
                        if (DocumentsList[0].DocName == "Upload Tech Ref")
                        {
                            DocumentHelper.SaveFiles(Convert.ToString(model.ComplaintId), DocumentsList, Enums.GetEnumDescription(Enums.FolderPath.ComplaintTechRef), env, iDocument, string.Empty, FileName, DocumentIds);
                        }
                    }
                    return Json(new { status = true, responseText = msg, redirectToIndex = true });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ConsigneeComplaints", "FinalDisposal", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorization("ConsigneeComplaints", "Index", "edit")]
        public IActionResult UploadPDF(ConsigneeComplaints model, IFormCollection FrmCollection)
        {
            string msg = "";
            List<APPDocumentDTO> DocumentsList = new List<APPDocumentDTO>();
            if (FrmCollection != null && FrmCollection["hdnUploadedDocumentList_tab-1"].Count > 0)
            {
                DocumentsList = JsonConvert.DeserializeObject<List<APPDocumentDTO>>(FrmCollection["hdnUploadedDocumentList_tab-1"]);
            }
            var FileName = model.CASE_NO + "-" + model.BK_NO + "-" + model.SET_NO;
            msg = "Select file to upload.";
            if (!string.IsNullOrEmpty(FrmCollection["hdnUploadedDocumentList_tab-1"]))
            {
                if (DocumentsList.Count > 1)
                {
                    if (DocumentsList[0].DocName == "Upload JI Case")
                    {
                        int[] DocumentIds1 = { (int)Enums.DocumentCategory_CANRegisrtation.Upload_JI_Case };
                        DocumentHelper.SaveFiles(Convert.ToString(model.ComplaintId), DocumentsList, Enums.GetEnumDescription(Enums.FolderPath.ComplaintCase), env, iDocument, FileName, string.Empty, DocumentIds1);
                    }
                    //if (DocumentsList[1].DocName == "Upload Rejection Memo")
                    //{
                    //    int[] DocumentIds = { (int)Enums.DocumentCategory_CANRegisrtation.Upload_Rejection_Memo };
                    //    DocumentHelper.SaveFiles(Convert.ToString(model.ComplaintId), DocumentsList, Enums.GetEnumDescription(Enums.FolderPath.RejectionMemo), env, iDocument, FileName, string.Empty, DocumentIds);
                    //}
                    msg = "Upload Successfully.";
                    return Json(new { status = true, responseText = msg, redirectToIndex = true });
                }
            }
            return Json(new { status = false, responseText = msg, redirectToIndex = true });
        }
    }
}
