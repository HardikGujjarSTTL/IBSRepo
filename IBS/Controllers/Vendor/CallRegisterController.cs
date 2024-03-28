﻿using IBS.Interfaces.Vendor;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers.Vendor
{
    public class CallRegisterController : BaseController
    {
        #region Variables
        private readonly ICallRegisterRepository callregisterRepository;
        
        public CallRegisterController(ICallRegisterRepository _callregisterRepository)
        {
            callregisterRepository = _callregisterRepository;
        }

        #endregion

        #region View
        public IActionResult Index(string CaseNo, string CallRecvDt, string CallSNo)
        {
            CallRegisterModel model = new();

            if (CaseNo != null && CallRecvDt != null && CallSNo != null)
            {
                model = callregisterRepository.FindByID(CaseNo, CallRecvDt, CallSNo, UserName);
            }

            return View(model);
        }

        public IActionResult Manage(string CaseNo, string CallRecvDt, int CallSNo, int ItemSrNoPo)
        {
            CallRegisterModel model = new();

            if (CaseNo != null && CallRecvDt != null && CallSNo > 0)
            {
                model = callregisterRepository.FindManageByID(CaseNo, CallRecvDt, CallSNo, ItemSrNoPo, UserName);
            }

            return View(model);
        }

        #endregion

        #region Load Data Table
        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<CallRegisterModel> dTResult = callregisterRepository.GetDataList(dtParameters, UserName);
            return Json(dTResult);
        }

        [HttpPost]
        public IActionResult LoadTableDetails([FromBody] DTParameters dtParameters)
        {
            DTResult<CallRegisterModel> dTResult = callregisterRepository.GetCallDetailsList(dtParameters);
            return Json(dTResult);
        }

        #endregion

        #region Other Events
        public IActionResult GetModifyClick(string CaseNo, string CallRecvDt, int CallSNo)
        {
            DTResult<CallRegisterModel> dTResult = callregisterRepository.FindByModifyDetail(CaseNo, CallRecvDt, CallSNo, UserName);
            return Json(dTResult);
        }

        public IActionResult GetMatch(string CaseNo, string CallRecvDt, int CallSNo)
        {
            DTResult<CallRegisterModel> dTResult = callregisterRepository.FindMatchDetail(CaseNo, CallRecvDt, CallSNo, UserName);
            return Json(dTResult);
        }

        #endregion

        #region Data Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DetailsSave(CallRegisterModel model)
        {
            try
            {
                string msg = "Inserted Successfully.";

                if (model.CaseNo != null && model.CallRecvDt != null && model.CallSNo > 0 && model.ItemSrNoPo > 0)
                {
                    msg = "Updated Successfully.";
                    model.Updatedby = Convert.ToString(UserId);
                }

                int i = callregisterRepository.DetailsSave(model, UserId);
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

        #endregion
    }
}
