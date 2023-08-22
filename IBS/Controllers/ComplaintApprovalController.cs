using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class ComplaintApprovalController : BaseController
    {
        #region Variables
        private readonly IComplaintApprovalRepository complaintApprovalRepository ;
        #endregion
        public ComplaintApprovalController(IComplaintApprovalRepository _complaintApprovalRepository)
        {
            complaintApprovalRepository = _complaintApprovalRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<OnlineComplaints> dTResult = complaintApprovalRepository.GetRejComplaints(dtParameters);
            return Json(dTResult);
        }

        public IActionResult Manage(string TEMP_COMPLAINT_ID,string SetNo,string BKNo,string CaseNo)
        {
            OnlineComplaints model = new();

            try
            {
                model = complaintApprovalRepository.FindByID(TEMP_COMPLAINT_ID, SetNo, BKNo, CaseNo);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "OnlineComplaints", "ComplaintsSave", 1, GetIPAddress());
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult RejectComplaint(OnlineComplaints model)
        {
            string msg = "";
            try
            {
                 msg = complaintApprovalRepository.RejectComp(model);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "OnlineComplaints", "ComplaintsSave", 1, GetIPAddress());
            }
            return Json(new { status = true, responseText = msg });
        }
    }
}
