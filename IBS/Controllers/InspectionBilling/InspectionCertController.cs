using IBS.Helper;
using IBS.Helpers;
using IBS.Interfaces;
using IBS.Interfaces.InspectionBilling;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IBS.Controllers.InspectionBilling
{
    public class InspectionCertController : BaseController
    {
        #region Variables
        private readonly IInspectionCertRepository inpsRepository;
        private readonly IDocument iDocument;
        private readonly IWebHostEnvironment env;
        private readonly IConfiguration _config;

        #endregion
        public InspectionCertController(IInspectionCertRepository _inpsRepository, IDocument _iDocumentRepository, IWebHostEnvironment _environment, IConfiguration configuration)
        {
            inpsRepository = _inpsRepository;
            iDocument = _iDocumentRepository;
            env = _environment;
            _config = configuration;
        }

        public IActionResult Index(string CaseNo, DateTime? CallRecvDt, int CallSno, string Bkno, string Setno)
        {
            InspectionCertModel model = new();
            if (CaseNo != "" && CallRecvDt != null && CallSno > 0)
            {
                model = inpsRepository.FindByID(CaseNo, CallRecvDt, CallSno, Bkno, Setno, GetRegionCode);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<InspectionCertModel> dTResult = inpsRepository.GetDataList(dtParameters, Region);
            return Json(dTResult);
        }

        public IActionResult InspectionDetails(string CaseNo, DateTime? CallRecvDt, int CallSno, string Bkno, string Setno, string ActionType)
        {
            InspectionCertModel model = new();
            if (CaseNo != "" && CallRecvDt != null && CallSno > 0)
            {
                model = inpsRepository.FindByInspDetailsID(CaseNo, CallRecvDt, CallSno, Bkno, Setno, ActionType, Region, RoleId);
            }
            model.ActionType = ActionType;
            List<IBS_DocumentDTO> lstDocument = iDocument.GetRecordsList((int)Enums.DocumentCategory.ICDocument, model.BillNo);
            FileUploaderDTO FileUploaderCOI = new FileUploaderDTO();
            FileUploaderCOI.Mode = (int)Enums.FileUploaderMode.Add_Edit;
            FileUploaderCOI.IBS_DocumentList = lstDocument.Where(m => m.ID == (int)Enums.DocumentCategory_AdminUserUploadDoc.ICDocument).ToList();
            FileUploaderCOI.OthersSection = false;
            FileUploaderCOI.MaxUploaderinOthers = 5;
            FileUploaderCOI.FilUploadMode = (int)Enums.FilUploadMode.Single;
            ViewBag.ICDocument = FileUploaderCOI;

            return View(model);
        }

        public IActionResult GetBillDetails(string BillNo)
        {
            DTResult<InspectionCertModel> dTResult = inpsRepository.GetBillDetails(BillNo);
            return Json(dTResult);
        }

        public IActionResult GetConsignee(int ConsigneeCd)
        {
            DTResult<InspectionCertModel> dTResult = inpsRepository.GetConsignee(ConsigneeCd);
            return Json(dTResult);
        }

        public IActionResult GetBPO(string BPOCd)
        {
            DTResult<InspectionCertModel> dTResult = inpsRepository.GetBPO(BPOCd);
            return Json(dTResult);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateGST(InspectionCertModel model)
        {
            try
            {
                string msg = "";
                if (model.GstinNo != null && model.LegalName != null)
                {
                    msg = "GSTIN NO. & LEGAL NAME HAS BEEN UPDATED!!!";
                    model.Updatedby = UserId;
                    model.UserId = UserName;
                }

                int i = inpsRepository.UpdateGSTDetails(model, UserName);
                if (i > 0)
                {
                    return Json(new { success = true, responseText = msg, Status = i });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "CallRegisterIB", "CallDetailsSave", 1, GetIPAddress());
            }
            return Json(new { success = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        public IActionResult GetBPOList(string BpoCd)
        {
            return Json(Common.GetBPOList(BpoCd));
        }

        int CheckDateDiff(string dt1, string dt2, int diff)
        {
            DateTime w_dt1 = new(Convert.ToInt32(dt1.Substring(6, 4)), Convert.ToInt32(dt1.Substring(3, 2)), Convert.ToInt32(dt1.Substring(0, 2)));
            DateTime w_dt2 = new(Convert.ToInt32(dt2.Substring(6, 4)), Convert.ToInt32(dt2.Substring(3, 2)), Convert.ToInt32(dt2.Substring(0, 2)));
            TimeSpan ts = w_dt1 - w_dt2;
            int differenceInDays = ts.Days;

            if (differenceInDays > diff)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        [HttpPost]
        public IActionResult InspectionCertSave(InspectionCertModel model)
        {
            try
            {
                string i = "";
                string msg = "Save Successfully.";

                if (Region == "N")
                {
                    string mess = "";
                    if (model.CallDt == null)
                    {
                        model.CallDt = model.Callrecvdt;
                    }
                    int FinspCdtdiff = CheckDateDiff(Convert.ToString(model.FirstInspDt), Convert.ToString(model.CallDt), 7);
                    int ICdtLinspdiff = CheckDateDiff(Convert.ToString(model.CertDt), Convert.ToString(model.LastInspDt), 3);
                    if (FinspCdtdiff == 1)
                    {
                        mess = "First Inspection Date - Call Date is greater then 7 Days!!!";
                        //AlertDanger(mess);
                        return Json(new { status = false, responseText = mess, Id = i });
                    }
                    if (ICdtLinspdiff == 1)
                    {
                        if (mess == "")
                        {
                            mess = "IC Date - Last Inspection Date is greater then 3 Days!!!";
                        }
                        else
                        {
                            mess = mess + " & IC Date - Last Inspection Date is greater then 3 Days!!!";
                        }
                        //AlertDanger(mess);
                        return Json(new { status = false, responseText = mess, Id = i });
                    }

                }
                if (model.Caseno != null && model.Callrecvdt != null && model.Callsno > 0)
                {
                    model.UserId = Convert.ToString(UserId);
                    model.Createdby = UserName;
                    i = inpsRepository.InspectionCertSave(model, Region);
                }
                if (i != "")
                {
                    return Json(new { status = true, responseText = msg, Id = i, BkNo = model.Bkno, SetNo = model.Setno });
                }
                else
                {
                    msg = "This Call cannot be deleted. because IC is present for this call!!!";
                    return Json(new { status = false, responseText = msg, Id = i });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "VenderCallRegisterModel", "DetailsDelete", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpPost]
        public IActionResult ReturnBillSubmit(InspectionCertModel model)
        {
            try
            {
                string str = "";
                if (model.BillNo != null && model.BillDt != null)
                {
                    model.UserId = UserName;
                    model.Createdby = UserName;
                    str = inpsRepository.ReturnBillSubmit(model, Region);
                }
                if (str != "")
                {
                    return Json(new { status = true, responseText = str });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "InspectionCert", "ReturnBillSubmit", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        public IActionResult Validation(InspectionCertModel model)
        {
            try
            {
                string str = "";
                str = inpsRepository.Validation(model, Region);
                return Json(new { status = true, responseText = str });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "InspectionCert", "Validation", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpPost]
        public IActionResult LoadTableDetails([FromBody] DTParameters dtParameters)
        {
            DTResult<InspectionCertModel> dTResult = inpsRepository.GetLoadTableDetails(dtParameters, Region);
            return Json(dTResult);
        }

        [HttpPost]
        public IActionResult BillUpdate(InspectionCertModel model)
        {
            try
            {
                string i = "";
                string msg = "";

                string myYear, myMonth, myDay;
                myYear = Convert.ToString(model.BillDt).Substring(6, 4);
                myMonth = Convert.ToString(model.BillDt).Substring(3, 2);
                myDay = Convert.ToString(model.BillDt).Substring(0, 2);
                string certdt = myYear + myMonth + myDay;
                //int idt = dt1.CompareTo(DateTime.Now.Date);

                string dt1 = Convert.ToDateTime(model.BillDt).ToString("dd/MM/yyyy");
                int idt = dt1.CompareTo(DateTime.Now.Date.ToString("dd/MM/yyyy"));

                model.Regioncode = Region;
                int fyr = inpsRepository.financial_year_check(model);
                if (fyr == 1)
                {
                    msg = "Bill must be generated within the same financial year in which IC was Issued!!!" + ". \\n(ie. Certificate Date & Bill Date shoud be in same financial year)";
                }
                else if (idt > 0)
                {
                    msg = "Bill Date Cannot be greater then Current Date!!!";
                }
                else if (model.BpoType == "R" && model.Au == "" && !(model.BpoRly == "RCF" || model.BpoRly == "ICF" || model.BpoRly == "RWF"))
                {
                    msg = "AU Cannot be Blank For Railways Bills, Kindly Update the AU for the BPO and then Generate the Bill!!!";
                }
                else
                {
                    model.UserId = Convert.ToString(UserId);

                    i = inpsRepository.BillUpdate(model, Region);
                    msg = "Update Successfully.";
                }
                if (model.UpdateStatus == "-1")
                {
                    msg = "Fee Details not available.";
                }
                else if (model.UpdateStatus == "-2")
                {
                    msg = "MINIMUM FEE PAYBLE IS GREATER THEN MAXIMUM FEE.";
                }
                else if (model.UpdateStatus == "-3")
                {
                    msg = "Unable to access Bill Master.";
                }
                else if (model.UpdateStatus == "-4")
                {
                    msg = "Unable to Insert New Bill No. in Bill Master.";
                }
                else if (model.UpdateStatus == "-5")
                {
                    msg = "Invalid Bill No. Passed as Parameter.";
                }
                else if (model.UpdateStatus == "-6")
                {
                    msg = "Unable to Insert Bill Details.";
                }
                else if (model.UpdateStatus == "-7")
                {
                    msg = "Error occured during updating Fee Details in Bill Master.";
                }
                else if (model.UpdateStatus == "-8")
                {
                    if (Convert.ToInt32(certdt) >= 20170701)
                    {
                        msg = "Unable to Select GST Tax Rates";
                    }
                    else
                    {
                        msg = "Unable to Select Service Tax Rates";
                    }
                }
                else
                {
                    if (model.TIFee < 1)
                    {
                        msg = "Zero Value BILL!!!";
                    }
                }
                if (msg != "")
                {
                    if (msg == "Update Successfully.")
                    {
                        if (i != "")
                        {
                            return Json(new { status = true, responseText = msg, Id = i });
                        }
                        else
                        {
                            msg = "This Call cannot be deleted. because IC is present for this call!!!";
                            return Json(new { status = false, responseText = msg, Id = i });
                        }
                    }
                    else
                    {
                        return Json(new { status = true, responseText = msg, Id = i });
                    }
                }

            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "InspectionCert", "BillUpdate", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpPost]
        public IActionResult BillDateUpdate(string BillNo, DateTime BillDt)
        {
            try
            {
                InspectionCertModel model = new();
                string i = "";
                string msg = "Update Successfully.";
                model.BillDt = BillDt;
                model.BillNo = BillNo;
                string myYear, myMonth, myDay;
                myYear = Convert.ToString(model.BillDt).Substring(6, 4);
                myMonth = Convert.ToString(model.BillDt).Substring(3, 2);
                myDay = Convert.ToString(model.BillDt).Substring(0, 2);
                //string dt1 = myYear + myMonth + myDay;
                string dt1 = Convert.ToDateTime(model.BillDt).ToString("dd/MM/yyyy");
                int idt = dt1.CompareTo(DateTime.Now.Date.ToString("dd/MM/yyyy"));
                model.Regioncode = Region;

                if (idt > 0)
                {
                    AlertDanger("Bill Date Cannot be greater then Current Date!!!");
                }
                else
                {
                    model.UserId = Convert.ToString(UserId);
                    i = inpsRepository.BillDateUpdate(model, Region);
                }
                if (i == "1")
                {
                    msg = "Bill Date Has Modified!!!";
                }
                else if (i == "2")
                {
                    msg = "The Date You are Modifing should lie in the same financial year!!!";
                }
                else
                {
                    msg = "Old Bill Date is not allowed!!!";
                }
                if (i != "")
                {
                    return Json(new { status = true, responseText = msg });
                }
                return Json(new { status = false, responseText = msg });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "InspectionCert", "BillDateUpdate", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpPost]
        public IActionResult EditListDetails(string CaseNo, DateTime CallRecvDt, int CallSno, int ItemSrnoPo)
        {
            try
            {
                InspectionCertModel model = new InspectionCertModel();
                if (CaseNo != null && CallSno > 0 && ItemSrnoPo > 0)
                {
                    model = inpsRepository.FindByItemID(CaseNo, CallRecvDt, CallSno, ItemSrnoPo, Region);
                }
                return PartialView("_EditListDetails", model);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "InspectionCert", "EditListDetails", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpPost]
        public IActionResult UpdateCallDetails(InspectionCertModel model, int ItemSrnoPo, string CaseNo, DateTime CallRecvDt, int CallSno)
        {
            try
            {
                string id = "";
                string msg = "Item Description Updated Successfully.";
                if (ItemSrnoPo > 0 && CaseNo != null && CallSno > 0)
                {
                    id = inpsRepository.UpdateCallDetails(model, ItemSrnoPo, CaseNo, CallRecvDt, CallSno);
                }

                if (id != null)
                {
                    return Json(new { status = true, responseText = msg });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "InspectionCert", "UpdateCallDetails", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        public IActionResult PopUp(string BillNo)
        {
            try
            {
                ICPopUpModel model = new ICPopUpModel();
                if (BillNo != null)
                {
                    model = inpsRepository.FindByBillDetails(BillNo, Region);
                }
                return PartialView("_PopUp", model);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "InspectionCert", "PopUp", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpPost]
        public IActionResult DocumentSave(string BillNo, string FrmCollection)
        {
            try
            {
                string msg = "Please select a file to upload.";

                if (BillNo != null)
                {
                    #region File Upload Invoice Supp Docs
                    if (!string.IsNullOrEmpty(FrmCollection))
                    {
                        int[] DocumentIds = { (int)Enums.DocumentCategory.ICDocument };
                        List<APPDocumentDTO> DocumentsList = JsonConvert.DeserializeObject<List<APPDocumentDTO>>(FrmCollection);
                        DocumentHelper.SaveFiles(BillNo, DocumentsList, Enums.GetEnumDescription(Enums.FolderPath.ICDocument), env, iDocument, string.Empty, BillNo + ".pdf", DocumentIds);

                        msg = "The file has been uploaded.";
                        string i = inpsRepository.DocUpdate(BillNo, Convert.ToString(UserId));
                    }
                    #endregion
                }
                return Json(new { status = true, responseText = msg });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "InspectionCert", "ICDocument", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        public IActionResult CallMaterialReadiness(string CaseNo, DateTime CallRecvDt, int CallSno, string FirstInspDt)
        {
            InspectionCertModel model = new();
            try
            {
                if (CaseNo != null && CallSno > 0)
                {
                    model = inpsRepository.FindByCallMaterialReadiness(CaseNo, CallRecvDt, CallSno, Region);
                    if (model.DtInspDesire == null)
                    {
                        AlertDanger("Material Readiness Date in the Call is Missing, Kindly Update it in the Call before using this option!!!");
                    }
                    else
                    {
                        string myYear, myMonth, myDay;
                        myYear = Convert.ToString(FirstInspDt).Substring(6, 4);
                        myMonth = Convert.ToString(FirstInspDt).Substring(3, 2);
                        myDay = Convert.ToString(FirstInspDt).Substring(0, 2);
                        string dt = myYear + myMonth + myDay;

                        string myYear1, myMonth1, myDay1;
                        myYear1 = Convert.ToString(model.DtInspDesire).Substring(6, 4);
                        myMonth1 = Convert.ToString(model.DtInspDesire).Substring(3, 2);
                        myDay1 = Convert.ToString(model.DtInspDesire).Substring(0, 2);
                        string dt1 = myYear1 + myMonth1 + myDay1;
                        int i = dt1.CompareTo(dt);
                        if (i > 0)
                        {
                            AlertDanger("Call/Material Readiness Date Cannot be greater then First Inspection Date!!!");
                        }
                        else
                        {
                            model.CallDt = model.DtInspDesire;
                        }
                    }
                }
                return View(model);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "InspectionCert", "CallMaterialReadiness", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        public IActionResult ChangeConsignee(string CaseNo, string Bkno, string Setno, string ActionType)
        {
            InspectionCertModel model = new InspectionCertModel();
            if (CaseNo != null && Bkno != null && Setno != null && ActionType != null)
            {
                model = inpsRepository.GetChangeConsigneeDetails(CaseNo, Bkno, Setno, ActionType, Region);
            }
            return View(model);
        }
        //Save ChangeConsignee
        [HttpPost]
        public IActionResult ChangeConsignee(InspectionCertModel model)
        {
            try
            {
                if (model.Caseno != null && model.Bkno != null && model.Setno != null && model.ConsigneeCd > 0)
                {
                    model.Updatedby = UserId;
                    model.UserId = USER_ID.Substring(0, 8);
                    model.Regioncode = Region;
                    inpsRepository.SaveChangeConsignee(model);
                    if (model.UpdateStatus == "1")
                    {
                        AlertAddSuccess("Record Updated Successfully.");
                    }
                    else
                    {
                        AlertDanger("The IC For the Given Case No, Call Date, Call SNo. and Consignee CD Already Present.So You Cannot update the Consignee!!!");
                    }
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "InspectionCert", "ChangeConsignee", 1, GetIPAddress());
            }
            return View(model);
        }

        public IActionResult Returned_Bills_BPO_Change_Form(string CaseNo, string Bkno, string Setno, string ActionType)
        {
            InspectionCertModel model = new InspectionCertModel();
            if (CaseNo != null && Bkno != null && Setno != null && ActionType != null)
            {
                model = inpsRepository.GetReturned_Bills_ChangesDetails(CaseNo, Bkno, Setno, ActionType, Region);
            }

            return View(model);
        }

        //Save Returned_Bills_BPO_Change_Form
        [HttpPost]
        public IActionResult Returned_Bills_BPO_Change_Form(InspectionCertModel model)
        {
            try
            {
                if (model.Caseno != null && model.Bkno != null && model.Setno != null && model.BillNo != null)
                {
                    model.Updatedby = UserId;
                    model.UserId = USER_ID.Substring(0, 8);
                    model.Regioncode = Region;
                    inpsRepository.SaveReturned_Bills_Changes(model);
                    if (model.UpdateStatus == "1")
                    {
                        AlertAddSuccess("Record Updated Successfully.");
                    }
                    else
                    {
                        AlertDanger("Kindly Select the BPO OR old Recipient GST No. does not match with new GST No. !!!");
                    }
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "InspectionCert", "ChangeConsignee", 1, GetIPAddress());
            }
            return View(model);
        }

        public IActionResult GetTaxTypeList(string StateCode)
        {
            if (StateCode == "7")
            {
                return Json(Common.GetTaxType_GST_07());
            }
            else
            {
                return Json(Common.GetTaxType_GST_O());
            }

        }
    }
}
