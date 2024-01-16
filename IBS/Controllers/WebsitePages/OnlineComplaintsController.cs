using IBS.Helper;
using IBS.Helpers;
using IBS.Interfaces;
using IBS.Interfaces.WebsitePages;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IBS.Controllers.WebsitePages
{
    public class OnlineComplaintsController : BaseController
    {
        private readonly IOnlineComplaintsRepository _onlineComplaintsRepository;
        private readonly IDocument iDocument;
        private readonly IWebHostEnvironment env;

        public OnlineComplaintsController(IDocument _iDocumentRepository, IWebHostEnvironment _environment, IOnlineComplaintsRepository onlineComplaintsRepository)
        {
            iDocument = _iDocumentRepository;
            env = _environment;
            _onlineComplaintsRepository = onlineComplaintsRepository;
        }
        public IActionResult Index()
        {
            try
            {
                List<IBS_DocumentDTO> lstDocumentUpload_Memo = iDocument.GetRecordsList((int)Enums.DocumentCategory.OnlineComplaints, Convert.ToString(0));
                FileUploaderDTO FileUploaderUpload_Memo = new FileUploaderDTO();
                FileUploaderUpload_Memo.Mode = (int)Enums.FileUploaderMode.Add_Edit;
                FileUploaderUpload_Memo.IBS_DocumentList = lstDocumentUpload_Memo.Where(m => m.ID == (int)Enums.DocumentCategory_CANRegisrtation.Upload_Rejection_Memo).ToList();
                FileUploaderUpload_Memo.OthersSection = false;
                FileUploaderUpload_Memo.MaxUploaderinOthers = 5;
                FileUploaderUpload_Memo.FilUploadMode = (int)Enums.FilUploadMode.Single;
                ViewBag.Upload_Rejection_Memo = FileUploaderUpload_Memo;
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "OnlineComplaints", "Index", 1, GetIPAddress());
            }
            return View();
        }

        public ActionResult GetItems(string ItemSno, string bkno, string setno, string InspRegionDropdown)
        {
            var json = "";
            try
            {
                json = _onlineComplaintsRepository.GetItems(ItemSno, bkno, setno, InspRegionDropdown);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "OnlineComplaints", "GetItems", 1, GetIPAddress());
            }
            return Json(json);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ComplaintsSave(OnlineComplaints onlineComplaints, IFormCollection FrmCollection)
        {
            string msg = "";
            try
            {
                string Compid = _onlineComplaintsRepository.SaveComplaints(onlineComplaints);
                if (!string.IsNullOrEmpty(FrmCollection["hdnUploadedDocumentList_tab-1"]))
                {
                    int[] DocumentIds = { (int)Enums.DocumentCategory_CANRegisrtation.Upload_Rejection_Memo };
                    List<APPDocumentDTO> DocumentsList = JsonConvert.DeserializeObject<List<APPDocumentDTO>>(FrmCollection["hdnUploadedDocumentList_tab-1"]);
                    DocumentHelper.SaveFiles(Convert.ToString(Compid), DocumentsList, Enums.GetEnumDescription(Enums.FolderPath.OnlineComplaints), env, iDocument, string.Empty, Compid, DocumentIds);
                }
                return Json(new { status = true, responseText = "Complaint Add Successfully!!" });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "OnlineComplaints", "ComplaintsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = msg });
        }
    }
}
