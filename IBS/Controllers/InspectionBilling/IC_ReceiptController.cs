using IBS.DataAccess;
using IBS.Filters;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Web;
namespace IBS.Controllers.InspectionBilling
{
    [Authorization]
    public class IC_ReceiptController : BaseController
    {
        #region Variables
        private readonly IIC_ReceiptRepository iC_ReceiptRepository;
        private readonly IWebHostEnvironment env;
        private readonly IConfiguration _config;
        #endregion

        public IC_ReceiptController(IIC_ReceiptRepository _iC_ReceiptRepository, IWebHostEnvironment _environment, IConfiguration configuration)
        {
            iC_ReceiptRepository = _iC_ReceiptRepository;
            env = _environment;
            _config = configuration;
        }

        [Authorization("IC_Receipt", "Index", "view")]
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

            var IEStatus = iC_ReceiptRepository.Get_IE_Whome_Issued(region);
            ViewBag.Status = IEStatus;

            var currDate = DateTime.Now.ToString("dd/MM/yyyy").Replace("-", "/"); //current_date();

            if (Action == "A")
            {                
                ViewBag.ICDT = currDate.Replace('-', '/');
                data.IC_SUBMIT_DT = currDate;
                ViewBag.IsDelete = false;
            }
            else if (Action == "M")
            {               
                data = iC_ReceiptRepository.Get_Selected_IC_Receipt(BKNO, SNO, region);
                ViewBag.IsDelete = true;
            }
            data.RDT = currDate;
            
            if (Convert.ToString(GetUserInfo.AuthLevl) == "4")
            {
                ViewBag.IsSave = false;
                ViewBag.IsDelete = false;                
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorization("IC_Receipt", "Index", "edit")]
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
                msg = "Oops Somthing Went Wrong !!";
                Common.AddException(ex.ToString(), ex.Message.ToString(), "IC_Receipt", "ICReceiptSave", 1, GetIPAddress());
            }
            return Json(msg);
        }

        [HttpPost]
        [Authorization("IC_Receipt", "Index", "delete")]
        public IActionResult ICReceiptDelete(string BK_NO, string SET_NO)
        {
            int result = 0;
            try
            {
                IC_ReceiptModel model = new ();
                model.BK_NO = BK_NO;
                model.SET_NO = SET_NO;
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

        public IActionResult IC_Issued_Partial(string Type)
        {
            ViewBag.Type = Type;            
            return PartialView("IC_Issued_Partial");
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
                
                foreach(var row in dtList.data)
                {                    
                    var tifpath = Path.Combine(env.WebRootPath, "/RBS/CASE_NO/" + row.CASE_NO+ ".TIF");
                    var pdfpath = Path.Combine(env.WebRootPath, "/RBS/CASE_NO/" + row.CASE_NO + ".PDF");
                    row.IsTIF =  System.IO.File.Exists(tifpath) == true ? true : false;
                    row.IsPDF = System.IO.File.Exists(pdfpath) == true ? true : false;
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "IC_Receipt", "Get_UnBilled_IC", 1, GetIPAddress());
            }
            return Json(dtList);
        }


        public IActionResult ICStatus()
        {
            var region = GetUserInfo.Region;
            ViewBag.Region = region;
            return View();
        }

        public IActionResult Get_IC_Status([FromBody] DTParameters dTParameters)
        {
            DTResult<IC_ReceiptModel> dtList = new();
            try
            {
                var region = GetUserInfo.Region;
                dtList = iC_ReceiptRepository.Get_IC_Status(dTParameters, region);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "IC_Receipt", "Get_UnBilled_IC", 1, GetIPAddress());
            }
            return Json(dtList);
        }
    }
}
