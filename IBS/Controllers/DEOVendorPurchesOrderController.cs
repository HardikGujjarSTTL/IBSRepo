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
using System.Drawing;

namespace IBS.Controllers
{
    [Authorization]
    public class DEOVendorPurchesOrderController : BaseController
    {
        #region Variables
        private readonly IDEOVendorPurchesOrderRepository deovendorpurchesRepository;
        private readonly IPOMasterRepository pOMasterRepository;
        private readonly ISendMailRepository pSendMailRepository;
        private readonly IUploadDocRepository uploaddocRepository;
        private readonly IDocument iDocument;
        private readonly IWebHostEnvironment env;
        #endregion

        public DEOVendorPurchesOrderController(IDEOVendorPurchesOrderRepository _deovendorpurchesRepository, IPOMasterRepository _pOMasterRepository, ISendMailRepository _pSendMailRepository, IUploadDocRepository _uploaddocRepository, IDocument _iDocumentRepository, IWebHostEnvironment _environment)
        {
            deovendorpurchesRepository = _deovendorpurchesRepository;
            pOMasterRepository = _pOMasterRepository;
            pSendMailRepository = _pSendMailRepository;
            uploaddocRepository = _uploaddocRepository;
            iDocument = _iDocumentRepository;
            env = _environment;
        }

        [Authorization("DEOVendorPurchesOrder", "Index", "view")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<DEOVendorPurchesOrderModel> dTResult = deovendorpurchesRepository.GetDataList(dtParameters, GetRegionCode);
            return Json(dTResult);
        }

        [Authorization("DEOVendorPurchesOrder", "Index", "view")]
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
        [ValidateAntiForgeryToken]
        [Authorization("DEOVendorPurchesOrder", "Index", "edit")]
        public IActionResult POMasterSave(PO_MasterModel model, IFormCollection FrmCollection)
        {
            try
            {
                string msg = "PO Master Inserted Successfully.";
                if (model.CaseNo != null)
                {
                    msg = "PO Master Updated Successfully.";
                    model.Updatedby = UserId;
                }
                model.Createdby = UserId;
                if (model.PoiCd == null || model.PoiCd == 0)
                {
                    model.PoiCd = model.VendCd;
                }

                //PO_MasterModel pO_MasterModel = pOMasterRepository.alreadyExistT80_PO_MASTER(model);
                //if (pO_MasterModel != null)
                //{
                //    var Retmsg = "This Po No. Already Exists Vide Ref No. " + pO_MasterModel.CaseNo + " And PO Date: " + pO_MasterModel.PoDt;
                //    return Json(new { status = false, responseText = Retmsg });
                //}
                //PO_MasterModel pO_MasterModel2 = pOMasterRepository.alreadyExistT13_PO_MASTER(model);
                //if (pO_MasterModel2 != null)
                //{
                //    var Retmsg = "This Po No. Already Registered Vide Case No." + pO_MasterModel2.CaseNo + " And PO Date: " + pO_MasterModel2.PoDt + ". Use this Case No. to register the call using Call for Inspection Menu.";
                //    return Json(new { status = false, responseText = Retmsg });
                //}
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
                Common.AddException(ex.ToString(), ex.Message.ToString(), "DEOVendorPurchesOrder", "POMasterSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        public IActionResult GetLOADetails(string CaseNo, int VendCd)
        {
            try
            {
                var obj = pOMasterRepository.FindCaseNo(CaseNo, VendCd);
                return Json(new { status = true, poMaster = obj });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "DEOVendorPurchesOrder", "GetLOADetails", 1, GetIPAddress());
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
                Common.AddException(ex.ToString(), ex.Message.ToString(), "DEOVendorPurchesOrder", "GetAgencyClient", 1, GetIPAddress());
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
                Common.AddException(ex.ToString(), ex.Message.ToString(), "DEOVendorPurchesOrder", "Getfill_consignee_purcher", 1, GetIPAddress());
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
                return Json(new { status = true, list = agencyClient });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "DEOVendorPurchesOrder", "GetPurchaserCd", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
        [HttpGet]
        public IActionResult GetVendor(int VendCd, int id = 0)
        {
            try
            {
                if (id > 0)
                {
                    VendCd = id;
                }
                List<SelectListItem> agencyClient = Common.GetVendor(VendCd);
                foreach (var item in agencyClient.Where(x => x.Value == Convert.ToString(VendCd)).ToList())
                {
                    if (item.Value == Convert.ToString(VendCd))
                    {
                        item.Selected = true;
                    }
                }
                return Json(new { status = true, list = agencyClient });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "DEOVendorPurchesOrder", "GetVendor", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpGet]
        public IActionResult Getvendor_status(int VendCd)
        {
            try
            {
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
                Common.AddException(ex.ToString(), ex.Message.ToString(), "DEOVendorPurchesOrder", "Getvendor_status", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpGet]
        public IActionResult GetManufVEND(int VendCd, int id = 0)
        {
            try
            {
                if (id > 0)
                {
                    VendCd = id;
                }
                VendorModel getvendor = Common.GetManufVEND(VendCd);
                return Json(new { status = true, getvendor = getvendor });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "DEOVendorPurchesOrder", "GetManufVEND", 1, GetIPAddress());
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

        [Authorization("DEOVendorPurchesOrder", "Index", "view")]
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

        [Authorization("DEOVendorPurchesOrder", "Index", "edit")]
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
                Common.AddException(ex.ToString(), ex.Message.ToString(), "DEOVendorPurchesOrder", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("PODetails", "DEOVendorPurchesOrder", new { CaseNo = CASE_NO });
        }

        [Authorization("DEOVendorPurchesOrder", "Index", "view")]
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
        [Authorization("DEOVendorPurchesOrder", "Index", "edit")]
        public IActionResult POMasterDetailsSave(PO_MasterDetailsModel model)
        {
            try
            {
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
                Common.AddException(ex.ToString(), ex.Message.ToString(), "DEOVendorPurchesOrder", "POMasterDetailsSave", 1, GetIPAddress());
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
                Common.AddException(ex.ToString(), ex.Message.ToString(), "DEOVendorPurchesOrder", "GetBillPayingOfficer", 1, GetIPAddress());
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
                Common.AddException(ex.ToString(), ex.Message.ToString(), "DEOVendorPurchesOrder", "GetConsigneeUsingConsignee", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        public IActionResult GetUOMChanged(decimal id)
        {
            DTResult<PO_MasterDetailsModel> dTResult = pOMasterRepository.FindByUOMDetail(id);
            return Json(dTResult);
        }

        [HttpPost]
        public IActionResult EditCaseNo(string CaseNo, string PoNo, string PoDt, string RlyCd, string RealCaseNo)
        {
            try
            {
                DEOVendorPurchesOrderModel model = new DEOVendorPurchesOrderModel();
                model.CaseNo = CaseNo.Trim();
                model.PoNo = PoNo;
                model.PoDt = Convert.ToDateTime(PoDt);
                model.RlyCd = RlyCd;
                model.RealCaseNo = RealCaseNo;

                return PartialView("_EditCaseNo", model);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "DEOVendorPurchesOrder", "EditCaseNo", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpPost]
        public IActionResult UpdateRealCaseNo(string CaseNo, string PoNo, string PoDt, string RlyCd, string RealCaseNo)
        {
            try
            {
                string retMsg = "";
                string msg = "PO Master Details Updated Successfully.";
                DEOVendorPurchesOrderModel model = new DEOVendorPurchesOrderModel();
                model.CaseNo = CaseNo.Trim();
                model.PoNo = PoNo;
                model.PoDt = Convert.ToDateTime(PoDt);
                model.RlyCd = RlyCd;
                model.RealCaseNo = RealCaseNo == "null" ? string.Empty : RealCaseNo;
                retMsg = pOMasterRepository.UpdateRealCaseNo(model);
                if (retMsg == "Not Match")
                {
                    msg = "Vendors PO NO, PO Date OR Client Does not match with the Real Case Nos PO NO, PO Date OR Client.!!!";
                    return Json(new { status = false, responseText = msg });
                }
                else if (retMsg.Trim() == CaseNo.Trim())
                {
                    bool IsSend = SendMail(CaseNo, PoNo, PoDt, RealCaseNo);
                    return Json(new { status = true, responseText = msg });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "DEOVendorPurchesOrder", "EditCaseNo", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpPost]
        public IActionResult AcceptPO(string CaseNo, string PoNo, string PoDt)
        {
            try
            {
                string retMsg = "";
                int err_code = 0;
                string RealCaseNo = "";
                string msg = "Successfully.";
                var Region = IBS.Helper.SessionHelper.UserModelDTO.Region;
                var UserID = IBS.Helper.SessionHelper.UserModelDTO.UserID;
                string[] result = new string[2];
                result = pOMasterRepository.GenerateRealCaseNo(Region, CaseNo, Convert.ToString(UserID));
                if (result.Length > 0)
                {
                    if (result[0] != null)
                    {
                        err_code = Convert.ToInt32(result[0]);
                    }
                    if (result[1] != null)
                    {
                        RealCaseNo = result[1].ToString();
                    }
                }
                if (err_code != 0 && err_code <= 7)
                {
                    retMsg = HandleErrorCodes(err_code);
                    return Json(new { status = false, responseText = retMsg });
                }
                else
                {
                    if (result[1] != null)
                    {
                        //SendMail(CaseNo, PoNo, PoDt, RealCaseNo);
                        return Json(new { status = true, responseText = msg });
                    }
                }
                return Json(new { status = false, responseText = "PO is not accepted" });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "DEOVendorPurchesOrder", "EditCaseNo", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        public string GetRegionInfo(string region)
        {
            string wRegion = "";
            if (string.IsNullOrEmpty(region))
            {
                return wRegion;
            }
            switch (region)
            {
                case "N":
                    wRegion = "NORTHERN REGION \n 12th FLOOR,CORE-II,SCOPE MINAR,LAXMI NAGAR, DELHI - 110092 \n Phone : +918800018691-95 \n Fax : 011-22024665";
                    break;
                case "S":
                    wRegion = "SOUTHERN REGION \n CTS BUILDING - 2ND FLOOR, BSNL COMPLEX, NO. 16, GREAMS ROAD,  CHENNAI - 600 006 \n Phone : 044-28292807/044- 28292817 \n Fax : 044-28290359";
                    break;
                case "E":
                    wRegion = "EASTERN REGION \n CENTRAL STATION BUILDING(METRO), 56, C.R. AVENUE,3rd FLOOR,KOLKATA-700 012  \n Fax : 033-22348704";
                    break;
                case "W":
                    wRegion = "WESTERN REGION \n 5TH FLOOR, REGENT CHAMBER, ABOVE STATUS RESTAURANT,NARIMAN POINT,MUMBAI-400021 \n Phone : 022-68943400/68943445";
                    break;
                case "C":
                    wRegion = "Central Region";
                    break;
                case "Q":
                    wRegion = "CO QA Division";
                    break;
                default:
                    break;
            }

            return wRegion;
        }

        public string HandleErrorCodes(int err_code)
        {
            string msg = "";
            if (err_code == 1)
            {
                msg = "No Purchase Order Registered by the Vendor.";
            }
            else if (err_code == 2)
            {
                msg = "Either Agent/Client or Purchaser is Missing. The value [Others] is not acceptable for Agent/Client or Purchaser";
            }
            else if (err_code == 3)
            {
                msg = "Either Consignee or BPO is Missing in Item Details. The value [Others] is not acceptable in Consignee/BPO.";
            }
            else if (err_code == 4)
            {
                msg = "Unable to Insert data in PO Master (T13). Contact System Administrator.";
            }
            else if (err_code == 5)
            {
                msg = "Unable to Insert data in PO-BPO Master (T14). Contact System Administrator.";
            }
            else if (err_code == 6)
            {
                msg = "Unable to Insert data in PO Details (T15). Contact System Administrator.";
            }
            else if (err_code == 7)
            {
                msg = "Unable to Update Case No. in T80. Contact System Administrator.";
            }
            return msg;
        }
        public bool SendMail(string CaseNo, string PoNo, string PoDt, string RealCaseNo)
        {
            var Region = IBS.Helper.SessionHelper.UserModelDTO.Region;
            string vendorEmail = pOMasterRepository.getVendorEmail(CaseNo);
            string wRegion = "";
            wRegion = GetRegionInfo(Region);
            string mail_body = "Dear Sir/Madam,\n\n In Reference to your PO: No. " + PoNo + " dated.  " + PoDt + " the Case No. allocated is  -  " + RealCaseNo + ". Kindly mention this Case No. while placing call on RITES. Thanks for using RITES Inspection Services. \n NATIONAL INSPECTION HELP LINE NUMBER : 1800 425 7000 (TOLL FREE). \n\n" + wRegion + ".";
            string sender = "";
            if (Region == "N")
            {
                sender = "nrinspn@rites.com";
            }
            else if (Region == "W")
            {
                sender = "wrinspn@rites.com";
            }
            else if (Region == "E")
            {
                sender = "erinspn@rites.com";
            }
            else if (Region == "S")
            {
                sender = "srinspn@rites.com";
            }
            else if (Region == "Q")
            {
                sender = "ritescqa@rites.com";
            }
            SendMailModel sendMailModel = new SendMailModel();
            // sender for local mail testing
            sender = "hardiksilvertouch007@outlook.com";
            sendMailModel.From = sender;
            sendMailModel.To = vendorEmail;
            sendMailModel.Subject = "Case No. allocated against PO registered by you on our Portal.";
            sendMailModel.Message = mail_body;
            bool isSend = pSendMailRepository.SendMail(sendMailModel, null);
            return isSend;
        }
    }
}
