using IBS.Interfaces.Vendor;
using IBS.Models;
using IBS.Repositories;
using IBS.Repositories.Vendor;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers.Vendor
{
    public class CallRegisterController : BaseController
    {
        #region Variables
        private readonly ICallRegisterRepository callregisterRepository;
        #endregion
        public CallRegisterController(ICallRegisterRepository _callregisterRepository)
        {
            callregisterRepository = _callregisterRepository;
        }

        public IActionResult Index(string CaseNo, string CallRecvDt, string CallSNo)
        {
            CallRegisterModel model = new();

            if (CaseNo != null)
            {
                model = callregisterRepository.FindByID(CaseNo, CallRecvDt, CallSNo, UserName);
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<CallRegisterModel> dTResult = callregisterRepository.GetDataList(dtParameters, UserName);
            return Json(dTResult);
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

        [HttpPost]
        public IActionResult LoadTableDetails([FromBody] DTParameters dtParameters)
        {
            DTResult<CallRegisterModel> dTResult = callregisterRepository.GetCallDetailsList(dtParameters);
            return Json(dTResult);
        }

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
                    model.Updatedby = UserName;
                }

                int i = callregisterRepository.DetailsSave(model,UserName);
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

        
    }
}
