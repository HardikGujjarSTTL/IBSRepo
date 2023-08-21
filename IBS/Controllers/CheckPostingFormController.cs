using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
//using System.Web.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using JsonResult = Microsoft.AspNetCore.Mvc.JsonResult;

namespace IBS.Controllers
{
    public class CheckPostingFormController : BaseController
    {
        private readonly ICheckPostingFormRepository checkpostingrepository;
        public CheckPostingFormController(ICheckPostingFormRepository _checkpostingrepository )
        {
            checkpostingrepository = _checkpostingrepository;
        }
       

        [HttpPost]
        public IActionResult ChequePost([FromBody] DTParameters dtParameters)
        {
            DTResult<CheckPostingFormModel> dTResult = checkpostingrepository.BillList(dtParameters);
            
            return Json(dTResult);

           
        }

        public IActionResult GetValue(string BankNameDropdown, string CHQ_NO, string CHQ_DATE)
        {
            string region = GetRegionCode;
            CheckPostingHeader dTResult = checkpostingrepository.GetTextboxValues(BankNameDropdown, CHQ_NO, CHQ_DATE , region);
            return Json(dTResult);
                
        }

      
        public IActionResult Index(string BankNameDropdown="", string CHQ_NO = "", string CHQ_DATE = "") 
        {
      
            ViewBag.BankNameDropdown = BankNameDropdown;
            ViewBag.CHQ_NO = CHQ_NO;
            ViewBag.CHQ_DATE = CHQ_DATE;
            return View();
        }

        
        public IActionResult FindByID( string billNo)
        {
            CheckPostingFormModel model = new CheckPostingFormModel();
            if (billNo != "" && billNo != null)
            {
                model = checkpostingrepository.FindByID(billNo);
            }
            return Json(model);





            
        }

        [HttpPost]
        public JsonResult UpdateInfo() {
            try
            {
                CheckPostingFormModel model = new CheckPostingFormModel();
                model.BANK_CD = Convert.ToInt32(Request.Form["BANK_CD"]);
                model.CHQ_DATE = Convert.ToDateTime(Request.Form["CHQ_DATE"]);
                model.CHQ_NO = Convert.ToString(Request.Form["CHQ_NO"]);
                var msg = "";
                var Uname = UserId.ToString();

                if (model.BILL_NO != "")
                {
                    msg = "Data Updated Successfully.";
                    
                }
                string i = checkpostingrepository.UpdateData(model);
                if (i != null || i != "" )
                {
                    return Json(new { status = true, responseText = msg });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "CheckPostingForm", "UpdateInfo", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        public IActionResult btnBillDetailsClick( string RadioBill ) 
        {
            var region = GetRegionCode;
            var result = checkpostingrepository.ChkBillNo(RadioBill, region);
            var Amount_Recieved = (result.AMOUNT_RECIEVED) + (result.TDS) + (result.WRITE_OFF_AMT) + (result.RETENTION_MONEY) + (result.CNOTE_AMT);
            var alert = "";
            if(result == null)
            {
                ViewBag.AlertMessage = "InValid Bill No..!!!";

            }
            else
            {

                if(result.BILL_AMOUNT == Amount_Recieved)
                {
                    ViewBag.AlertMessage = "This Bill has already been cleared!!!";
                    alert = " This Bill has already been cleared!!!";
                }
                else
                {



                }
            }

            return Json(new { status = false, alert = alert ,  responseText = "Oops Somthing Went Wrong !!" }); 
        }
            
        public IActionResult btnInvoiceClick(string RadioInvoice)
        {
            var region = GetRegionCode;
            var result = checkpostingrepository.ChkInvoiceNo(RadioInvoice, region);
            var Amount_recieved = (result.AMOUNT_RECIEVED) + (result.TDS) + (result.WRITE_OFF_AMT) + (result.RETENTION_MONEY) + (result.CNOTE_AMT);
            var alert = "";
            if (result == null)
            {
                ViewBag.AlertMessage = "InValid Bill No..!!!";

            }
            else
            {

                if (result.BILL_AMOUNT == Amount_recieved)
                {
                    ViewBag.AlertMessage = "This Bill has already been cleared!!!";
                    alert = " This Bill has already been cleared!!!";
                }
                else
                {



                }
            }

            return Json(new { status = false, alert = alert, responseText = "Oops Somthing Went Wrong !!" });
        }
    }
}
