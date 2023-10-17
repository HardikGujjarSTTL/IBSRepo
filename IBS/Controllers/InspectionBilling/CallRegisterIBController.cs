﻿using IBS.Helper;
using IBS.Helpers;
using IBS.Interfaces;
using IBS.Interfaces.InspectionBilling;
using IBS.Interfaces.Vendor;
using IBS.Models;
using IBS.Models.Reports;
using IBS.Repositories;
using IBS.Repositories.InspectionBilling;
using IBS.Repositories.Reports;
using IBS.Repositories.Vendor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using System.Dynamic;

namespace IBS.Controllers.InspectionBilling
{
    public class CallRegisterIBController : BaseController
    {
        #region Variables
        private readonly ICallRegisterIBRepository callregisterRepository;
        private readonly IDocument iDocument;
        private readonly IWebHostEnvironment env;
        private readonly IConfiguration _config;

        #endregion
        public CallRegisterIBController(ICallRegisterIBRepository _callregisterRepository, IDocument _iDocumentRepository, IWebHostEnvironment _environment, IConfiguration configuration)
        {
            callregisterRepository = _callregisterRepository;
            iDocument = _iDocumentRepository;
            env = _environment;
            _config = configuration;
        }

        public IActionResult Index(string CaseNo, DateTime? _CallRecvDt, string CallSno)
        {
            VenderCallRegisterModel model = new();
            if (CaseNo != null && _CallRecvDt != null && CallSno != null)
            {
                model = callregisterRepository.FindByID(CaseNo, _CallRecvDt, CallSno, GetRegionCode);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<VenderCallRegisterModel> dTResult = callregisterRepository.GetDataList(dtParameters, GetRegionCode);
            return Json(dTResult);
        }

        public IActionResult GetModifyClick(string CaseNo, string CallRecvDt, int CallSno)
        {
            DTResult<VenderCallRegisterModel> dTResult = callregisterRepository.FindByModifyDetail(CaseNo, CallRecvDt, CallSno, GetRegionCode);
            return Json(dTResult);
        }

        public IActionResult GetMatch(string CaseNo, string CallRecvDt, int CallSno)
        {
            DTResult<VenderCallRegisterModel> dTResult = callregisterRepository.FindMatchDetail(CaseNo, CallRecvDt, CallSno, GetRegionCode);
            return Json(dTResult);
        }

        public IActionResult Manage(string CaseNo, string CallRecvDt, int CallSno, string ActionType)
        {
            VenderCallRegisterModel model = new();
            string myYear1 = "", myMonth1 = "", myDay1 = "";

            myYear1 = CallRecvDt.ToString().Substring(6, 4);
            myMonth1 = CallRecvDt.ToString().Substring(3, 2);
            myDay1 = CallRecvDt.ToString().Substring(0, 2);

            string calldt = myYear1 + myMonth1 + myDay1;
            string DocID = CaseNo + "-" + calldt + "-" + CallSno;

            if (ActionType == "A")
            {
                DocID = "New Document";
            }
            List<IBS_DocumentDTO> lstDocument = iDocument.GetRecordsList((int)Enums.DocumentCategory.CallRegistrationDoc, DocID);
            FileUploaderDTO FileUploaderCOI = new FileUploaderDTO();
            FileUploaderCOI.Mode = (int)Enums.FileUploaderMode.Add_Edit;
            FileUploaderCOI.IBS_DocumentList = lstDocument.Where(m => m.ID == (int)Enums.DocumentCategory_AdminUserUploadDoc.CallRegistrationDoc).ToList();
            FileUploaderCOI.OthersSection = false;
            FileUploaderCOI.MaxUploaderinOthers = 5;
            FileUploaderCOI.FilUploadMode = (int)Enums.FilUploadMode.Single;
            ViewBag.CallRegistrationDoc = FileUploaderCOI;

            if (CaseNo != null && CallRecvDt != null)
            {
                model = callregisterRepository.FindByManageID(CaseNo, Convert.ToDateTime(CallRecvDt), CallSno, ActionType, Region);
            }
            if (model.MsgStatus != null)
            {
                AlertDanger(model.MsgStatus);
            }
            return View(model);
        }

        public IActionResult BindCluster()
        {
            return Json(Common.GetCluster(GetRegionCode));
        }

        public IActionResult GetVendorDetails(int MfgCd, string CaseNo)
        {
            //int MfgCd = Convert.ToInt32(UserName);

            DTResult<VenderCallRegisterModel> dTResult = callregisterRepository.FindByVenderDetail1(MfgCd, CaseNo);
            return Json(dTResult);
        }

        [HttpPost]
        public IActionResult DetailsSave(VenderCallRegisterModel model)
        {
            try
            {
                string i = "";
                string msg = "";
                if (model.ActionType == "A")
                {
                    msg = "Record Inserted Successfully.";
                }
                else
                {
                    msg = "Record Update Successfully.";
                }
                model.SetRegionCode = model.RegionCode;
                if (model.CaseNo != null && model.CallRecvDt != null && model.CallSno != null)
                {
                    model.UserId = UserName.Trim();
                    model.Createdby = UserName.Trim();
                    i = callregisterRepository.RegiserCallSave(model);
                }
                if (model.e_status == 1 && model.RejCanCall == null)
                {
                    //Bhavesh Code SMS & Mail Code comment.
                    //if (model.IeCd > 0)
                    //{
                    //    Task<string> smsResult = callregisterRepository.send_IE_smsAsync(model);
                    //    AlertDanger("SMS Send Success...");
                    //}
                    //string emailResult = callregisterRepository.send_Vendor_Email(model);
                    //if (emailResult == "success")
                    //{
                    //    AlertDanger("Mail Send Success...");
                    //}
                }
                if (i != null)
                {
                    if (i == "NoFound")
                    {
                        return Json(new { status = true, responseText = "The Call Letter No. is already present for this Case No.", Id = i });
                    }
                    else
                    {
                        return Json(new { status = true, responseText = msg, Id = i });
                    }

                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "VenderCallRegisterModel", "DetailsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpPost]
        public IActionResult DetailsDelete(VenderCallRegisterModel model)
        {
            try
            {
                string i = "";
                string msg = "Delete Successfully.";
                if (model.CaseNo != null && model.CallRecvDt != null && model.CallSno != null)
                {
                    model.UserId = UserName;
                    model.Createdby = UserName;
                    i = callregisterRepository.RegiserCallDelete(model);
                }
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
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "VenderCallRegisterModel", "DetailsDelete", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpPost]
        public IActionResult UploadDoc(VenderCallRegisterModel model, IFormCollection FrmCollection)
        {
            try
            {
                string myYear1 = "", myMonth1 = "", myDay1 = "";
                myYear1 = model.CallRecvDt.ToString().Substring(6, 4);
                myMonth1 = model.CallRecvDt.ToString().Substring(3, 2);
                myDay1 = model.CallRecvDt.ToString().Substring(0, 2);

                string calldt = myYear1 + myMonth1 + myDay1;
                string DocID = model.CaseNo + "-" + calldt + "-" + model.CallSno;

                string msg = "Please select a file to upload.";
                if (model.CaseNo != null && model.CallRecvDt != null && model.CallSno != null)
                {
                    #region File Upload Profile Picture
                    if (!string.IsNullOrEmpty(FrmCollection["hdnUploadedDocumentList_tab-1"]))
                    {
                        msg = "Document Upload Successfully.";
                        int[] DocumentIds = { (int)Enums.DocumentCategory_AdminUserUploadDoc.CallRegistrationDoc };
                        List<APPDocumentDTO> DocumentsList = JsonConvert.DeserializeObject<List<APPDocumentDTO>>(FrmCollection["hdnUploadedDocumentList_tab-1"]);
                        DocumentHelper.SaveFiles(DocID, DocumentsList, Enums.GetEnumDescription(Enums.FolderPath.CallRegistrationDoc), env, iDocument, "CallRegistrationDoc", string.Empty, DocumentIds);
                    }
                    #endregion
                }
                if (DocID != null)
                {
                    return Json(new { status = true, responseText = msg, Id = DocID });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "VenderCallRegisterModel", "DetailsDelete", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        public IActionResult GetAddDetails(string CaseNo, string CallRecvDt, int CallSno)
        {
            try
            {
                string Client = "";
                string msg ="";
                if (CaseNo != null)
                {
                    var GetData = callregisterRepository.FindAddDetails(CaseNo);
                    Client = GetData.OfflineCallStatus;
                    if (GetData.InspectingAgency == "R")
                    {
                        DateTime inputDateTime = DateTime.ParseExact(CallRecvDt, "dd/MM/yyyy", null);
                        int ss = Math.Sign((DateTime.Now - inputDateTime).TotalDays);
                        if (ss == 1)
                        {
                            var check = callregisterRepository.GetMatch(CaseNo, GetRegionCode);
                            if (check == "2")
                            {
                                string code = CaseNo;
                                string dt = CallRecvDt;
                                if (GetData.VendInspStopped == "Y")
                                {
                                    var w_itemBlocked = GetData.VendInspStopped;
                                    msg = "Some Items of the Vendor have been blocked due to following reasons :\\n" + GetData.VendRemarks + "\\nDo You Still Want to Register/Update This Call?";
                                    return Json(new { status = true, responseText = msg, code, dt, CallSno, w_itemBlocked, Client = Client });
                                }
                                else
                                {
                                    if (GetData.RlyNonrly == "R" || GetData.RlyNonrly == "U")
                                    {
                                        int dp = callregisterRepository.show2(CaseNo, CallRecvDt, CallSno);
                                        if (dp == 0)
                                        {
                                            msg = "Please ensure Inspection Call is submitted at least five(5) working days before the expiry of the delivery period , otherwise Call shall not be accepted.";
                                            return Json(new { status = true, responseText = msg, code, dt, CallSno, Client = Client });
                                        }
                                        else if (dp == 2)
                                        {
                                            msg = "Delivery Period not available, so Call shall not be accepted.";
                                            return Json(new { status = true, responseText = msg, code, dt, CallSno, Client = Client });
                                        }
                                        else
                                        {
                                            return Json(new { status = true, responseText = "", code, dt, CallSno, w_itemBlocked = "N", Client = Client });
                                        }
                                    }
                                    else
                                    {
                                        return Json(new { status = true, responseText = "", code, dt, CallSno, w_itemBlocked = "N", Client = Client });
                                    }
                                }
                            }
                            else if (check == "0")
                            {
                                msg = "No Record Present for the Given Case No.!!! ";
                            }
                            else
                            {
                                msg= "You are not Authorised to Add The Call For Other Regions.!!! ";
                            }
                        }
                        else
                        {
                            msg = "The Call Date Cannot be greater then Current Date.";
                        }
                    }
                    else
                    {
                        if (GetData.InspectingAgency == "C")
                        {
                            if (GetData.Remarks == "")
                            {
                                msg = "RITES is not the Inspection Agency for this CASE.";
                            }
                            else
                            {
                                msg = "RITES is not the Inspection Agency for this CASE. Kindly see the comments below : " + "\\n" + GetData.Remarks;
                            }
                        }
                        else if (GetData.InspectingAgency == "X")
                        {
                            if (GetData.Remarks == "")
                            {
                                msg = "Railways has cancelled the PO for this CASE.";
                            }
                            else
                            {
                                msg = "Railways has cancelled the PO for this CASE. Kindly see the comments below : " + "\\n" + GetData.Remarks;
                            }
                        }
                        else if (GetData.InspectingAgency == "S")
                        {
                            if (GetData.Remarks == "")
                            {
                                msg = "RITES has Suspended the Inspection against this PO.";
                            }
                            else
                            {
                                msg = "RITES has Suspended the Inspection against this PO. Kindly see the comments below : " + "\\n" + GetData.Remarks;
                            }
                        }
                    }
                }
                return Json(new { status = true, responseText = "", code = CaseNo, dt = CallRecvDt, CallSno = CallSno, w_itemBlocked = "", Client = Client });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "CallRegisterIB", "GetAddDetails", 1, GetIPAddress());
            }

            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        public IActionResult DeleteDetails(string CaseNo, string CallRecvDt, int CallSno)
        {
            try
            {
                if (CaseNo != null && CallRecvDt != null && CallSno > 0)
                {
                    var check = callregisterRepository.GetMatch(CaseNo, GetRegionCode);
                    if (check == "2")
                    {
                        return Json(new { status = true, responseText = "Delete Successfully!!!", CaseNo, CallRecvDt, CallSno, ActionType = "D" });
                    }
                }
                return Json(new { status = true, responseText = "Delete", CaseNo, CallRecvDt, CallSno, ActionType = "D" });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "CallRegisterIB", "DeleteDetails", 1, GetIPAddress());
            }

            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        public IActionResult GetCCancellationDetails(string CaseNo, string CallRecvDt, int CallSno)
        {
            try
            {
                if (CaseNo != null && CallRecvDt != null && CallSno > 0)
                {
                    var check = callregisterRepository.GetMatch(CaseNo, GetRegionCode);
                    var GetCaseNo = callregisterRepository.GetCaseNoFind(CaseNo, CallRecvDt, CallSno);
                    if (check == "2")
                    {
                        if (GetCaseNo == null || GetCaseNo == "")
                        {
                            return Json(new { status = true, responseText = "Null", CaseNo, CallRecvDt, CallSno, ActionType = "A" });
                        }
                        else
                        {
                            return Json(new { status = true, responseText = "Not Null", CaseNo, CallRecvDt, CallSno, ActionType = "M" });
                        }
                    }
                }
                return Json(new { status = true, responseText = "Delete", CaseNo, CallRecvDt, CallSno, ActionType = "D" });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "CallRegisterIB", "GetCCancellationDetails", 1, GetIPAddress());
            }

            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        public IActionResult CallCancellation(string CaseNo, string _CallRecvDt, int CallSno, string ActionType)
        {
            VenderCallCancellationModel model = new();
            if (ActionType == "A")
            {
                if (CaseNo != null && _CallRecvDt != null && CallSno > 0 && ActionType != null)
                {
                    model = callregisterRepository.CancelCallFindByID(CaseNo, _CallRecvDt, CallSno, ActionType);
                }
            }
            if (ActionType == "M")
            {
                if (CaseNo != null && _CallRecvDt != null && CallSno > 0 && ActionType != null)
                {
                    model = callregisterRepository.CancelCallFindByIDM(CaseNo, _CallRecvDt, CallSno, ActionType);
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CallCancellationSave(VenderCallCancellationModel model)
        {
            try
            {
                string i = "";
                string msg = "Insert Successfully.";
                if (model.CaseNo != null && model.CallRecvDt != null && model.CallSno > 0)
                {
                    i = callregisterRepository.CallCancellationSave(model, UserName);
                }
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
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "VenderCallRegisterModel", "DetailsDelete", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        public IActionResult CallCancelDelete(string CaseNo, string CallRecvDt, int CallSno)
        {
            try
            {
                if (CaseNo != null && CallRecvDt != null && CallSno > 0)
                {
                    var check = callregisterRepository.CallCancelDelete(CaseNo, CallRecvDt, CallSno);
                    if (check != "")
                    {
                        return Json(new { status = true, responseText = "Delete Successfully!!!" });
                    }
                }

            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "CallRegisterIB", "CallCancelDelete", 1, GetIPAddress());
            }

            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        public IActionResult CallStatus(string CaseNo, DateTime? CallRecvDt, int CallSno, string IeCd, string ActionType)
        {
            VenderCallStatusModel model = new();
            if (CaseNo != null && CallRecvDt != null && CallSno > 0)
            {
                model = callregisterRepository.FindCallStatus(CaseNo, CallRecvDt, CallSno);
            }
            model.IeCd = IeCd;
            model.ActionType = ActionType;
            return View(model);
        }

        [HttpPost]
        public IActionResult CallStatus(VenderCallStatusModel model)
        {
            try
            {
                if (!string.IsNullOrEmpty(model.CaseNo) && model.CallRecvDt != null && model.CallSno > 0)
                {
                    model.Updatedby = UserName.Substring(0, 8);
                    model.UserId = Convert.ToString(UserId);
                    callregisterRepository.Save(model);
                    AlertAddSuccess("Call Status and Call Update Status has Been Modified!!!");
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "CallRegisterIB", "CallStatusSave", 1, GetIPAddress());
            }
            return View(model);
        }

        public IActionResult CallDetails(string CaseNo, string _CallRecvDt, int CallSno, int ItemSrNoPo)
        {
            VendrorCallDetailsModel model = new();
            if (CaseNo != null && _CallRecvDt != null && CallSno > 0)
            {
                model = callregisterRepository.CallDetailsFindByID(CaseNo, _CallRecvDt, CallSno, ItemSrNoPo);
            }


            return View(model);
            //return View();
        }

        [HttpPost]
        public IActionResult LoadTableCallDetails([FromBody] DTParameters dtParameters)
        {
            DTResult<VendrorCallDetailsModel> dTResult = callregisterRepository.GetCallDetailsList(dtParameters);
            return Json(dTResult);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CallDetailsSave(VendrorCallDetailsModel model)
        {
            try
            {
                string msg = "Inserted Successfully.";
                if (model.CaseNo != null && model.CallRecvDt != null && model.CallSno > 0 && model.ItemSrNoPo > 0)
                {
                    msg = "Updated Successfully.";
                    model.Updatedby = UserName;
                }

                int i = callregisterRepository.CallDetailsSave(model, UserName.Trim());
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


        public IActionResult CallDetailsDelete(VendrorCallDetailsModel model)
        {
            try
            {
                if (callregisterRepository.CallDetailsRemove(model))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "CallRegisterIB", "CallDelete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }


    }
}
