﻿using IBS.Interfaces;
using IBS.Interfaces.WebsitePages;
using IBS.Models;
using IBS.Repositories;
using IBS.Repositories.WebsitePages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IBS.Controllers.WebsitePages
{
    public class ClientFeedbackController : BaseController
    {
        #region Variables
        private readonly IFeedbackRepository feedbackRepository;
        private readonly ISendMailRepository pSendMailRepository;
        #endregion
        public ClientFeedbackController(IFeedbackRepository _feedbackRepository, ISendMailRepository _pSendMailRepository)
        {
            feedbackRepository = _feedbackRepository;
            pSendMailRepository = _pSendMailRepository;
        }
        public IActionResult Index()
        {
            ClientFeedbackModel model = new ClientFeedbackModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult SaveClientFeedback(ClientFeedbackModel model)
        {
            try
            {
                if (feedbackRepository.CheckClientAlreadyExist(model))
                {
                    AlertAlreadyExist("Entry Already Exist For Given Mobile and Region!!!");
                    return RedirectToAction("Index");
                }
                feedbackRepository.ClientFeedbackSave(model);

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
                SendMailModel sendMailModel = new SendMailModel();
                // sender for local mail testing
                sender = "hardiksilvertouch007@outlook.com";
                //sender = "nrinspn@gmail.com";
                sendMailModel.From = sender;
                //sendMailModel.CC = "nrinspn@gmail.com";
                sendMailModel.To = model.Email;
                sendMailModel.Subject = "Your Feedback Response - RITES LTD";
                sendMailModel.Message = "Thank You Sir/Mam for your feedback, we will work as the suggestions given by you to improve our services. \n\n RITES LTD \n QA Division"; 
                bool isSend = pSendMailRepository.SendMail(sendMailModel, null);

                AlertAddSuccess("Your FeedBack is sent to QA Division, RITES LTD");
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
