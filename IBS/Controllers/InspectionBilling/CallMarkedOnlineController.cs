using IBS.Filters;
using IBS.Interfaces;
using IBS.Interfaces.Inspection_Billing;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IBS.Controllers.InspectionBilling
{
    [Authorization]
    public class CallMarkedOnlineController : BaseController
    {
        #region Variables
        private readonly ICallMarkedOnlineRepository callMarkedOnlineRepository;
        private readonly IWebHostEnvironment env;
        //private readonly IConfiguration _config;
        private readonly ISendMailRepository pSendMailRepository;
        private readonly IConfiguration config;
        #endregion

        public CallMarkedOnlineController(ICallMarkedOnlineRepository _callMarkedOnlineRepository, IWebHostEnvironment _environment, ISendMailRepository _pSendMailRepository, IConfiguration _config)
        {
            callMarkedOnlineRepository = _callMarkedOnlineRepository;
            env = _environment;
            //_config = configuration;
            pSendMailRepository = _pSendMailRepository;
            this.config = _config;
        }


        [Authorization("CallMarkedOnline", "Index", "view")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorization("CallMarkedOnline", "Index", "view")]
        public IActionResult Manage(string CASE_NO, string CALL_RECV_DT, string CALL_SNO, string CHECK_SELECTED, string RUN_DT)
        {
            var region = GetUserInfo.Region;
            var model = new CallMarkedOnlineModel();
            var CNO = CASE_NO;  //Convert.ToString(Request.Query["CASE_NO"]).Trim();
            var DT = CALL_RECV_DT;  //Convert.ToString(Request.Query["CALL_RECV_DT"]).Trim();
            var CSNO = CALL_SNO; //Convert.ToString(Request.Query["CALL_SNO"]).Trim();
            var wchk_val = CHECK_SELECTED; //Convert.ToString(Request.Query["CHECK_SELECTED"]).Trim();
            var wrun_dt = RUN_DT; //Convert.ToString(Request.Query["RUN_DT"]).Trim();

            //bool RDB1 = false, RDB2 = false, RDB3 = false;
            //if (wchk_val == "1")
            //{
            //    RDB1 = true;
            //}
            //else if (wchk_val == "2")
            //{
            //    RDB2 = true;
            //}
            //else if (wchk_val == "3")
            //{
            //    RDB3 = true;
            //}

            var obj = new CallMarkedOnlineFilter();
            obj.CASE_NO = CNO;
            obj.Date = DT;
            obj.CALL_SNO = CSNO;

            model = callMarkedOnlineRepository.Get_Call_Marked_Online_Detail(obj);
            double mat_val = 0;
            var data = callMarkedOnlineRepository.Get_Call_Material_Value(obj);
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    double val = (Convert.ToDouble(item.VALUE.ToString()) / Convert.ToDouble(item.QTY.ToString())) * Convert.ToDouble(item.QTY_TO_INSP.ToString());
                    mat_val = mat_val + val;
                }
            }
            model.CALL_MATERIAL_VALUE = Convert.ToString(Math.Round(mat_val, 2));

            ViewBag.ClusterIEList = callMarkedOnlineRepository.Get_Cluster_IE(region, model.DEPARTMENT_CODE);
            ViewBag.InspectedList = new List<SelectListItem>
            {
                new SelectListItem { Text = "Mechanical", Value = "M" },
                new SelectListItem { Text = "Electrical", Value = "E" },
                new SelectListItem { Text = "Civil", Value = "C" },
                new SelectListItem { Text = "Textiles", Value = "T" },
                new SelectListItem { Text = "M & P", Value = "Z" },
            };
            return View(model);
        }

        public IActionResult Get_Call_Marked_Online([FromBody] DTParameters dtParameters)
        {
            DTResult<CallMarkedOnlineModel> dtList = new();
            try
            {
                var region = Convert.ToString(GetUserInfo.Region);
                dtList = callMarkedOnlineRepository.Get_Call_Marked_Online(dtParameters, region);
            }
            catch (Exception ex)
            {
                dtList.draw = 1;
                Common.AddException(ex.ToString(), ex.Message.ToString(), "CallMarkedOnline", "Get_Call_Marked_Online", 1, GetIPAddress());
            }
            return Json(dtList);
        }

        // Send Mail For Rejection Call
        [HttpPost]
        public IActionResult SendVendorMailForRejectedCall(CallMarkedOnlineModel obj)
        {
            var result = false;
            try
            {
                var Region = GetUserInfo.Region;
                var email = callMarkedOnlineRepository.Send_Vendor_Mail_For_Rejected_Call(obj, Region);
                //if (email == "Success")
                //    AlertUpdateSuccess("Mail send Successfuly");
                //else
                //    AlertDanger("Mail not sent");

                CallMarkedOnlineFilter model = new();
                model.CASE_NO = obj.CASE_NO;
                model.Date = obj.CALL_RECV_DT;
                model.CALL_SNO = obj.CALL_SNO;
                result = callMarkedOnlineRepository.Call_Rejected(model, GetUserInfo);
            }
            catch (Exception ex)
            {
                result = false;
                Common.AddException(ex.ToString(), ex.Message.ToString(), "CallMarkedOnline", "Send_Mail_For_Rejected_Call", 1, GetIPAddress());
            }
            return Json(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorization("CallMarkedOnline", "Index", "edit")]
        public IActionResult Call_Marked_Online_Save(CallMarkedOnlineModel Model)
        {
            var result = false;
            try
            {
                result = callMarkedOnlineRepository.Call_Marked_Online_Save(Model, GetUserInfo);
                if (Convert.ToBoolean(config.GetSection("AppSettings")["SendMail"]) == true)
                {
                    callMarkedOnlineRepository.Send_Vendor_Email(Model, GetUserInfo.Region);
                }
                if (Convert.ToBoolean(config.GetSection("AppSettings")["SendSMS"]) == true)
                {
                    if (Model.IE_NAME.Trim() != "" && Model.IE_NAME.Trim() != null)
                    {
                        var res = callMarkedOnlineRepository.send_IE_smsAsync(Model);
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                Common.AddException(ex.ToString(), ex.Message.ToString(), "CallMarkedOnline", "Call_Marked_Online_Save", 1, GetIPAddress());
            }
            return Json(result);
        }

        public IActionResult CaseHistory(string CASE_NO)
        {
            var model = new CaseHistoryModel();
            try
            {
                DTParameters dTParameters = new DTParameters();
                var Region = GetUserInfo.Region;
                model = callMarkedOnlineRepository.Get_Vendor_Detail_By_CaseNo(CASE_NO, Region);
                model.itemList = callMarkedOnlineRepository.Get_Case_History_Item(dTParameters, CASE_NO, Region);
                model.poIrepsList = callMarkedOnlineRepository.Get_Case_History_PO_IREPS(dTParameters, model.PO_NO, model.PO_DT);
                model.poVendorList = callMarkedOnlineRepository.Get_Case_History_PO_Vendor(dTParameters, CASE_NO);
                model.PrevCallList = callMarkedOnlineRepository.Get_Case_History_Previous_Call(dTParameters, CASE_NO);
                model.ConsingCompList = callMarkedOnlineRepository.Get_Case_History_Consignee_Complaints(dTParameters, model.VEND_CD);
                model.RejectVendorList = callMarkedOnlineRepository.Get_Case_History_Rejection_Vendor_Place(dTParameters, CASE_NO, model.VEND_CD, Region);
                var RegionName = "";
                if (Convert.ToString(Region) == "N") { RegionName = "Northern Region"; }
                else if (Convert.ToString(Region) == "S") { RegionName = "Southern Region"; }
                else if (Convert.ToString(Region) == "E") { RegionName = "Eastern Region"; }
                else if (Convert.ToString(Region) == "W") { RegionName = "Western Region"; }
                else if (Convert.ToString(Region) == "C") { RegionName = "Central Region"; }

                ViewBag.Case_NO = CASE_NO;
                ViewBag.RegionName = RegionName;
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "CallMarkedOnline", "CaseHistory", 1, GetIPAddress());
            }
            return View(model);
        }

        //[HttpPost]
        public IActionResult GetCaseHistoryItem([FromBody] DTParameters dTParameters)
        {
            DTResult<CaseHistoryItemModel> dtList = new();
            try
            {
                var Region = GetUserInfo.Region;
                //dtList = callMarkedOnlineRepository.Get_Case_History_Item(dTParameters, Region);
            }
            catch (Exception ex)
            {
                dtList.draw = 1;
                Common.AddException(ex.ToString(), ex.Message.ToString(), "CallMarkedOnline", "GetCaseHistoryItem", 1, GetIPAddress());
            }
            return Json(dtList);
        }

        [HttpPost]
        public IActionResult GetPoIREPSList([FromBody] DTParameters dTParameters)
        {
            DTResult<CaseHistoryPoIREPSModel> dtList = new();
            try
            {
                var Region = GetUserInfo.Region;
                //dtList = callMarkedOnlineRepository.Get_Case_History_PO_IREPS(dTParameters);
            }
            catch (Exception ex)
            {
                dtList.draw = 1;
                Common.AddException(ex.ToString(), ex.Message.ToString(), "CallMarkedOnline", "GetCaseHistoryItem", 1, GetIPAddress());
            }
            return Json(dtList);
        }

        [HttpPost]
        public IActionResult GetPoVendorList([FromBody] DTParameters dTParameters)
        {
            DTResult<CaseHistoryPoVendorModel> dtList = new();
            try
            {
                var Region = GetUserInfo.Region;
                //dtList = callMarkedOnlineRepository.Get_Case_History_PO_Vendor(dTParameters);
            }
            catch (Exception ex)
            {
                dtList.draw = 1;
                Common.AddException(ex.ToString(), ex.Message.ToString(), "CallMarkedOnline", "GetCaseHistoryItem", 1, GetIPAddress());
            }
            return Json(dtList);
        }

        [HttpPost]
        public IActionResult GetPreviousCallList([FromBody] DTParameters dTParameters)
        {
            DTResult<CaseHistoryPreviousCallModel> dtList = new();
            try
            {
                var Region = GetUserInfo.Region;
                //dtList = callMarkedOnlineRepository.Get_Case_History_Previous_Call(dTParameters);
            }
            catch (Exception ex)
            {
                dtList.draw = 1;
                Common.AddException(ex.ToString(), ex.Message.ToString(), "CallMarkedOnline", "GetPreviousCallList", 1, GetIPAddress());
            }
            return Json(dtList);
        }

        [HttpPost]
        public IActionResult GetConsigneeComplaintsList([FromBody] DTParameters dTParameters)
        {
            DTResult<CaseHistoryConsigneeComplaintModel> dtList = new();
            try
            {
                var Region = GetUserInfo.Region;
                //dtList = callMarkedOnlineRepository.Get_Case_History_Consignee_Complaints(dTParameters);
            }
            catch (Exception ex)
            {
                dtList.draw = 1;
                Common.AddException(ex.ToString(), ex.Message.ToString(), "CallMarkedOnline", "GetPreviousCallList", 1, GetIPAddress());
            }
            return Json(dtList);
        }

        [HttpPost]
        public IActionResult GetCaseHistoryRejectionVendorPlaceList([FromBody] DTParameters dTParameters)
        {
            DTResult<CaseHistoryRejectionVendorPlaceModel> dtList = new();
            try
            {
                var Region = GetUserInfo.Region;
                //dtList = callMarkedOnlineRepository.Get_Case_History_Rejection_Vendor_Place(dTParameters, Region);
            }
            catch (Exception ex)
            {
                dtList.draw = 1;
                Common.AddException(ex.ToString(), ex.Message.ToString(), "CallMarkedOnline", "GetPreviousCallList", 1, GetIPAddress());
            }
            return Json(dtList);
        }

        [HttpPost]
        public IActionResult SendVendorEmailForIncompleteCall(CallMarkedOnlineModel obj)
        {
            var result = 0;
            try
            {
                var Region = GetUserInfo.Region;
                var email = callMarkedOnlineRepository.Send_Vendor_Email_For_Incomplete_Call_Details(obj, Region);
                //if (email == "Success")
                //    AlertUpdateSuccess("Mail send Successfuly");
                //else
                //    AlertDanger("Mail not sent");

                CallMarkedOnlineFilter model = new();
                model.CASE_NO = obj.CASE_NO;
                model.Date = obj.CALL_RECV_DT;
                model.CALL_SNO = obj.CALL_SNO;
                result = callMarkedOnlineRepository.Delete_Incomplete_Call(model, GetUserInfo);
            }
            catch (Exception ex)
            {
                result = 0;
                Common.AddException(ex.ToString(), ex.Message.ToString(), "CallMarkedOnline", "SendVendorEmailForIncompleteCall", 1, GetIPAddress());
            }
            return Json(result);
        }

        public IActionResult BindClusterIE(string DeptName)
        {
            var region = GetUserInfo.Region;
            return Json(callMarkedOnlineRepository.Get_Cluster_IE(region, DeptName));
        }
    }
}
