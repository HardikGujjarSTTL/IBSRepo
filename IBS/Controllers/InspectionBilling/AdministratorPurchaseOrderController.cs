using IBS.Filters;
using IBS.Helper;
using IBS.Helpers;
using IBS.Interfaces;
using IBS.Interfaces.Inspection_Billing;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace IBS.Controllers.InspectionBilling
{
    [Authorization]
    public class AdministratorPurchaseOrderController : BaseController
    {
        #region Variables
        private readonly IAdministratorPurchaseOrderRepository pIAdministratorPurchaseOrderRepository;
        private readonly IPOMasterRepository pOMasterRepository;
        private readonly IDocument iDocument;
        private readonly IWebHostEnvironment env;
        private readonly IMasterItemsPLFormRepository masterItemsPLFormRepository;
        #endregion
        public AdministratorPurchaseOrderController(IAdministratorPurchaseOrderRepository _pIAdministratorPurchaseOrderRepository, IPOMasterRepository _pOMasterRepository, IDocument _iDocumentRepository, IWebHostEnvironment _environment, IMasterItemsPLFormRepository _masterItemsPLFormRepository)
        {
            pIAdministratorPurchaseOrderRepository = _pIAdministratorPurchaseOrderRepository;
            pOMasterRepository = _pOMasterRepository;
            iDocument = _iDocumentRepository;
            env = _environment;
            masterItemsPLFormRepository = _masterItemsPLFormRepository;
        }

        [Authorization("AdministratorPurchaseOrder", "Index", "view")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            string region_code = Convert.ToString(IBS.Helper.SessionHelper.UserModelDTO.Region);
            DTResult<AdministratorPurchaseOrderListModel> dTResult = pIAdministratorPurchaseOrderRepository.GetPOMasterList(dtParameters, region_code);
            return Json(dTResult);
        }

        [Authorization("AdministratorPurchaseOrder", "Index", "view")]
        public IActionResult Manage(string PO_TYPE, string RLY_CD, string CaseNo)
        {
            AdministratorPurchaseOrderModel model = new();
            if (CaseNo != null)
            {
                model = pIAdministratorPurchaseOrderRepository.FindByID(CaseNo);
                string[] types = getType(model.RlyNonrly, model.RlyCd);
                if (model.VendCd != null && model.PoiCd != null)
                {
                    VendorModel getvendor = Common.GetManufVEND(Convert.ToInt32(model.VendCd));
                    model.MPOI = getvendor.VendAdd1;
                }
                ViewBag.Railway = types[0];
                ViewBag.RLY_CD = types[1];
            }
            else
            {
                string[] types = getType(PO_TYPE, RLY_CD);
                ViewBag.Railway = types[0];
                ViewBag.RLY_CD = types[1];
                model.RlyNonrly = PO_TYPE;
                model.RlyCd = RLY_CD;
            }

            List<IBS_DocumentDTO> lstDocumentUpload_a_scanned_copy = iDocument.GetRecordsList((int)Enums.DocumentCategory.PurchaseOrderFormCase, Convert.ToString(CaseNo));
            FileUploaderDTO FileUploaderUpload_a_scanned_copy = new FileUploaderDTO();
            FileUploaderUpload_a_scanned_copy.Mode = (int)Enums.FileUploaderMode.Add_Edit;
            FileUploaderUpload_a_scanned_copy.IBS_DocumentList = lstDocumentUpload_a_scanned_copy.Where(m => m.ID == (int)Enums.DocumentPurchaseOrderForm.Upload_a_scanned_copy_of_Purchase_Order).ToList();
            FileUploaderUpload_a_scanned_copy.OthersSection = false;
            FileUploaderUpload_a_scanned_copy.MaxUploaderinOthers = 5;
            FileUploaderUpload_a_scanned_copy.FilUploadMode = (int)Enums.FilUploadMode.Single;
            ViewBag.Upload_a_scanned_copy = FileUploaderUpload_a_scanned_copy;

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

        public string[] getType(string ptype, string rtype)
        {
            string[] strings = new string[2];
            if (ptype == "P" || ptype == "F" || ptype == "U" || ptype == "S")
            {
                string rcd = ptype == "P" ? "Private" :
                                     ptype == "F" ? "Foreign Railway" :
                                     ptype == "U" ? "PSU" :
                                     ptype == "S" ? "State Government" : "";
                strings[0] = rcd;
                strings[1] = " (" + rtype + ") ";
                return strings;
            }
            else
            {
                string rcd = Common.GetRailway(rtype);
                strings[0] = "Railways";
                strings[1] = " (" + rcd + ") ";
                return strings;
            }
        }
        [HttpGet]
        public IActionResult GetRailwayCode(string type)
        {
            try
            {
                List<SelectListItem> objList = Common.GetRailwayCode(type);
                return Json(new { status = true, list = objList });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "AdministratorPurchaseOrder", "GetRailwayCode", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpGet]
        public IActionResult getVendor(string vend_cd)
        {
            try
            {
                List<SelectListItem> objList = Common.GetVendCd(vend_cd);
                return Json(new { status = true, list = objList });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "AdministratorPurchaseOrder", "getVendor", 1, GetIPAddress());
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
                Common.AddException(ex.ToString(), ex.Message.ToString(), "AdministratorPurchaseOrder", "Getvendor_status", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
        [HttpGet]
        public IActionResult GetPurchaserCd(string consignee)
        {
            try
            {
                List<SelectListItem> agencyClient = Common.GetPurchaserCd(consignee);
                return Json(new { status = true, list = agencyClient });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "AdministratorPurchaseOrder", "GetPurchaserCd", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        public IActionResult GetVendors(int id = 0)
        {
            try
            {
                List<SelectListItem> agencyClient = Common.GetVendor(id);
                foreach (var item in agencyClient.Where(x => x.Value == Convert.ToString(id)).ToList())
                {
                    if (item.Value == Convert.ToString(id))
                    {
                        item.Selected = true;
                    }
                }
                return Json(new { status = true, list = agencyClient });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "AdministratorPurchaseOrder", "GetVendor", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpGet]
        public IActionResult GetManufVEND(int id = 0)
        {
            try
            {
                VendorModel getvendor = Common.GetManufVEND(id);
                return Json(new { status = true, getvendor = getvendor });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "AdministratorPurchaseOrder", "GetManufVEND", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorization("AdministratorPurchaseOrder", "Index", "edit")]
        public IActionResult POMasterSave(AdministratorPurchaseOrderModel model, IFormCollection FrmCollection)
        {
            try
            {
                string RegionCode = IBS.Helper.SessionHelper.UserModelDTO.Region;
                string msg = "Administrator Purchase Order Inserted Successfully.";
                if (model.CaseNo != null)
                {
                    msg = "Administrator Purchase Order Updated Successfully.";
                    model.Updatedby = UserId;
                }
                model.Createdby = UserId;
                model.UserId = Convert.ToString(UserId);
                model.RegionCode = RegionCode;

                //PO_MasterModel pO_MasterModel = pIAdministratorPurchaseOrderRepository.alreadyExistT80_PO_MASTER(model);
                //if (pO_MasterModel != null)
                //{
                //    var Retmsg = "This Po No. Already Exists Vide Ref No. " + pO_MasterModel.CaseNo + " And PO Date: " + pO_MasterModel.PoDt; 
                //    return Json(new { status = false, responseText = Retmsg });
                //}
                //PO_MasterModel pO_MasterModel2 = pIAdministratorPurchaseOrderRepository.alreadyExistT13_PO_MASTER(model);
                //if (pO_MasterModel2 != null)
                //{
                //    var Retmsg = "This Po No. Already Registered Vide Case No." + pO_MasterModel2.CaseNo + " And PO Date: " + pO_MasterModel2.PoDt + ". Use this Case No. to register the call using Call for Inspection Menu.";
                //    return Json(new { status = false, responseText = Retmsg });
                //}

                string id = pIAdministratorPurchaseOrderRepository.POMasterDetailsInsertUpdate(model);
                if (id != "" && id != null)
                {
                    if (!string.IsNullOrEmpty(FrmCollection["hdnUploadedDocumentList_tab-1"]))
                    {
                        string SpecificFileName = id.Trim();
                        int[] DocumentIdCases = { (int)Enums.DocumentPurchaseOrderForm.Upload_a_scanned_copy_of_Purchase_Order};
                        List<APPDocumentDTO> DocumentsCaseList = JsonConvert.DeserializeObject<List<APPDocumentDTO>>(FrmCollection["hdnUploadedDocumentList_tab-1"]).Where(x=>x.Documentid == (int)Enums.DocumentPurchaseOrderForm.Upload_a_scanned_copy_of_Purchase_Order).ToList();
                        DocumentHelper.SaveFiles(Convert.ToString(id.TrimEnd()), DocumentsCaseList, Enums.GetEnumDescription(Enums.FolderPath.AdministratorPurchaseOrderCASE_NO), env, iDocument, string.Empty, SpecificFileName, DocumentIdCases);

                        int[] DocumentIds = { (int)Enums.DocumentPurchaseOrderForm.DrawingSpecification,
                        (int)Enums.DocumentPurchaseOrderForm.Amendment,(int)Enums.DocumentPurchaseOrderForm.ParentLOA};
                        List<APPDocumentDTO> DocumentsList = JsonConvert.DeserializeObject<List<APPDocumentDTO>>(FrmCollection["hdnUploadedDocumentList_tab-1"]).Where(x => x.Documentid != (int)Enums.DocumentPurchaseOrderForm.Upload_a_scanned_copy_of_Purchase_Order).ToList(); ;
                        DocumentHelper.SaveFiles(Convert.ToString(id.TrimEnd()), DocumentsList, Enums.GetEnumDescription(Enums.FolderPath.AdministratorPurchaseOrder), env, iDocument, "AdmPurOr", string.Empty, DocumentIds);
                    }
                    return Json(new { status = true, responseText = msg });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "AdministratorPurchaseOrder", "POMasterSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        #region Consignee
        [HttpPost]
        public IActionResult ConsigneeLoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<ConsigneeListModel> dTResult = pIAdministratorPurchaseOrderRepository.GetConsigneeDetaisList(dtParameters);
            return Json(dTResult);
        }

        public IActionResult ConsigneeDelete(string CASE_NO, string CONSIGNEE_CD, string BPO_CD)
        {
            try
            {
                if (pIAdministratorPurchaseOrderRepository.ConsigneeDelete(CASE_NO, CONSIGNEE_CD, BPO_CD))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "AdministratorPurchaseOrder", "ConsigneeDelete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Manage", new { CaseNo = CASE_NO });
        }

        [HttpPost]
        public IActionResult AddEditConsignee(string CaseNo, int Consignee_CD, string RLY_CD)
        {
            try
            {
                ConsigneeModel model = new ConsigneeModel();
                if (CaseNo != null)
                {
                    model = pIAdministratorPurchaseOrderRepository.FindConsigneeByID(CaseNo, Consignee_CD);
                }
                ViewBag.RLY_CD = RLY_CD;
                return PartialView("_AddEditConsignee", model);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "AdministratorPurchaseOrder", "AddEditConsignee", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpGet]
        public IActionResult getConsignee(int ConsigneeCd)
        {
            try
            {
                List<SelectListItem> objList = Common.GetConsigneeUsingConsignee(ConsigneeCd);
                return Json(new { status = true, list = objList });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "AdministratorPurchaseOrder", "getConsignee", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpGet]
        public IActionResult getBPO(string SBPO)
        {
            try
            {
                List<SelectListItem> objList = Common.GetBillPayingOfficerUsingSBPO(SBPO);
                return Json(new { status = true, list = objList });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "AdministratorPurchaseOrder", "getBPO", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpPost]
        public IActionResult SaveConsignee(string CaseNo, int ConsigneeCd, string BpoCd)
        {
            try
            {
                ConsigneeModel model = new ConsigneeModel();
                string RegionCode = IBS.Helper.SessionHelper.UserModelDTO.Region;
                string msg = "Consignee Inserted Successfully.";
                if (CaseNo != null && ConsigneeCd > 0)
                {
                    msg = "Consignee Updated Successfully.";
                }
                model.CaseNo = CaseNo;
                model.ConsigneeCd = ConsigneeCd;
                model.BpoCd = BpoCd;
                string i = pIAdministratorPurchaseOrderRepository.SaveConsignee(model);
                if (i != "" && i != null)
                {
                    return Json(new { status = true, responseText = msg });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "DEOVendorPurchesOrder", "SaveConsignee", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        #endregion
        #region PODetails
        [Authorization("AdministratorPurchaseOrder", "Index", "view")]
        public IActionResult PODetails(string CaseNo)
        {
            AdministratorPurchaseOrderModel model = new();
            if (CaseNo != null)
            {
                model = pIAdministratorPurchaseOrderRepository.FindByID(CaseNo);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTableForPODetails([FromBody] DTParameters dtParameters)
        {
            DTResult<PO_MasterDetailListModel> dTResult = pIAdministratorPurchaseOrderRepository.GetPOMasterDetailsList(dtParameters);
            return Json(dTResult);
        }

        public IActionResult DeletePODetails(string CASE_NO, string ITEM_SRNO)
        {
            try
            {
                if (pIAdministratorPurchaseOrderRepository.RemovePODetails(CASE_NO, ITEM_SRNO, UserId))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "AdministratorPurchaseOrder", "DeletePODetails", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("PODetails", new { CaseNo = CASE_NO });
        }

        public IActionResult PoDetailManage(string CASE_NO, string ITEM_SRNO)
        {
            PO_MasterDetailsModel model = new();
            AdministratorPurchaseOrderModel modelPM = new();
            if (CASE_NO != null)
            {
                modelPM = pIAdministratorPurchaseOrderRepository.FindByID(CASE_NO);
            }
            if (CASE_NO != null && ITEM_SRNO != null && ITEM_SRNO != "0")
            {
                model = pIAdministratorPurchaseOrderRepository.FindPODetailsByID(CASE_NO, ITEM_SRNO);
            }
            else
            {
                model.CaseNo = modelPM.CaseNo;
                model.ItemSrno = Convert.ToByte(pIAdministratorPurchaseOrderRepository.GenerateITEM_SRNO(CASE_NO));
            }
            model.RlyCd = modelPM.RlyCd;
            model.RlyNonrly = modelPM.RlyNonrly;
            model.PoDt = modelPM.PoDt;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorization("AdministratorPurchaseOrder", "Index", "edit")]
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
                int i = pIAdministratorPurchaseOrderRepository.POMasterSubDetailsInsertUpdate(model);
                if (i > 0)
                {
                    return Json(new { status = true, responseText = msg });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "AdministratorPurchaseOrder", "POMasterDetailsSave", 1, GetIPAddress());
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

        #endregion

        [HttpPost]
        public IActionResult EditPODate(string CaseNo)
        {
            try
            {
                AdministratorPurchaseOrderModel model = new AdministratorPurchaseOrderModel();
                if (CaseNo != null)
                {
                    model = pIAdministratorPurchaseOrderRepository.FindByID(CaseNo);
                }

                return PartialView("_EditPODate", model);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "DEOVendorPurchesOrder", "EditCaseNo", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpPost]
        public IActionResult UpdatePODate(string CaseNo, string PoDtNew)
        {
            try
            {
                AdministratorPurchaseOrderModel model=new AdministratorPurchaseOrderModel();
                string msg = "PO Date Updated Successfully.";
                model.CaseNo = CaseNo;
                model.PoDtNew =Convert.ToDateTime(PoDtNew);
                string id = pIAdministratorPurchaseOrderRepository.UpdatePODate(model);
                if (id != null)
                {
                    return Json(new { status = true, responseText = msg });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "AdministratorPurchaseOrder", "UpdatePODate", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpGet]
        public IActionResult GetPODetails(string PlNo)
        {
            try
            {
                MasterItemsPLFormModel model = new();
                if (!string.IsNullOrEmpty(PlNo))
                {
                    model = masterItemsPLFormRepository.FindByID(PlNo);
                }
                return Json(new { status = true, model = model });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "AdministratorPurchaseOrder", "GetPODetails", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
    }
}
