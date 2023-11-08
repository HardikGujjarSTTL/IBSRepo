﻿using IBSAPI.Interfaces;
using IBSAPI.Models;
using IBSAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Dynamic;

namespace IBSAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VendorController : ControllerBase
    {
        #region Variable
        private readonly IVendorRepository vendorRepository;
        #endregion

        public VendorController(IVendorRepository _vendorRepository)
        {
            vendorRepository = _vendorRepository;
        }

        [HttpGet("GetCaseDetailsforvendor", Name = "GetCaseDetailsforvendor")]
        public IActionResult GetCaseDetailsforvendor(int UserID)
        {
            try
            {
                List<CallRegiModel> callRegiModels = vendorRepository.GetCaseDetailsforvendor(UserID);
                if (callRegiModels.Count() > 0)
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                        message = "Successfully",
                        data = callRegiModels
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

        [HttpGet("GetManufacturerList", Name = "GetManufacturerList")]
        public IActionResult GetManufacturerList(string searchValues)
        {
            try
            {
                bool IsDigit = false;
                if (searchValues != null && searchValues != "0")
                {
                    char characterToCheck = searchValues[3];
                    IsDigit = Char.IsDigit(characterToCheck);
                }

                List<ManufacturerModel> agencyClient = new List<ManufacturerModel>();
                if (searchValues != null && searchValues != "0")
                {
                    if (IsDigit)
                    {
                        agencyClient = Common.GetVendorDigit(Convert.ToInt32(searchValues));
                    }
                    else
                    {
                        agencyClient = Common.GetVendorUsingTextAndValues(searchValues);
                    }
                }
                if (agencyClient.Count() > 0)
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                        message = "Successfully",
                        data = agencyClient
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
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Vendor_API", "GetManufacturerList", 1, string.Empty);
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                    message = ex.Message.ToString(),
                };
                return Ok(response);
            }
        }

        [HttpGet("GetItemDetails", Name = "GetItemDetails")]
        public IActionResult GetItemDetails(RequestVenderCallRegisterModel model)
        {
            try
            {
                List<VenderCallRegisterModel> models = vendorRepository.GetVenderListM(model);
                if (models.Count() > 0)
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                        message = "Data get successfully",
                        data = models
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
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Vendor_API", "GetItemDetails", 1, string.Empty);
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                    message = ex.Message.ToString(),
                };
                return Ok(response);
            }
        }

        [HttpPost("UpdateItemDetails", Name = "UpdateItemDetails")]
        public IActionResult UpdateItemDetails(RequestUpdateItemModel models)
        {
            try
            {
                string id = "";
                if (models.ItemSrnoPo > 0 && models.CaseNo != null && models.CallRecvDt != null && models.CallSno > 0)
                {
                    VenderCallRegisterModel model = new();
                    model.CaseNo = models.CaseNo;
                    model.CallRecvDt = models.CallRecvDt;
                    model.CallSno = models.CallSno;
                    model.QtyDue = models.QtyDue;
                    model.QtyToInsp = models.QtyToInsp;
                    model.QtyOrdered = models.QtyOrdered;
                    model.ConsigneeCd = models.ConsigneeCd;
                    model.ItemDescPo = models.ItemDescPo;

                    id = vendorRepository.UpdateCallDetails(model, model.ItemSrnoPo);
                    if (id != null)
                    {
                        var response1 = new
                        {
                            resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                            message = "Item Description Updated Successfully."
                        };
                        return Ok(response1);
                    }
                }
                else
                {
                    var response2 = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                        message = "Please enter Qty Off Now.",
                    };
                    return Ok(response2);
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Vendor_API", "UpdateItemDetails", 1, string.Empty);
            }
            var response = new
            {
                resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                message = "Oops Somthing Went Wrong !!",
            };
            return Ok(response);
        }

        [HttpPost("UpdateManufacturerDetails", Name = "UpdateManufacturerDetails")]
        public IActionResult UpdateManufacturerDetails(RequestUpdateManufacturerDetailsModel models)
        {
            try
            {
                int id = vendorRepository.DetailsInsertUpdate(models);
                if (id > 0)
                {
                    var response1 = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                        message = "Manufacturer Updated Successfully."
                    };
                    return Ok(response1);
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Vendor_API", "UpdateManufacturerDetails", 1, string.Empty);
            }
            var response = new
            {
                resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                message = "Oops Somthing Went Wrong !!",
            };
            return Ok(response);
        }

        [HttpPost("RegisterCall", Name = "RegisterCall")]
        public IActionResult RegisterCall([FromBody] VenderCallRegisterModel model)
        {
            try
            {
                string i = "";
                string msg = "";
                model.RegionCode = model.CaseNo.Substring(0, 1);
                if (model.CaseNo != null && model.CallRecvDt != null)
                {
                    if (model.ActionType == "A")
                    {
                        model = vendorRepository.GetValidate(model);
                        if (model.callval == 0)
                        {
                            //msg = "Master data not entered.So please enter master data cluster/vender/ie";
                            msg = "Call can't be assigned to IE beyond the maximumn call limit.";
                            //return Json(new { status = false, responseText = msg, callval = model.callval });
                            if ((model.RlyNonrly == "R" && model.wMat_value > 1000 && model.desire_dt == 0) || (model.RlyNonrly != "R" && model.wMat_value > 1000 && model.desire_dt == 0 && model.Bpo != "" && model.RecipientGstinNo != ""))
                            {
                                i = vendorRepository.RegiserCallSave(model);
                                if (model.callval == 0)
                                {
                                    msg = "Your Call is Registered, Acknowledgement mail is sent on your registered email-id!!!";
                                }
                                else
                                {
                                    msg = "Your Call is Registered, Acknowledgement mail is sent on your registered email-id!!!.        ";
                                    msg += "Call Marked To:" + model.IE_name;
                                }
                            }
                            else
                            {
                                if (model.RlyNonrly != "R" && model.Bpo == "" && model.RecipientGstinNo == "")
                                {
                                    msg = "Mention the Name, Address and GST No of the party in whose favour invoice is to be raised. It is mandatory in Case of Non Railways Calls!!!";
                                }
                                else if (model.wMat_value < 1000)
                                {
                                    msg = "Sorry, Your Call is not registered as offered material value is less than Rs 1 Thousand!!!";
                                }
                                else if (model.desire_dt > 0)
                                {
                                    msg = "Sorry, Your Call is not registered as Delivery Period is not mentioned or Desire Date should be atleast five(5) days before the expiry of the delivery period!!!";
                                }
                                var response1 = new
                                {
                                    resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                                    message = msg,
                                };
                                return Ok(response1);
                            }
                        }
                        else
                        {
                            if ((model.RlyNonrly == "R" && model.wMat_value > 1000 && model.desire_dt == 0) || (model.RlyNonrly != "R" && model.wMat_value > 1000 && model.desire_dt == 0 && model.Bpo != "" && model.RecipientGstinNo != ""))
                            {
                                i = vendorRepository.RegiserCallSave(model);
                                if (model.callval == 0)
                                {
                                    msg = "Your Call is Registered, Acknowledgement mail is sent on your registered email-id!!!";
                                }
                                else
                                {
                                    msg = "Your Call is Registered, Acknowledgement mail is sent on your registered email-id!!!.        ";
                                    msg += "Call Marked To:" + model.IE_name;
                                }
                            }
                            else
                            {
                                if (model.RlyNonrly != "R" && model.Bpo == "" && model.RecipientGstinNo == "")
                                {
                                    msg = "Mention the Name, Address and GST No of the party in whose favour invoice is to be raised. It is mandatory in Case of Non Railways Calls!!!";
                                }
                                else if (model.wMat_value < 1000)
                                {
                                    msg = "Sorry, Your Call is not registered as offered material value is less than Rs 1 Thousand!!!";
                                }
                                else if (model.desire_dt > 0)
                                {
                                    msg = "Sorry, Your Call is not registered as Delivery Period is not mentioned or Desire Date should be atleast five(5) days before the expiry of the delivery period!!!";
                                }
                                var response2 = new
                                {
                                    resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                                    message = msg,
                                };
                                return Ok(response2);
                            }
                        }
                    }
                    else
                    {
                        i = vendorRepository.RegiserCallSave(model);
                        msg = "Record Update Successfully.";
                        var response3 = new
                        {
                            resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                            message = msg
                        };
                        return Ok(response3);
                    }
                }
                if (i != null)
                {
                    var response4 = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                        message = msg
                    };
                    return Ok(response4);
                }

            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Vendor_API", "RegisterCall", 1, string.Empty);
            }
            var response = new
            {
                resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                message = "Oops Somthing Went Wrong !!",
            };
            return Ok(response);
        }
    }
}
