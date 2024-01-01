using IBS.Interfaces;
using IBS.Interfaces.WebsitePages;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers.WebsitePages
{
    public class VendorFeedbackController : BaseController
    {
        #region Variables
        private readonly IFeedbackRepository feedbackRepository;
        private readonly ISendMailRepository pSendMailRepository;
        private readonly IConfiguration config;
        #endregion
        public VendorFeedbackController(IFeedbackRepository _feedbackRepository, ISendMailRepository _pSendMailRepository, IConfiguration _config)
        {
            feedbackRepository = _feedbackRepository;
            pSendMailRepository = _pSendMailRepository;
            this.config = _config;
        }
        public IActionResult Index()
        {
            VendorFeedbackModel model = new VendorFeedbackModel();
            return View(model);
        }

        [HttpGet]
        public IActionResult GetVenderDetails(int VEND_CD)
        {
            try
            {
                string[] objList = Common.GetVenderDetails(VEND_CD);
                return Json(new { status = true, list = objList });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "VendorFeedback", "GetVenderDetails", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpPost]
        public IActionResult SaveVendorFeedback(VendorFeedbackModel model)
        {
            try
            {
                if (feedbackRepository.CheckAlreadyExist(model))
                {
                    AlertAlreadyExist("Entry Already Exist For Given Vendor Code and Region!!!");
                    return RedirectToAction("Index");
                }
                feedbackRepository.VendorFeedbackSave(model);

                string sender = "";
                if (model.RegionCode == "N")
                {
                    sender = "nrinspn@rites.com";
                }
                else if (model.RegionCode == "W")
                {
                    sender = "wrinspn@rites.com";
                }
                else if (model.RegionCode == "E")
                {
                    sender = "erinspn@rites.com";
                }
                else if (model.RegionCode == "S")
                {
                    sender = "srinspn@rites.com";
                }
                else if (model.RegionCode == "Q")
                {
                    sender = "ritescqa@rites.com";
                }
                bool isSend = false;
                if (Convert.ToString(config.GetSection("MailConfig")["SendMail"]) == "1")
                {
                    SendMailModel sendMailModel = new SendMailModel();
                    // sender for local mail testing
                    sender = "hardiksilvertouch007@outlook.com";
                    //sender = "nrinspn@gmail.com";
                    sendMailModel.From = sender;
                    //sendMailModel.CC = "nrinspn@gmail.com";
                    sendMailModel.To = model.VendEmail;
                    sendMailModel.Subject = "Your Feedback Response - RITES LTD";
                    sendMailModel.Message = "Thank You Sir/Mam for your feedback, we will work as the suggestions given by you to improve our services. \n\n RITES LTD \n QA Division";
                    isSend = pSendMailRepository.SendMail(sendMailModel, null);
                    AlertAddSuccess("Your FeedBack is sent to QA Division for this Month,You can give your feedback in next Month, RITES LTD");
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "VendorFeedback", "Manage", 1, GetIPAddress());
            }
            return View(model);
        }
    }
}
