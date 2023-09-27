using IBS.Helper;
using IBS.Interfaces;
using IBS.Interfaces.Vendor;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Net.NetworkInformation;
using static IBS.Helper.Enums;

namespace IBS.Controllers.Vendor
{
    public class VendorCallRegisterController : BaseController
    {
        #region Variables
        private readonly IVendorCallRegisterRepository venderRepository;
        SessionHelper objSessionHelper = new SessionHelper();
        #endregion
        public VendorCallRegisterController(IVendorCallRegisterRepository _venderRepository)
        {
            venderRepository = _venderRepository;
        }

        public IActionResult VendorCallRegister()
        {
            VenderCallRegisterModel model = new();
            //model = venderRepository.FindByID(UserId);
            model.CDATE = DateTime.Now.ToString("dd/MM/yyyy");
            model.CDAY = DateTime.Now.DayOfWeek.ToString("D");
            model.CTYM = DateTime.Now.ToString("HH24MI");

            if (model.CDAY == "1")
            {
                model.CDATE = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
            }
            if (model.CaseNo == null || model.CallRecvDt == null)
            {
                if (model.CDATE == "27/01/2023")
                {
                    model.CallRecvDt = Convert.ToDateTime("27/01/2023");
                }
                else if (model.CDATE == "15/08/2023")
                {
                    model.CallRecvDt = Convert.ToDateTime("16/08/2023");
                }
                else if (model.CDATE == "02/10/2023")
                {
                    model.CallRecvDt = Convert.ToDateTime("03/10/2023");
                }
                else
                {
                    model.CallRecvDt = Convert.ToDateTime(model.CDATE);
                }
            }
            else
            {

            }

            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<VenderCallRegisterModel> dTResult = venderRepository.GetUserList(dtParameters, UserName);
            return Json(dTResult);
        }

        public IActionResult VendorCallRegisterDetail(string Action, string CaseNo, string CallRecvDt, int CallSno)
        {
            VenderCallRegisterModel model = new();

            if (CaseNo != null && CallRecvDt != null && CallSno > 0)
            {
                model = venderRepository.FindByID(CaseNo, CallRecvDt, CallSno, UserName);
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTableList([FromBody] DTParameters dtParameters)
        {
            DTResult<VenderCallRegisterModel> dTResult = venderRepository.GetVenderList(dtParameters, UserName);
            return Json(dTResult);
        }

        [HttpPost]
        public IActionResult SearchMenufacturer(int id, int MfgCd)
        {
            VenderCallRegisterModel model = new();

            if (MfgCd > 0)
            {
                model = venderRepository.FindByVenderDetail(MfgCd);
            }

            return View(model);
            //return RedirectToAction("VendorCallRegister");
        }

        [HttpPost]
        public IActionResult CheckSameVendor(int id)
        {
            VenderCallRegisterModel model = new();
            int MfgCd = Convert.ToInt32(UserName);
            if (MfgCd > 0)
            {
                model = venderRepository.FindByVenderDetail(MfgCd);
            }

            return View(model);
            //return RedirectToAction("VendorCallRegister");
        }

        public IActionResult GetVendorDetails(int id)
        {
            int MfgCd = Convert.ToInt32(UserName);

            DTResult<VenderCallRegisterModel> dTResult = venderRepository.FindByVenderDetail1(MfgCd);
            return Json(dTResult);
        }

        public IActionResult GetVendorDetailsClick(int id)
        {
            int MfgCd = id;

            DTResult<VenderCallRegisterModel> dTResult = venderRepository.FindByVenderDetail1(MfgCd);
            return Json(dTResult);
        }

        public IActionResult BindVendor(int id)
        {
            return Json(Common.GetVendor_City(id));
        }

        [HttpPost]
        public IActionResult UpdateDetails(VenderCallRegisterModel model)
        {
            try
            {
                string msg = "Message Inserted Successfully.";

                if (model.MfgCd > 0)
                {
                    msg = "Message Updated Successfully.";
                    //model.Updatedby = Convert.ToString(UserId);
                }
                //model.Createdby = Convert.ToString(UserId);

                int i = venderRepository.DetailsInsertUpdate(model);
                if (i > 0)
                {
                    return Json(new { status = true, responseText = msg, Id = i });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Update Manufacturer Details", "VendorCallRegister", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });

        }

        [HttpPost]
        public IActionResult DetailsSave(VenderCallRegisterModel model)
        {
            try
            {
                DateTime? Dt1 = model.CallRecvDt;
                DateTime? Dt2 = model.DtInspDesire;
                int w_no_of_days = 0;
                string i = "";
                string msg = "Message Inserted Successfully.";

                if (model.RegionCode == "Northern")
                {
                    model.SetRegionCode = "N";
                }
                else if (model.RegionCode == "Eastern")
                {
                    model.SetRegionCode = "E";
                }
                else if (model.RegionCode == "Western")
                {
                    model.SetRegionCode = "W";
                }
                else if (model.RegionCode == "Southern")
                {
                    model.SetRegionCode = "S";
                }
                else if (model.RegionCode == "Central")
                {
                    model.SetRegionCode = "C";
                }
                else if (model.RegionCode == "CO QA Division")
                {
                    model.SetRegionCode = "Q";
                }

                if (Dt1.HasValue && Dt2.HasValue)
                {
                    TimeSpan ts = Dt2.Value - Dt1.Value;

                    int differenceInDays = ts.Days;

                    if (differenceInDays > 5)
                    {
                        w_no_of_days = 1;
                    }
                    else
                    {
                        w_no_of_days = 0;
                    }
                }
                if (w_no_of_days == 1)
                {
                    AlertDanger("Expected Date of Inspection cannot be more then 5 days from Call Registration Date!!!");
                }
                else if (model.SetRegionCode == "R" && model.IrfcFunded == "")
                {
                    AlertDanger("Select The project is IRFC Funded [Yes/No] !!!");
                }
                else
                {
                    model.UserId = UserName;
                    model.Createdby = UserName;
                    i = venderRepository.RegiserCallSave(model);
                }
                if ((model.RlyNonrly == "R" && model.wMat_value > 1000 && model.desire_dt == 0) || (model.RlyNonrly != "R" && model.wMat_value > 1000 && model.desire_dt == 0 && model.Bpo != "" && model.RecipientGstinNo != ""))
                {
                    if (model.callval == 0)
                    {
                        AlertDanger("Your Call is Registered, Acknowledgement mail is sent on your registered email-id!!!");
                    }
                    else
                    {
                        AlertDanger("Your Call is Registered, Acknowledgement mail is sent on your registered email-id!!!.Call Marked To:" + model.IE_name);
                    }
                }
                else
                {
                    if (model.RlyNonrly != "R" && model.Bpo == "" && model.RecipientGstinNo == "")
                    {
                        AlertDanger("Mention the Name, Address and GST No of the party in whose favour invoice is to be raised. It is mandatory in Case of Non Railways Calls!!!");
                    }
                    else if (model.wMat_value < 1000)
                    {
                        AlertDanger("Sorry, Your Call is not registered as offered material value is less than Rs 1 Thousand!!!");
                    }
                    else if (model.desire_dt > 0)
                    {
                        AlertDanger("Sorry, Your Call is not registered as Delivery Period is not mentioned or Desire Date should be atleast five(5) days before the expiry of the delivery period!!!");
                    }
                }
                if (model.e_status == 1 && model.callval != 0)
                {
                    //send_IE_sms(callval);
                    //send_Vendor_Email(callval);

                    Task<string> smsResult = venderRepository.send_IE_smsAsync(model);
                    AlertDanger("SMS Send Success...");

                    string emailResult = venderRepository.send_Vendor_Email(model);
                    if (emailResult == "success")
                    {
                        AlertDanger("Mail Send Success...");
                    }
                }


                if (i != null)
                {
                    return Json(new { status = true, responseText = msg, Id = i });
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
                string msg = "Message Delete Successfully.";
                if (model.CaseNo != null && model.CallRecvDt != null && model.CallSno != null)
                {
                    model.UserId = UserName;
                    model.Createdby = UserName;
                    i = venderRepository.RegiserCallDelete(model);
                }
                if (i != null)
                {
                    return Json(new { status = true, responseText = msg, Id = i });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "VenderCallRegisterModel", "DetailsDelete", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        public IActionResult PrintCallletter(string CaseNo, string CallRecvDt, int CallSno)
        {
            VendorCallRegPrintReport model = new();

            if (CaseNo != null && CallRecvDt != null && CallSno != null)
            {
                model = venderRepository.FindByPrintReport(CaseNo, CallRecvDt, CallSno, UserName);
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTableReport([FromBody] DTParameters dtParameters)
        {
            DTResult<VenderCallRegisterModel> dTResult = venderRepository.GetDataListReport(dtParameters);
            return Json(dTResult);
        }

        //public IActionResult GetAddCallDetails(string CaseNo)
        //{
        //    DTResult<VenderCallRegisterModel> dTResult = venderRepository.FindByAddDetails(CaseNo, UserId);
        //    return Json(dTResult);
        //}

        //public IActionResult MAXCALL(string CaseNo,string CallStage)
        //{
        //    return Json(Common.GetMAXCALL(CaseNo, CallStage));
        //}

        [HttpPost]
        public IActionResult VendorCallRegister(string CaseNo, DateTime CallRecvDt, string CallStage)
        {
            VenderCallRegisterModel model = new();
            try
            {

                if (CaseNo != null)
                {
                    model = venderRepository.FindByAddDetails(CaseNo, CallRecvDt, CallStage, UserId);
                    if (model.OnlineCallStatus == "Y")
                    {
                        if (model.InspectingAgency == "R")
                        {
                            if (model.PoOrLetter == "P")
                            {
                                if (model.PendingCharges > 0)
                                {
                                    AlertDanger("Call Cancellation/Rejection charges are pending, Kindly submit the pending charges before submitting the call.");
                                }
                                else
                                {
                                    string check = model.VendCd;
                                    if (check == "2")
                                    {
                                        int cno = model.MaxCount;
                                        if (cno == 0)
                                        {
                                            if (model.RlyNonrly == "R" || model.RlyNonrly == "U")
                                            {
                                                string dp = model.dp;
                                                if (dp == "0")
                                                {
                                                    AlertDanger("Please ensure Inspection Call is submitted at least five(5) working days before the expiry of the delivery period , otherwise Call shall not be accepted.");
                                                }
                                                else if (dp == "2")
                                                {
                                                    AlertDanger("Delivery Period not available, so Call shall not be accepted.");
                                                }
                                                else
                                                {
                                                    return RedirectToAction("VendorCallRegisterDetail?Action=A&CaseNo=" + CaseNo + "&CallRecvDt=" + model.CallRecvDt + "&FOS=" + model.CallStage);
                                                }
                                            }
                                            else
                                            {
                                                return RedirectToAction("VendorCallRegisterDetail?Action=A&CaseNo=" + CaseNo + "&CallRecvDt=" + model.CallRecvDt + "&FOS=" + model.CallStage);
                                            }
                                        }
                                        else
                                        {
                                            AlertDanger("Call is already registered against given Case No. and the Call Status is still Pending, so New Call shall not be accepted.");
                                        }
                                    }
                                    else if (check == "0")
                                    {
                                        AlertDanger("No Record Present for the Given Case No.!!! ");
                                    }
                                    else
                                    {
                                        AlertDanger("You are not Authorised to Add The Call For Other Vendors.!!! ");
                                    }

                                }
                            }
                            else if (model.PoOrLetter == "L")
                            {
                                if (model.RlyNonrly == "R" || model.RlyNonrly == "U")
                                {
                                    string dp = model.dp;
                                    if (dp == "0")
                                    {
                                        AlertDanger("Please ensure Inspection Call is submitted at least five(5) working days before the expiry of the delivery period , otherwise Call shall not be accepted.");
                                    }
                                    else if (dp == "2")
                                    {
                                        AlertDanger("Delivery Period not available, so Call shall not be accepted.");
                                    }
                                    else
                                    {
                                        return RedirectToAction("VendorCallRegisterDetail?Action=A&CaseNo=" + CaseNo + "&CallRecvDt=" + model.CallRecvDt + "&FOS=" + model.CallStage);
                                    }
                                }
                                else
                                {
                                    return RedirectToAction("VendorCallRegisterDetail?Action=A&CaseNo=" + CaseNo + "&CallRecvDt=" + model.CallRecvDt + "&FOS=" + model.CallStage);
                                }
                            }
                            else
                            {
                                AlertDanger("Online Call cannot be registered as Purchase Order OR Letter of Offer is Blank.");
                            }
                        }
                        else
                        {
                            if (model.InspectingAgency == "C")
                            {
                                if (model.Remarks == "")
                                {
                                    AlertDanger("RITES is not the Inspection Agency for this CASE.");
                                }
                                else
                                {
                                    AlertDanger("RITES is not the Inspection Agency for this CASE. Kindly see the comments below : " + "\\n" + model.Remarks.Trim());
                                }
                            }
                            else if (model.InspectingAgency == "X")
                            {
                                if (model.Remarks == "")
                                {
                                    AlertDanger("Railways has cancelled the PO for this CASE.");
                                }
                                else
                                {
                                    AlertDanger("Railways has cancelled the PO for this CASE. Kindly see the comments below : " + "\\n" + model.Remarks.Trim());
                                }
                            }
                            else if (model.InspectingAgency == "S")
                            {
                                if (model.Remarks == "")
                                {
                                    AlertDanger("RITES has Suspended the Inspection against this PO.");
                                }
                                else
                                {
                                    AlertDanger("RITES has Suspended the Inspection against this PO. Kindly see the comments below : " + "\\n" + model.Remarks.Trim());
                                }
                            }
                        }
                    }
                    else
                    {
                        AlertDanger("Please Your Call are Register in Online.");
                    }
                    //return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "VendorCallRegister", "VendorCallRegister", 1, GetIPAddress());
            }
            return View(model);
        }


    }
}
