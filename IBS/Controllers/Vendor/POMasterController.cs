using IBS.Filters;
using IBS.Helper;
using IBS.Helpers;
using IBS.Interfaces;
using IBS.Interfaces.Administration;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace IBS.Controllers.Vendor
{
    [Authorization]
    public class POMasterController : BaseController
    {
        #region Variables
        private readonly IPOMasterRepository pOMasterRepository;
        private readonly IUploadDocRepository uploaddocRepository;
        private readonly IDocument iDocument;
        private readonly IWebHostEnvironment env;
        #endregion
        public POMasterController(IPOMasterRepository _pOMasterRepository, IUploadDocRepository _uploaddocRepository, IDocument _iDocumentRepository, IWebHostEnvironment _environment)
        {
            pOMasterRepository = _pOMasterRepository;
            uploaddocRepository = _uploaddocRepository;
            iDocument = _iDocumentRepository;
            env = _environment;
        }
        [Authorization("POMaster", "Index", "view")]
        public IActionResult Index()
        {
            return View();
        }
        [Authorization("POMaster", "Index", "view")]
        public IActionResult Manage(string CaseNo)
        {
            PO_MasterModel model = new();
            if (CaseNo != null)
            {
                model = pOMasterRepository.FindByID(CaseNo);
            }

            List<IBS_DocumentDTO> lstDocument = iDocument.GetRecordsList((int)Enums.DocumentCategory.PurchaseOrderForm, CaseNo);
            FileUploaderDTO FileUploaderDrawingSpecification = new FileUploaderDTO();
            FileUploaderDrawingSpecification.Mode = (int)Enums.FileUploaderMode.Add_Edit;
            FileUploaderDrawingSpecification.IBS_DocumentList = lstDocument.Where(m => m.ID == (int)Enums.DocumentPurchaseOrderForm.DrawingSpecification).ToList();
            FileUploaderDrawingSpecification.OthersSection = false;
            FileUploaderDrawingSpecification.MaxUploaderinOthers = 5;
            FileUploaderDrawingSpecification.FilUploadMode = (int)Enums.FilUploadMode.Single;
            ViewBag.DrawingSpecification = FileUploaderDrawingSpecification;

            List<IBS_DocumentDTO> lstAmendmentDocument = iDocument.GetRecordsList((int)Enums.DocumentCategory.PurchaseOrderForm, CaseNo);
            FileUploaderDTO FileUploaderAmendment = new FileUploaderDTO();
            FileUploaderAmendment.Mode = (int)Enums.FileUploaderMode.Add_Edit;
            FileUploaderAmendment.IBS_DocumentList = lstAmendmentDocument.Where(m => m.ID == (int)Enums.DocumentPurchaseOrderForm.Amendment).ToList();
            FileUploaderAmendment.OthersSection = false;
            FileUploaderAmendment.MaxUploaderinOthers = 5;
            FileUploaderAmendment.FilUploadMode = (int)Enums.FilUploadMode.Single;
            ViewBag.Amendment = FileUploaderAmendment;

            List<IBS_DocumentDTO> lstParentLOADocument = iDocument.GetRecordsList((int)Enums.DocumentCategory.PurchaseOrderForm, CaseNo);
            FileUploaderDTO FileUploaderParentLOA = new FileUploaderDTO();
            FileUploaderParentLOA.Mode = (int)Enums.FileUploaderMode.Add_Edit;
            FileUploaderParentLOA.IBS_DocumentList = lstParentLOADocument.Where(m => m.ID == (int)Enums.DocumentPurchaseOrderForm.ParentLOA).ToList();
            FileUploaderParentLOA.OthersSection = false;
            FileUploaderParentLOA.MaxUploaderinOthers = 5;
            FileUploaderParentLOA.FilUploadMode = (int)Enums.FilUploadMode.Single;
            ViewBag.ParentLOA = FileUploaderParentLOA;
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            int VendCd = Convert.ToInt32(IBS.Helper.SessionHelper.UserModelDTO.UserName);
            DTResult<PO_MasterModel> dTResult = pOMasterRepository.GetPOMasterList(dtParameters, VendCd);
            return Json(dTResult);
        }
        [Authorization("POMaster", "Index", "delete")]
        public IActionResult Delete(string CaseNo)
        {
            try
            {
                if (pOMasterRepository.Remove(CaseNo, UserId))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "POMaster", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorization("POMaster", "Index", "edit")]
        public IActionResult POMasterSave(PO_MasterModel model, IFormCollection FrmCollection)
        {
            try
            {
                int VendCd = Convert.ToInt32(IBS.Helper.SessionHelper.UserModelDTO.UserName);
                string msg = "PO Master Inserted Successfully.";
                if (model.PoDt > DateTime.Now)
                {
                    return Json(new { status = false, responseText = "PO Date Cannot Be Greater Then Current Date!!!" });
                }
                if (model.CaseNo != null)
                {
                    msg = "PO Master Updated Successfully.";
                    model.Updatedby = UserId;
                }
                else
                {
                    //PO_MasterModel pO_MasterModel = pOMasterRepository.alreadyExistT80_PO_MASTER(model);
                    //if (pO_MasterModel != null)
                    //{
                    //    var Retmsg = "This Po No. Already Exists Vide Ref No. " + pO_MasterModel.CaseNo + " And PO Date: " + pO_MasterModel.PoDt;
                    //    return Json(new { status = false, responseText = Retmsg });
                    //}
                    PO_MasterModel pO_MasterModel2 = pOMasterRepository.alreadyExistT13_PO_MASTER(model);
                    if (pO_MasterModel2 != null)
                    {
                        var Retmsg = "This Po No. Already Registered Vide Case No." + pO_MasterModel2.CaseNo + " And PO Date: " + pO_MasterModel2.PoDt + ". Use this Case No. to register the call using Call for Inspection Menu.";
                        return Json(new { status = false, responseText = Retmsg });
                    }
                }
                model.Createdby = UserId;
                model.VendCd = VendCd;
                //model.Purchaser = model.TempPurchaser;
                model.PoiCd = model.TempPoiCd;
                if (model.PoiCd == null || model.PoiCd == 0)
                {
                    model.PoiCd = VendCd;
                }
                string id = pOMasterRepository.POMasterDetailsInsertUpdate(model);
                if (id != "" && id != null)
                {
                    if (!string.IsNullOrEmpty(FrmCollection["hdnUploadedDocumentList_tab-1"]))
                    {
                        int[] DocumentIds = { (int)Enums.DocumentPurchaseOrderForm.DrawingSpecification, (int)Enums.DocumentPurchaseOrderForm.Amendment, (int)Enums.DocumentPurchaseOrderForm.ParentLOA };
                        List<APPDocumentDTO> DocumentsList = JsonConvert.DeserializeObject<List<APPDocumentDTO>>(FrmCollection["hdnUploadedDocumentList_tab-1"]);
                        DocumentHelper.SaveFiles(Convert.ToString(id.TrimEnd()), DocumentsList, Enums.GetEnumDescription(Enums.FolderPath.PurchaseOrderForm), env, iDocument, "POMaster", string.Empty, DocumentIds);
                    }
                    return Json(new { status = true, responseText = msg });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "POMaster", "POMasterSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        public IActionResult GetLOADetails(string CaseNo)
        {
            try
            {
                int VendCd = Convert.ToInt32(IBS.Helper.SessionHelper.UserModelDTO.UserName);
                var obj = pOMasterRepository.FindCaseNo(CaseNo, VendCd);
                if (obj != null)
                {
                    return Json(new { status = true, poMaster = obj, responseText = "" });
                }
                return Json(new { status = false, poMaster = obj, responseText = "Their is no record present for the given Case No." });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "POMaster", "GetLOADetails", 1, GetIPAddress());
            }

            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpGet]
        public IActionResult GetAgencyClient(string RlyNonrly)
        {
            try
            {
                List<SelectListItem> agencyClient = Common.GetAgencyClient(RlyNonrly);
                return Json(new { status = true, list = agencyClient });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "POMaster", "GetAgencyClient", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpGet]
        public IActionResult Getfill_consignee_purcher(string RlyNonrlyValue, string RlyNonrlyText, string RlyCd)
        {
            try
            {
                List<SelectListItem> agencyClient = Common.Getfill_consignee_purcher(RlyNonrlyValue, RlyNonrlyText, RlyCd);
                return Json(new { status = true, list = agencyClient });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "POMaster", "Getfill_consignee_purcher", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpGet]
        public IActionResult GetPurchaserCd(string consignee)
        {
            try
            {
                List<SelectListItem> agencyClient = new List<SelectListItem>();
                if (consignee != null)
                {
                    agencyClient = Common.GetPurchaserCd(consignee);
                }
                else
                {
                    SelectListItem drop = new SelectListItem();
                    drop.Text = "Other";
                    drop.Value = "0";
                    agencyClient.Add(drop);
                }
                return Json(new { status = true, list = agencyClient });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "POMaster", "GetPurchaserCd", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
        [HttpGet]
        public IActionResult GetVendor(string searchValues = null,bool isSameAs =false)
        {
            try
            {
                bool IsDigit = false;
                if (searchValues != null && searchValues != "0")
                {
                    char characterToCheck = searchValues[3];  // Access a character at the specific index (5 in this case)
                    IsDigit = Char.IsDigit(characterToCheck);
                    //IsDigit = Char.IsDigit(searchValues, 5);
                }

                int VendCdID = Convert.ToInt32(IBS.Helper.SessionHelper.UserModelDTO.UserName);
                List<SelectListItem> agencyClient = new List<SelectListItem>();
                if (isSameAs)
                {
                    agencyClient = Common.GetVendor(VendCdID);
                }
                else
                {
                    if (IsDigit)
                    {
                        agencyClient = Common.GetVendor_City(Convert.ToInt32(searchValues));
                    }
                    else
                    {
                        agencyClient = Common.GetVendorUsingTextAndValues(searchValues);
                    }
                }
                return Json(new { status = true, list = agencyClient });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "POMaster", "GetVendor", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpGet]
        public IActionResult Getvendor_status()
        {
            try
            {
                int VendCd = Convert.ToInt32(IBS.Helper.SessionHelper.UserModelDTO.UserName);
                VendorModel getvendor_status = Common.Getvendor_status(VendCd);
                if (getvendor_status != null)
                {
                    if (getvendor_status.VendStatus == "B")
                    {
                        return Json(new { status = true, responseText = "This Vendor is Banned/Blacklisted From  " + getvendor_status.VendStatusDtFr + " To " + getvendor_status.VendStatusDtTo });
                    }
                }
                return Json(new { status = false, responseText = "" });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "POMaster", "Getvendor_status", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpGet]
        public IActionResult GetManufVEND(int id = 0)
        {
            try
            {
                int VendCd = Convert.ToInt32(IBS.Helper.SessionHelper.UserModelDTO.UserName);
                if (id > 0)
                {
                    VendCd = id;
                }
                VendorModel getvendor = Common.GetManufVEND(VendCd);
                return Json(new { status = true, getvendor = getvendor });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "POMaster", "GetManufVEND", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        public IActionResult ManageVendorDetails(string CaseNo)
        {
            PO_MasterModel model = new();
            if (CaseNo != null)
            {
                model = pOMasterRepository.FindByID(CaseNo);
            }
            return View(model);
        }

        [Authorization("POMaster", "Index", "view")]
        public IActionResult PODetails(string CaseNo)
        {
            PO_MasterModel model = new();
            if (CaseNo != null)
            {
                model = pOMasterRepository.FindByID(CaseNo);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTableForPODetails([FromBody] DTParameters dtParameters)
        {
            DTResult<PO_MasterDetailListModel> dTResult = pOMasterRepository.GetPOMasterDetailsList(dtParameters);
            return Json(dTResult);
        }

        public IActionResult DeletePODetails(string CASE_NO, string ITEM_SRNO)
        {
            try
            {
                if (pOMasterRepository.RemovePODetails(CASE_NO, ITEM_SRNO, UserId))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "POMaster", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }

        public IActionResult PoDetailManage(string CASE_NO, string ITEM_SRNO)
        {
            PO_MasterDetailsModel model = new();
            PO_MasterModel modelPM = new();
            if (CASE_NO != null)
            {
                modelPM = pOMasterRepository.FindByID(CASE_NO);
            }
            if (CASE_NO != null && ITEM_SRNO != null && ITEM_SRNO != "0")
            {
                model = pOMasterRepository.FindPODetailsByID(CASE_NO, ITEM_SRNO);
            }
            else
            {
                model.CaseNo = modelPM.CaseNo;
                model.ItemSrno = Convert.ToByte(pOMasterRepository.GenerateITEM_SRNO(CASE_NO));
            }
            model.RlyCd = modelPM.RlyCd;
            model.RlyNonrly = modelPM.RlyNonrly;
            model.PoDt = modelPM.PoDt;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorization("POMaster", "Index", "edit")]
        public IActionResult POMasterDetailsSave(PO_MasterDetailsModel model)
        {
            try
            {
                int VendCd = Convert.ToInt32(IBS.Helper.SessionHelper.UserModelDTO.UserName);
                string msg = "PO Master Details Inserted Successfully.";
                if (model.CaseNo != null)
                {
                    msg = "PO Master Details Updated Successfully.";
                    model.Updatedby = UserId;
                }
                model.Createdby = UserId;
                int i = pOMasterRepository.POMasterSubDetailsInsertUpdate(model);
                if (i > 0)
                {
                    return Json(new { status = true, responseText = msg });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "POMaster", "POMasterDetailsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpGet]
        public IActionResult GetBillPayingOfficer(string SBPO)
        {
            try
            {
                List<SelectListItem> agencyClient = Common.GetBillPayingOfficerUsingSBPO(SBPO);
                return Json(new { status = true, list = agencyClient });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "POMaster", "GetBillPayingOfficer", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
        [HttpGet]
        public IActionResult GetConsigneeUsingConsignee(int ConsigneeSearch)
        {
            try
            {
                List<SelectListItem> agencyClient = Common.GetConsigneeUsingConsignee(ConsigneeSearch);
                return Json(new { status = true, list = agencyClient });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "POMaster", "GetConsigneeUsingConsignee", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        public IActionResult GetUOMChanged(decimal id)
        {
            DTResult<PO_MasterDetailsModel> dTResult = pOMasterRepository.FindByUOMDetail(id);
            return Json(dTResult);
        }
    }
}
