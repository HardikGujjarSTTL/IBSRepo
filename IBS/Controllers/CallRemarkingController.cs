using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Manage(CallRemarkingModel model, IFormCollection formCollection)
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


    }
}
