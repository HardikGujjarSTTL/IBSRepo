using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace IBS.Controllers
{
    public class CallRemarkingController : BaseController
    {
        private readonly ICallRemarkingRepository callRemarkingRepository;

        public CallRemarkingController(ICallRemarkingRepository _callRemarkingRepository)
        {
            callRemarkingRepository = _callRemarkingRepository;
        }

        public IActionResult CallRemarkingRequest()
        {
            CallRemarkingModel model = new() { Region = Region };
            return View(model);
        }

        public IActionResult GetIEByCo(string Region, int COCd)
        {
            return Json(Common.GetIEByCo(Region, COCd).ToList());
        }

        public IActionResult GetPendingCallsFromIE(int IeCd)
        {
            int PendingCallsFromIE = callRemarkingRepository.GetPendingCallsFromIE(Region, IeCd);
            return Json(new { status = true, responseText = PendingCallsFromIE });
        }

        [HttpPost]
        public IActionResult LoadTablePendingCalls1([FromBody] DTParameters dtParameters)
        {
            DTResult<PendingCallsListModel> dTResult = callRemarkingRepository.GetPendingCallsList1(dtParameters);
            return Json(dTResult);
        }

        [HttpPost]
        public IActionResult LoadTablePendingCalls2([FromBody] DTParameters dtParameters)
        {
            DTResult<PendingCallsListModel> dTResult = callRemarkingRepository.GetPendingCallsList2(dtParameters);
            return Json(dTResult);
        }

        [HttpPost]
        public IActionResult SaveCallRemarkingRequestDetails(CallRemarkingModel model, IFormCollection formCollection)
        {
            try
            {
                if (formCollection.Keys.Contains("checkedCaseNos"))
                {
                    model.CaseNos = formCollection["checkedCaseNos"];
                }

                model.RemInitBy = USER_ID.Substring(0, 8);

                callRemarkingRepository.SaveDetails(model);

                AlertAddSuccess("Record Updated Successfully.");

                return RedirectToAction("CallRemarkingRequest");
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "CallRemarking", "Manage", 1, GetIPAddress());
            }
            return View("CallRemarkingRequest", model);
        }

        public IActionResult CallRemarkingApproval()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CallRemarkingListForApproval([FromBody] DTParameters dtParameters)
        {
            dtParameters.AdditionalValues.Add("UserId", Convert.ToString(UserId));
            dtParameters.AdditionalValues.Add("Region", Region);
            DTResult<PendingCallsListModel> dTResult = callRemarkingRepository.GetCallRemarkingListForApproval(dtParameters);
            return Json(dTResult);
        }

        public IActionResult Manage(int id)
        {
            CallRemarkingApprovalModel model = new();
            if (id > 0)
            {
                model = callRemarkingRepository.FindCallRemarkingDetailsForApproval(id);
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Manage(CallRemarkingApprovalModel model, IFormCollection formCollection)
        {
            try
            {
                if (formCollection.Keys.Contains("btnStatus"))
                {
                    model.Action = formCollection["btnStatus"];
                }

                if (model.DtInspDesire < DateTime.Now.Date)
                {
                    model.DtInspDesire = DateTime.Now.Date;
                }

                model.UserId = USER_ID.Substring(0, 8);

                if (model.Action == "Approve")
                {
                    callRemarkingRepository.SaveDetails(model);
                    AlertAddSuccess("Record is Approved Successfully and Desire Date is " + model.Display_DtInspDesire + "!!");
                }
                else if (model.Action == "Reject")
                {
                    callRemarkingRepository.SaveDetails(model);
                    AlertAddSuccess("Record is Rejected Successfully!!");
                }
                return RedirectToAction("CallRemarkingApproval");
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "CallRemarking", "Manage", 1, GetIPAddress());
            }
            return View(model);
        }
    }
}
