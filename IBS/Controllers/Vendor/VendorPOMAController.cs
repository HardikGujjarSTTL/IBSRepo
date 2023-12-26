using IBS.Helper;
using IBS.Helpers;
using IBS.Interfaces;
using IBS.Interfaces.Vendor;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IBS.Controllers.Vendor
{
    public class VendorPOMAController : BaseController
    {
        private readonly IVendorPOMARepository vendorRepository;
        private readonly IDocument iDocument;
        private readonly IWebHostEnvironment env;
        private readonly IConfiguration _config;

        public VendorPOMAController(IVendorPOMARepository _vendorRepository, IDocument _iDocumentRepository, IWebHostEnvironment _environment, IConfiguration configuration)
        {
            vendorRepository = _vendorRepository;
            iDocument = _iDocumentRepository;
            env = _environment;
            _config = configuration;
        }


        public IActionResult Index(string CaseNo, string MaNo, string MaDt, string MaStatus, string MaSNo)
        {
            VendorPOMAModel model = new();
            if (CaseNo != null)
            {
                model = vendorRepository.FindByID(CaseNo, MaNo, MaDt, MaStatus, MaSNo, UserName);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<VendorPOMAModel> dTResult = vendorRepository.GetDataList(dtParameters, UserName);
            return Json(dTResult);
        }

        public IActionResult GetMatch(string CaseNo)
        {
            DTResult<VendorPOMAModel> dTResult = vendorRepository.FindMatchDetail(CaseNo, UserName);
            return Json(dTResult);
        }

        public IActionResult Manage(string id, string CaseNo, string MaNo, string MaDt, string MaStatus, byte MaSNo, string Action_T)
        {
            List<IBS_DocumentDTO> lstDocument = iDocument.GetRecordsList((int)Enums.DocumentCategory.VendorMADoc, id);
            FileUploaderDTO FileUploaderCOI = new FileUploaderDTO();
            FileUploaderCOI.Mode = (int)Enums.FileUploaderMode.Add_Edit;
            FileUploaderCOI.IBS_DocumentList = lstDocument.Where(m => m.ID == (int)Enums.DocumentCategory_VendorMADoc.VendorMADoc).ToList();
            FileUploaderCOI.OthersSection = false;
            FileUploaderCOI.MaxUploaderinOthers = 5;
            FileUploaderCOI.FilUploadMode = (int)Enums.FilUploadMode.Single;
            ViewBag.VendorMADoc = FileUploaderCOI;

            VendorPOMAModel model = new();
            model.MA_STATUS = MaStatus;
            if (Action_T == "N")
            {
                if (CaseNo != null)
                {
                    model = vendorRepository.FindManageByID(CaseNo, Convert.ToInt32(UserName));
                }
            }
            if (Action_T == "E")
            {
                if (MaStatus == "Pending")
                {
                    if (CaseNo != null)
                    {
                        model = vendorRepository.FindManageByID(CaseNo, Convert.ToInt32(UserName));
                    }
                }
                else
                {
                    if (CaseNo != null)
                    {
                        model = vendorRepository.FindManageModifyByID(CaseNo, MaNo, MaDt, MaStatus, MaSNo, Convert.ToInt32(UserName));
                    }
                }
            }

            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DetailsSave(VendorPOMAModel model, IFormCollection FrmCollection)
        {
            try
            {
                int i = 0;
                string msg = "Inserted Successfully.";
                if (model.CASE_NO != null && model.MA_NO != null && model.MA_DT != null)
                {
                    //msg = "Updated Successfully.";
                    i = vendorRepository.DetailsSave(model, model.CASE_NO, model.MA_NO, Convert.ToString(model.MA_DT), UserName);
                }

                if (i == 2)
                {
                    if (model.CASE_NO != null && model.MA_NO != null)
                    {
                        if (!string.IsNullOrEmpty(FrmCollection["hdnUploadedDocumentList_tab-1"]))
                        {
                            string UNo = model.CASE_NO + "_" + model.MA_NO;
                            int[] DocumentIds = { (int)Enums.DocumentCategory_VendorMADoc.VendorMADoc };
                            List<APPDocumentDTO> DocumentsList = JsonConvert.DeserializeObject<List<APPDocumentDTO>>(FrmCollection["hdnUploadedDocumentList_tab-1"]);
                            DocumentHelper.SaveFiles(UNo, DocumentsList, Enums.GetEnumDescription(Enums.FolderPath.VendorMADocument), env, iDocument, "MADoc", string.Empty, DocumentIds);
                        }
                    }
                    return Json(new { success = true, responseText = msg, Status = i });
                }
                if (i > 0)
                {
                    return Json(new { success = true, responseText = msg, Status = i });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Call Register Details", "DetailsSave", 1, GetIPAddress());
            }
            return Json(new { success = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        public IActionResult CaseHistoryReport(string CaseNo)
        {
            VendorPOMAModel model = new();
            if (CaseNo != null)
            {
                model = vendorRepository.FindByReportID(CaseNo, UserName);
            }
            return View(model);
        }

        public IActionResult GetMatchModify(string CaseNo, string MaNo, string MaDt)
        {
            DTResult<VendorPOMAModel> dTResult = vendorRepository.FindMatchDetailModify(CaseNo, MaNo, MaDt, UserName);
            return Json(dTResult);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DetailsUpdate(VendorPOMAModel model, IFormCollection FrmCollection)
        {
            try
            {
                int i = 0;
                string msg = "Updated Successfully.";
                if (model.CASE_NO != null && model.MA_NO != null && model.MA_DT != null)
                {
                    i = vendorRepository.DetailsUpdate(model, UserName);
                }
                if (i > 0)
                {
                    int FindDoc = vendorRepository.GetDocument(model);
                    if (FindDoc > 0)
                    {
                        if (model.CASE_NO != null && model.MA_NO != null)
                        {
                            if (!string.IsNullOrEmpty(FrmCollection["hdnUploadedDocumentList_tab-1"]))
                            {
                                string UNo = model.CASE_NO + "_" + model.MA_NO;
                                int[] DocumentIds = { (int)Enums.DocumentCategory_VendorMADoc.VendorMADoc };
                                List<APPDocumentDTO> DocumentsList = JsonConvert.DeserializeObject<List<APPDocumentDTO>>(FrmCollection["hdnUploadedDocumentList_tab-1"]);
                                DocumentHelper.SaveFiles(UNo, DocumentsList, Enums.GetEnumDescription(Enums.FolderPath.VendorMADocument), env, iDocument, "MADoc", string.Empty, DocumentIds);
                            }
                        }
                    }
                    return Json(new { success = true, responseText = msg, Status = i });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Call Register Details", "DetailsSave", 1, GetIPAddress());
            }
            return Json(new { success = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpPost]
        public IActionResult LoadTableSub([FromBody] DTParameters dtParameters)
        {
            DTResult<VendorPOMAModel> dTResult = vendorRepository.GetSubDataList(dtParameters, UserName);
            return Json(dTResult);
        }

        [HttpPost]
        public IActionResult LoadTablePODetails([FromBody] DTParameters dtParameters)
        {
            DTResult<PODetailsModel> dTResult = vendorRepository.GetDataListPODetails(dtParameters, UserName);
            return Json(dTResult);
        }

        [HttpPost]
        public IActionResult LoadTableIREPS([FromBody] DTParameters dtParameters)
        {
            DTResult<POIREPSModel> dTResult = vendorRepository.GetDataListIREPS(dtParameters, UserName);
            return Json(dTResult);
        }

        [HttpPost]
        public IActionResult LoadTablePOVENDOR([FromBody] DTParameters dtParameters)
        {
            DTResult<VendorPOMAModel> dTResult = vendorRepository.GetDataListPOVENDOR(dtParameters, UserName);
            return Json(dTResult);
        }

        [HttpPost]
        public IActionResult LoadTablePCallDetails([FromBody] DTParameters dtParameters)
        {
            DTResult<PCallDetailsModel> dTResult = vendorRepository.GetDataListPCallDetails(dtParameters, UserName);
            return Json(dTResult);
        }

        [HttpPost]
        public IActionResult LoadTableCComplaints([FromBody] DTParameters dtParameters)
        {
            DTResult<CComplaintsModel> dTResult = vendorRepository.GetDataListCComplaints(dtParameters, UserName);
            return Json(dTResult);
        }

        [HttpPost]
        public IActionResult LoadTableRVendorPlace([FromBody] DTParameters dtParameters)
        {
            DTResult<RVendorPlaceModel> dTResult = vendorRepository.GetDataListRVendorPlace(dtParameters, UserName);
            return Json(dTResult);
        }
    }
}
