using IBSAPI.Interfaces;
using IBSAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBSAPI.Controllers
{
    public class ClientController : Controller
    {
        #region Variable
        private readonly IVendorRepository vendorRepository;
        #endregion

        public ClientController(IVendorRepository _vendorRepository)
        {
            vendorRepository = _vendorRepository;
        }

        [HttpGet("GetCaseDetailsforClient", Name = "GetCaseDetailsforClient")]
        public IActionResult GetCaseDetailsforClient(string userID, string CaseNo, DateTime? CallRecvDt, string CallStage)
        {
            try
            {
                VenderCallRegisterModel model = new();
                string ActionType = "A";
                string msg = "";
                if (CaseNo != null)
                {
                    model = vendorRepository.FindByAddDetails(CaseNo, CallRecvDt, CallStage, Convert.ToInt32(userID));
                    if (model.OnlineCallStatus == "Y")
                    {
                        if (model.InspectingAgency == "R")
                        {
                            if (model.PoOrLetter == "P")
                            {
                                if (model.PendingCharges > 0)
                                {
                                    msg="Call Cancellation/Rejection charges are pending, Kindly submit the pending charges before submitting the call.";
                                }
                                else
                                {
                                    //string check = model.VendCd;

                                    string check = vendorRepository.GetMatch(CaseNo, userID);
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
                                                    msg = "Please ensure Inspection Call is submitted at least five(5) working days before the expiry of the delivery period , otherwise Call shall not be accepted.";
                                                }
                                                else if (dp == "2")
                                                {
                                                    msg = "Delivery Period not available, so Call shall not be accepted.";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            msg = "Call is already registered against given Case No. and the Call Status is still Pending, so New Call shall not be accepted.";
                                        }
                                    }
                                    else if (check == "0")
                                    {
                                        msg = "No Record Present for the Given Case No.!!! ";
                                    }
                                    else
                                    {
                                        msg = "You are not Authorised to Add The Call For Other Vendors.!!! ";
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
                                        msg = "Please ensure Inspection Call is submitted at least five(5) working days before the expiry of the delivery period , otherwise Call shall not be accepted.";
                                    }
                                    else if (dp == "2")
                                    {
                                        msg = "Delivery Period not available, so Call shall not be accepted.";
                                    }
                                }
                            }
                            else
                            {
                                msg = "Online Call cannot be registered as Purchase Order OR Letter of Offer is Blank.";
                            }
                        }
                        else
                        {
                            if (model.InspectingAgency == "C")
                            {
                                if (model.Remarks == "")
                                {
                                    msg = "RITES is not the Inspection Agency for this CASE.";
                                }
                                else
                                {
                                    msg = "RITES is not the Inspection Agency for this CASE. Kindly see the comments below : " + "\\n" + model.Remarks.Trim();
                                }
                            }
                            else if (model.InspectingAgency == "X")
                            {
                                if (model.Remarks == "")
                                {
                                    msg = "Railways has cancelled the PO for this CASE.";
                                }
                                else
                                {
                                    msg = "Railways has cancelled the PO for this CASE. Kindly see the comments below : " + "\\n" + model.Remarks.Trim();
                                }
                            }
                            else if (model.InspectingAgency == "S")
                            {
                                if (model.Remarks == "")
                                {
                                    msg = "RITES has Suspended the Inspection against this PO.";
                                }
                                else
                                {
                                    msg = "RITES has Suspended the Inspection against this PO. Kindly see the comments below : " + "\\n" + model.Remarks.Trim();
                                }
                            }
                        }
                    }
                    else
                    {
                        msg = "Please Your Call are Register in Online.";
                    }
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                        message = "Successfully",
                        data = model
                    };
                    return Ok(response);
                }
                else
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                        message = "No Data Found",
                    };
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Vendor_API", "GetCaseDetailsforvendor", 1, string.Empty);
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                    message = ex.Message.ToString(),
                };
                return Ok(response);
            }
        }
    }
}
