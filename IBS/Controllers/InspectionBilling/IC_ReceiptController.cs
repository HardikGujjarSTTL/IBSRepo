using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Globalization;

namespace IBS.Controllers.InspectionBilling
{
    public class IC_ReceiptController : BaseController
    {
        #region Variables
        private readonly IIC_ReceiptRepository iC_ReceiptRepository;
        #endregion

        public IC_ReceiptController(IIC_ReceiptRepository _iC_ReceiptRepository)
        {
            iC_ReceiptRepository = _iC_ReceiptRepository;
        }

        public IActionResult Index()
        {
            ViewBag.IsSave = true;
            ViewBag.IsDelete = true;
            var data = new IC_ReceiptModel();
            string BKNO = "", SNO = "", Action = "";
            var region = Convert.ToString(GetUserInfo.Region);
            if (Convert.ToString(Request.Query["BK_NO"]) == null && Convert.ToString(Request.Query["SET_NO"]) == null)
            {
                BKNO = "";
                SNO = "";
                Action = "A";
            }
            else
            {
                BKNO = Convert.ToString(Request.Query["BK_NO"]);
                SNO = Convert.ToString(Request.Query["SET_NO"]);
                Action = "M";
            }

            //if (!IsPostBack)
            //{
            //fill_IEwhomeIssued();

            var IEStatus = iC_ReceiptRepository.Get_IE_Whome_Issued(region);
            ViewBag.Status = IEStatus;

            var currDate = DateTime.Now.ToString("dd/MM/yyyy").Replace("-", "/"); //current_date();

            if (Action == "A")
            {
                //txtICDT.Text = ss.Substring(0, 10);
                //btnDelete.Visible = false;

                ViewBag.ICDT = currDate.Replace('-', '/');
                data.IC_SUBMIT_DT = currDate;
                ViewBag.IsDelete = false;
            }
            else if (Action == "M")
            {
                //show();
                //btnDelete.Visible = true;


                data = iC_ReceiptRepository.Get_Selected_IC_Receipt(BKNO, SNO, region);
                ViewBag.IsDelete = true;
            }
            data.RDT = currDate;

            //}

            if (Convert.ToString(GetUserInfo.AuthLevl) == "4")
            {
                ViewBag.IsSave = false;
                ViewBag.IsDelete = false;
                //btnSave.Visible = false;
                //btnDelete.Visible = false;
            }

            return View(data);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            var dTResult = new DTResult<IC_Receipt_Grid_Model>();
            var Region = GetUserInfo.Region.ToString();
            try
            {
                dTResult = iC_ReceiptRepository.Get_IC_Receipt(dtParameters, Region);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "IC_Receipt", "LoadTable", 1, GetIPAddress());
            }
            return Json(dTResult);
        }

        [HttpPost]
        public IActionResult Get_Selected_IC_Receipt_Detail(string BK_NO, string SET_NO)
        {
            var data = new IC_ReceiptModel();
            try
            {
                var region = Convert.ToString(GetUserInfo.Region);
                data = iC_ReceiptRepository.Get_Selected_IC_Receipt(BK_NO, SET_NO, region);
                ViewBag.IsDelete = true;
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "IC_Receipt", "Get_Selected_IC_Receipt_Detail", 1, GetIPAddress());
            }

            return Json(data);
        }

        public IActionResult ICReceiptSave(IC_ReceiptModel model)
        {
            int result = 0;
            var msg = "";
            try
            {
                var res = iC_ReceiptRepository.CheckIC(model);
                if (res == 1)
                {
                    result = iC_ReceiptRepository.IC_Receipt_InsertUpdate(model);
                }
                else if (res == 0)
                {
                    msg = "Book No. and Set No. specified is not issued to the Selected Inspection Engineer!!!";
                }
                else if (res == 2)
                {
                    msg = "Bill already generated for given Book and Set No.!!!";
                }
                else if (res == 3)
                {
                    msg = "IC Submit Date Cannot be Less then Issue Date  !!!";
                }
            }
            catch (Exception ex)
            {
                msg = "Somthing went wrong";
                Common.AddException(ex.ToString(), ex.Message.ToString(), "IC_Receipt", "ICReceiptSave", 1, GetIPAddress());
            }
            return Json(msg);
        }

        public IActionResult ICReceiptDelete(IC_ReceiptModel model)
        {
            int result = 0;
            try
            {
                var REGION = Convert.ToString(GetUserInfo.Region);
                model.REGION = REGION;
                model.USER_ID = GetUserInfo.UserID;
                result = iC_ReceiptRepository.IC_Receipt_Delete(model);

            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "IC_Receipt", "ICReceiptDelete", 1, GetIPAddress());
            }
            return Json(result);
        }

        public IActionResult ICFromToDate()
        {
            var action = Request.Query["Action"];
            ViewBag.Action = action;
            var partialView = "IC_Unbilled_Partial";
            if(action == "UNBILLEDIC")
            {
                partialView = "IC_Unbilled_Partial";
            }
            else if(action == "ICISSUEDNSUB")
            {
                partialView = "IC_Issued_Partial";
            }
            ViewBag.PartialView = partialView;
            return View();
        }

        public IActionResult Get_UnBilled_IC([FromBody] DTParameters dtParameters)
        {
            DTResult<ICReportModel> dtList = new();
            try
            {
                var region = GetUserInfo.Region;
                dtList = iC_ReceiptRepository.Get_UnBilled_IC(dtParameters, region);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "IC_Receipt", "Get_UnBilled_IC", 1, GetIPAddress());
            }
            return Json(dtList);
        }

        public IActionResult Get_IC_Issue_Not_Receive([FromBody] DTParameters dtParameters)
        {
            DTResult<ICIssueNotReceiveModel> dtList = new();
            try
            {
                var region = GetUserInfo.Region;
                var username = GetUserInfo.UserName;
                var iccd = Convert.ToString(GetUserInfo.IeCd);
                dtList = iC_ReceiptRepository.Get_IC_Issue_Not_Receive(dtParameters, region, username, iccd);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "IC_Receipt", "Get_UnBilled_IC", 1, GetIPAddress());
            }
            return Json(dtList);
        }

        //public IActionResult IC_Issued_Partial()
        //{

        //    return View();
        //}
    }
}
