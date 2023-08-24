using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Dynamic;
using System.Globalization;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;

namespace IBS.Controllers.InspectionBilling
{
    [Authorization]
    public class HologramAccountalController : BaseController
    {

        #region Variables
        private readonly IHologramAccountalRepository hologramaccountRepository;
        #endregion
        public HologramAccountalController(IHologramAccountalRepository _hologramaccountalRepository)
        {
            hologramaccountRepository = _hologramaccountalRepository;
        }

        [Authorization("HologramAccountal", "Index", "view")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            var dTResult = new DTResult<HologramAccountalModel>();
            var Region = GetUserInfo.Region.ToString();
            try
            {
                if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["BK_NO"]) || !string.IsNullOrEmpty(dtParameters.AdditionalValues["SET_NO"]))
                {
                    dTResult = hologramaccountRepository.GetHologramAcountList(dtParameters, Region);
                }
                else
                {
                    var list = new List<HologramAccountalModel>();
                    dTResult.data = list;
                    dTResult.draw = 1;
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "HologramAccountal", "LoadTable", 1, GetIPAddress());
            }
            return Json(dTResult);
        }

        public JsonResult GetRegionCode()
        {
            var result = string.Empty;
            var region = GetUserInfo.Region.ToString();

            var BNO = Convert.ToString(Request.Query["BK_NO"]);
            var SNO = Convert.ToString(Request.Query["SET_NO"]);

            result = hologramaccountRepository.GetRegionCode(BNO, SNO, region);

            if (result == "")
            {
                result = "";
            }
            else if (result == region)
            {
                result = "2";
            }
            else
            {
                result = "1";
            }
            return Json(result);
        }

        [Authorization("HologramAccountal", "Index", "view")]
        public IActionResult Manage()
        {
            var obj = new HologramAccountalFilter();
            var detail = new HologramAccountalModel();
            var region = Convert.ToString(GetUserInfo.Region);

            if (region == null)
            {
                return RedirectToAction("Index", "Home");
            }
            try
            {
                string BNO = "", SNO = "";
                int CCD = 0, RNO = 0;

                if (Convert.ToString(Request.Query["BK_NO"]) == "" && Convert.ToString(Request.Query["SET_NO"]) == "" && Convert.ToString(Request.Query["CASE_NO"]) == "")
                {
                    BNO = "";
                    SNO = "";
                    RNO = 0;
                }
                else if (Convert.ToString(Request.Query["BK_NO"]) != "" && Convert.ToString(Request.Query["SET_NO"]) != null)
                {
                    BNO = Convert.ToString(Request.Query["BK_NO"]);
                    SNO = Convert.ToString(Request.Query["SET_NO"]);
                    RNO = 0;
                }
                else if (Convert.ToString(Request.Query["CASE_NO"]) != "")
                {
                    detail.CASE_NO = Convert.ToString(Request.Query["CASE_NO"]);
                    detail.CALL_DT = Convert.ToString(Request.Query["CALL_RECV_DT"]);
                    detail.CALL_SNO = Convert.ToString(Request.Query["CALL_SNO"]);
                    CCD = Convert.ToInt32(Request.Query["CONSIGNEE_CD"]);
                    RNO = 0;
                    if (Convert.ToString(Request.Query["REC_NO"]) != "" && Convert.ToString(Request.Query["REC_NO"]) != null)
                    {
                        RNO = Convert.ToInt32(Request.Query["REC_NO"]);
                    }
                }

                ViewData["BNO"] = BNO;
                ViewBag.BNO = BNO;
                ViewBag.SNO = SNO;
                ViewBag.CCD = CCD;
                ViewBag.RNO = RNO;
                ViewBag.IsDelete = true;


                if (Convert.ToString(Request.Query["BK_NO"]) == "" && Convert.ToString(Request.Query["SET_NO"]) == "" && Convert.ToString(Request.Query["CASE_NO"]) == "")
                {
                    BNO = "";
                    SNO = "";
                }
                else if (Convert.ToString(Request.Query["BK_NO"]) != "" && Convert.ToString(Request.Query["SET_NO"]) != null)
                {
                    //show();
                    obj.BK_NO = BNO;
                    obj.SET_NO = SNO;
                    obj.REGION = region;
                    detail = hologramaccountRepository.GetHologramAccountalDetail(obj);
                }
                else if (Convert.ToString(Request.Query["CASE_NO"]) != "")
                {
                    if (Convert.ToString(Request.Query["BK_NO"]) == "" || Convert.ToString(Request.Query["BK_NO"]) == null)
                    {
                        obj.CASE_NO = Convert.ToString(Request.Query["CASE_NO"]);
                        obj.CALL_DT = Convert.ToString(Request.Query["CALL_RECV_DT"]);
                        obj.CONSIGNEE_CD = Convert.ToString(Request.Query["CONSIGNEE_CD"]);
                        obj.CALL_SNO = Convert.ToString(Request.Query["CALL_SNO"]);
                        detail = hologramaccountRepository.GetHologramAccountalDetail(obj);
                    }
                    else if (Convert.ToString(Request.Query["CASE_NO"]) == "" || Convert.ToString(Request.Query["CASE_NO"]) == null)
                    {
                        obj.BK_NO = Convert.ToString(Request.Query["BK_NO"]);
                        obj.SET_NO = Convert.ToString(Request.Query["SET_NO"]);
                        obj.REGION = region;
                        detail = hologramaccountRepository.GetHologramAccountalDetail(obj);
                    }
                    ViewBag.IsDelete = false;

                    //show();
                    //btnDel.Visible = false;
                    if (Convert.ToString(Request.Query["REC_NO"]) != "" && Convert.ToString(Request.Query["REC_NO"]) != null)
                    {
                        detail = hologramaccountRepository.GetSelectedHologramAccountal(detail.CASE_NO, detail.CALL_DT, CCD, Convert.ToInt32(detail.CALL_SNO), RNO);
                        ViewBag.IsDelete = true;
                        //ViewBag.HoloACData = data;
                        //return Json(data);
                        //show2();
                        //btnDel.Visible = true;
                    }
                }
                var bk_no = Request.Query["BK_NO"].ToString();
                var set_no = Request.Query["SET_NO"].ToString();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "HologramAccountal", "Manage", 1, GetIPAddress());
            }
            return View(detail);
        }

        //[Authorization("HologramAccountal", "Index", "add")]
        public IActionResult HologramAccountalDetailsSave(HologramAccountalModel Model)
        {
            var message = "";
            var result = false;
            try
            {
                var userName = Convert.ToString(GetUserInfo.UserName);
                var userID = Convert.ToInt32(GetUserInfo.UserID);
                var region = Convert.ToString(GetUserInfo.Region);
                Model.USER_NAME = userName;
                Model.USER_ID = userID;
                Model.HG_REGION = region;
                message = hologramaccountRepository.CheckDuplicateHologram(Model);
                if (message == "0")
                {
                    var RNO = Convert.ToString(Request.Query["REC_NO"]) != "" ? Convert.ToInt32(Request.Query["REC_NO"]) : 0;
                    if (RNO == 0)
                    {
                        int rec_no = hologramaccountRepository.GetRecNo(Model.CASE_NO, Model.CALL_DT, Convert.ToInt32(Model.CONSIGNEE_CD), Convert.ToInt32(Model.CALL_SNO));
                        if (rec_no > 0)
                        {
                            Model.REC_NO = Convert.ToString(rec_no);
                            result = hologramaccountRepository.HologramInsertUpdate(Model, RNO);
                        }
                        else
                        {
                            message = "Rec no. is missing";
                        }
                    }
                    else if (RNO > 0)
                    {
                        result = hologramaccountRepository.HologramInsertUpdate(Model, RNO);
                    }
                }
                //else
                //{
                //    return Json(message);
                //}
            }
            catch (Exception ex)
            {
                message = "Somthing went wrong";
                Common.AddException(ex.ToString(), ex.Message.ToString(), "HologramAccountal", "HologramAccountalSave", 1, GetIPAddress());
            }
            return Json(new { _msg = message, _result = result });
        }
        
        [Authorization("HologramAccountal", "Index", "delete")]
        public IActionResult HologramAccountalDetailDelete(HologramAccountalModel Model)
        {
            var result = false;
            try
            {
                var RNO = Convert.ToString(Request.Query["REC_NO"]) != "" ? Convert.ToString(Request.Query["REC_NO"]) : null;
                Model.REC_NO = RNO;
                result = hologramaccountRepository.HologramDelete(Model);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "HologramAccountal", "HologramAccountalDetailDelete", 1, GetIPAddress());

            }
            return Json(result);
        }

        public IActionResult Bind_Grid([FromBody] DTParameters dtParameters)
        {
            var dTResult = new DTResult<HologramAccountalModel>();
            var Region = GetUserInfo.Region.ToString();
            try
            {
                dTResult = hologramaccountRepository.GetHologramAccountalDetails(dtParameters);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "HologramAccountal", "Bind_Grid", 1, GetIPAddress());
            }
            return Json(dTResult);
        }

        public IActionResult GetSelectedHologramAccountal(string CaseNo, string CallRecvDt, int CCD, int CallSNo, int RecNo)
        {
            var data = hologramaccountRepository.GetSelectedHologramAccountal(CaseNo, CallRecvDt, CCD, CallSNo, RecNo);

            return Json(data);
        }
    }
}
