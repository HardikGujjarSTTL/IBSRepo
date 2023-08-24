using IBS.Interfaces.InspectionBilling;
using IBS.Interfaces.Vendor;
using IBS.Models;
using IBS.Repositories.InspectionBilling;
using IBS.Repositories.Vendor;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;

namespace IBS.Controllers.InspectionBilling
{
    public class CallRegisterIBController : BaseController
    {
        #region Variables
        private readonly ICallRegisterIBRepository callregisterRepository;
        #endregion
        public CallRegisterIBController(ICallRegisterIBRepository _callregisterRepository)
        {
            callregisterRepository = _callregisterRepository;
        }

        public IActionResult Index(string CaseNo, string CallRecvDt, string CallSno)
        {
            VenderCallRegisterModel model = new();
            if (CaseNo != null && CallRecvDt != null && CallSno != null)
            {
                model = callregisterRepository.FindByID(CaseNo, CallRecvDt, CallSno, GetRegionCode);
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

        public IActionResult Manage(string CaseNo, string CallRecvDt, int CallSno)
        {
            VenderCallRegisterModel model = new();

            if (CaseNo != null && CallRecvDt != null && CallSno > 0)
            {
                model = callregisterRepository.FindByManageID(CaseNo, CallRecvDt, CallSno, UserName);
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
                    i = callregisterRepository.RegiserCallSave(model);
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
                    if (model.IeCd > 0)
                    {
                        Task<string> smsResult = callregisterRepository.send_IE_smsAsync(model);
                        AlertDanger("SMS Send Success...");
                    }

                    string emailResult = callregisterRepository.send_Vendor_Email(model);
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
                    i = callregisterRepository.RegiserCallDelete(model);
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
    }
}
