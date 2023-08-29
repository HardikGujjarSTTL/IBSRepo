using IBS.Interfaces.Inspection_Billing;
using IBS.Interfaces.Reports;
using Microsoft.AspNetCore.Mvc;
using IBS.Models;
using System.Drawing;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;
using static System.Net.Mime.MediaTypeNames;
using System.Net.Mail;
using IBS.Filters;

namespace IBS.Controllers.InspectionBilling
{
    [Authorization]
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


        [Authorization("CallMarkedOnline", "Index", "view")]
        public IActionResult Index()
        {
            var region = GetUserInfo.Region;
            ViewBag.ClusterIEList = callMarkedOnlineRepository.Get_Cluster_IE(region);
            ViewBag.InspectedList = new List<SelectListItem>
            {
                new SelectListItem { Text = "Mechanical", Value = "M" },
                new SelectListItem { Text = "Electrical", Value = "E" },
                new SelectListItem { Text = "Civil", Value = "C" },
                new SelectListItem { Text = "Textiles", Value = "T" },
            };

            if (Convert.ToString(Request.Query["CASE_NO"]) == null || Convert.ToString(Request.Query["CALL_RECV_DT"]) == null)
            {

            }
            else
            {
                var CNO = Convert.ToString(Request.Query["CASE_NO"]).Trim();
                var DT = Convert.ToString(Request.Query["CALL_RECV_DT"]).Trim();
                var CSNO = Convert.ToString(Request.Query["CALL_SNO"]).Trim();
                var wchk_val = Convert.ToString(Request.Query["CHECK_SELECTED"]).Trim();
                var wrun_dt = Convert.ToString(Request.Query["RUN_DT"]).Trim();

                bool RDB1 = false, RDB2 = false, RDB3 = false;
                if (wchk_val == "1")
                {
                    RDB1 = true;
                }
                else if (wchk_val == "2")
                {
                    RDB2 = true;
                }
                else if (wchk_val == "3")
                {
                    RDB3 = true;
                }

                var obj = new CallMarkedOnlineFilter();
                obj.CASE_NO = CNO;
                obj.Date = DT;
                obj.CALL_SNO = CSNO;

                var model = callMarkedOnlineRepository.Get_Call_Marked_Online_Detail(obj);
                double mat_val = 0;                
                var data = callMarkedOnlineRepository.Get_Call_Material_Value(obj);
                if(data.Count > 0)
                {
                    foreach(var item in data)
                    {
                        double val = (Convert.ToDouble(item.VALUE.ToString()) / Convert.ToDouble(item.QTY.ToString())) * Convert.ToDouble(item.QTY_TO_INSP.ToString());
                        mat_val = mat_val + val;
                    }
                }
                model.CALL_MATERIAL_VALUE = Convert.ToString(Math.Round(mat_val, 2));
                if (model != null)
                {
                    return PartialView("../CallMarkedOnline/Manage", model);
                }
            }
            return View();
        }

        public IActionResult Get_Call_Marked_Online([FromBody] DTParameters dtParameters)
        {
            DTResult<CallMarkedOnlineModel> dtList = new();
            try
            {
                var region = Convert.ToString(GetUserInfo.Region);
                dtList = callMarkedOnlineRepository.Get_Call_Marked_Online(dtParameters,region);
            }
            catch (Exception ex)
            {
                dtList.draw = 1;
                Common.AddException(ex.ToString(), ex.Message.ToString(), "CallMarkedOnline", "Get_Call_Marked_Online", 1, GetIPAddress());
            }
            return Json(dtList);
        }

        public IActionResult Send_Mail_For_Rejected_Call(CallMarkedOnlineModel obj)
        {
            var result = false;
            try
            {
                var Region = GetUserInfo.Region;
                string wRegion = "";
                string sender = "";
                if (Convert.ToString(Region) == "N") { wRegion = "NORTHERN REGION \n 12th FLOOR,CORE-II,SCOPE MINAR,LAXMI NAGAR, DELHI - 110092 \n Phone : +918800018691-95 \n Fax : 011-22024665"; sender = "nrinspn@rites.com"; }
                else if (Convert.ToString(Region) == "S") { wRegion = "SOUTHERN REGION \n CTS BUILDING - 2ND FLOOR, BSNL COMPLEX, NO. 16, GREAMS ROAD,  CHENNAI - 600 006 \n Phone : 044-28292807/044- 28292817 \n Fax : 044-28290359"; sender = "srinspn@rites.com"; }
                else if (Convert.ToString(Region) == "E") { wRegion = "EASTERN REGION \n CENTRAL STATION BUILDING(METRO), 56, C.R. AVENUE,3rd FLOOR,KOLKATA-700 012  \n Fax : 033-22348704"; sender = "erinspn@rites.com"; }
                else if (Convert.ToString(Region) == "W") { wRegion = "WESTERN REGION \n 5TH FLOOR, REGENT CHAMBER, ABOVE STATUS RESTAURANT,NARIMAN POINT,MUMBAI-400021 \n Phone : 022-68943400/68943445 <BR>"; sender = "wrinspn@rites.com"; }
                else if (Convert.ToString(Region) == "C") { wRegion = "Central Region"; }

                var model = new CallMarkedOnlineFilter();
                model.CASE_NO = obj.CASE_NO;
                model.Date = obj.CALL_RECV_DT;
                model.CALL_SNO = obj.CALL_SNO;
                var data = callMarkedOnlineRepository.Get_Vendor_For_Send_Mail(model);

                string call_letter_dt = "";
                if (obj.LETTER_DT == "")
                {
                    call_letter_dt = "NIL";
                }
                else
                {
                    call_letter_dt = obj.LETTER_DT;
                }
                //string mail_body = "Dear Sir/Madam,\n\n Call Letter dated:  " + call_letter_dt + " for inspection of material against PO No. - " + lblPONO.Text + " dated - " + lblPODT.Text + ", Case No -  " + txtCaseNo.Text + ", on date: " + txtDtOfReciept.Text + ", at SNo. " + lblCSNO.Text + ". The Call is rejected due to following Reason:- " + txtRejReason.Text + ", so not marked and deleted. Please Resubmit the call after making necessary corrections. \n\n Thanks for using RITES Inspection Services. \n\n" + wRegion + ".";
                string mail_body = "Dear Sir/Madam,\n\n Call Letter dated:  " + obj.LETTER_DT + " for inspection of material against PO No. - " + obj.PO_NO + " dated - " + obj.PO_DT + ", Case No -  " + obj.CASE_NO + ", on date: " + obj.CALL_RECV_DT + ", at SNo. " + obj.CALL_SNO + ". The Call is rejected due to following Reason:- " + obj.REJECT_REASON + ", so not marked and deleted. Please Resubmit the call after making necessary corrections. \n\n Thanks for using RITES Inspection Services. \n\n" + wRegion + ".";
                mail_body = mail_body + "\n\n THIS IS AN AUTO GENERATED EMAIL. PLEASE DO NOT REPLY. USE EMAIL GIVEN IN THE REGION ADDRESS";

                if(data.VEND_CD == data.MFG_CD && data.MANU_MAIL != "")
                {
                    MailMessage mail = new MailMessage();
                    //mail.To = data.MANU_MAIL;
                    //mail.Bcc = "nrinspn@gmail.com";
                    //mail.From = sender;
                    //mail.Subject = "Your Call for Inspection By RITES";
                    //mail.Body = mail_body;
                    //mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "0");    
                    
                    //SmtpMail.SmtpServer = "10.60.50.81";
                    //mail.Priority = MailPriority.High;
                    //SmtpMail.Send(mail);
                }
                else if (data.VEND_CD != data.MANU_MAIL) 
                {
                    MailMessage mail = new MailMessage();
                    //mail.To = manu_mail;
                    //mail.Bcc = "nrinspn@gmail.com";
                    //mail.From = sender;
                    //mail.Subject = "Your Call for Inspection By RITES";
                    //mail.Body = mail_body;
                    //mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "0");    

                    //SmtpMail.SmtpServer = "10.60.50.81";
                    //mail.Priority = MailPriority.High;
                    //SmtpMail.Send(mail);
                }

                result = callMarkedOnlineRepository.Call_Rejected(model);                
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
            }
            catch (Exception ex)
            {
                result = false;
                Common.AddException(ex.ToString(), ex.Message.ToString(), "CallMarkedOnline", "Call_Marked_Online_Save", 1, GetIPAddress());
            }
            return Json(result);
        }

    }
}
