using IBS.Interfaces.Inspection_Billing;
using IBS.Interfaces.Reports;
using Microsoft.AspNetCore.Mvc;
using IBS.Models;
using System.Drawing;

namespace IBS.Controllers.InspectionBilling
{
    public class CallMarkedOnlineController : BaseController
    {
        #region Variables
        private readonly ICallMarkedOnlineRepository callMarkedOnlineRepository;
        private readonly IWebHostEnvironment env;
        private readonly IConfiguration _config;
        #endregion

        public CallMarkedOnlineController(ICallMarkedOnlineRepository _callMarkedOnlineRepository, IWebHostEnvironment _environment, IConfiguration configuration)
        {
            callMarkedOnlineRepository = _callMarkedOnlineRepository;
            env = _environment;
            _config = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Get_Call_Marked_Online([FromBody] DTParameters dtParameters)
        {
            DTResult<CallMarkedOnlineModel> dtList = new();
            try
            {
                dtList = callMarkedOnlineRepository.Get_Call_Marked_Online(dtParameters);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "CallMarkedOnline", "Get_Call_Marked_Online", 1, GetIPAddress());
            }
            return Json(dtList);
        }

        public IActionResult Manage()
        {
            CallMarkedOnlineModel model = new() { 
                CASE_NO = "123456",
                PO_NO = "test po"
            };
            return PartialView("../CallMarkedOnline/Manage", model);
        }

    }
}
