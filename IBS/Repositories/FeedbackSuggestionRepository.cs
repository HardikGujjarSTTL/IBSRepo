using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using NuGet.Protocol.Plugins;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Globalization;

namespace IBS.Repositories
{
    public class FeedbackSuggestionRepository : IFeedbackSuggestionRepository
    {
        private readonly ModelContext context;
        private readonly ISendMailRepository pSendMailRepository;

        public FeedbackSuggestionRepository(ModelContext context, ISendMailRepository pSendMailRepository)
        {
            this.context = context;
            this.pSendMailRepository = pSendMailRepository;
        }
        public string SaveFeedback(EmailFeedback model)
        {
            string FeedbackID = "";
            int maxID = 0;
            var Feedback = (from r in context.Feedbacksuggestions where r.Id == Convert.ToInt32(model.Id) select r).FirstOrDefault();
            #region Feedback save
            if (Feedback == null)
            {
                if (context.Feedbacksuggestions.Any())
                {
                    maxID = context.Feedbacksuggestions.Max(x => x.Id) + 1;
                }
                else
                {
                    maxID = 1;
                }
                Feedbacksuggestion obj = new Feedbacksuggestion();
                obj.Id = maxID;
                obj.Name = model.Name;
                obj.Description = model.Description;
                obj.Region = model.ToRegion;
                obj.Email = model.Email;
                obj.Mobileno = model.MobileNo;
                obj.Subject = model.Subject;
                context.Feedbacksuggestions.Add(obj);
                context.SaveChanges();
                FeedbackID = obj.Id.ToString();
                FeedbackEmail(model);
            }
            #endregion
            return FeedbackID;
        }

        public void FeedbackEmail(EmailFeedback model)
        {
            string CCmail = "";
            string tosender = "";
            SendMailModel SendMailModel = new SendMailModel();

            if (model.ToRegion == "N")
            {
                CCmail = "sbu.ninsp@rites.com;qa_feedback@rites.com;";
                tosender = "nrinspn.feedback@rites.com";
            }
            else if (model.ToRegion == "S")
            {
                CCmail = "srinspn_feedback@rites.com";
                tosender = "sbu.sinsp@rites.com;qa_feedback@rites.com;";
            }
            else if (model.ToRegion == "E")
            {
                CCmail = "erinspn_feedback@rites.com";
                tosender = "sbu.einsp@rites.com;qa_feedback@rites.com;";
            }
            else if (model.ToRegion == "W")
            {
                CCmail = "wrinspn_feedback@rites.com";
                tosender = "sbu.winsp@rites.com;qa_feedback@rites.com;";
            }
            else if (model.ToRegion == "C")
            {
                CCmail = "crinspn_feedback@rites.com";
                tosender = "sbu.cinsp@rites.com;qa_feedback@rites.com;";
            }
            else if (model.ToRegion == "Q")
            {
                CCmail = "qa_feedback@rites.com";
                tosender = "sandeepmehra@rites.com;qa_feedback@rites.com;";
            }

            tosender = "hardiksilvertouch007@outlook.com";
            SendMailModel.CC = CCmail;
            SendMailModel.To = tosender;
            SendMailModel.From = model.Email;
            SendMailModel.Subject = model.Subject;
            SendMailModel.Message = model.Description + "\n\n Name: " + model.Name + "\n\n Mobile No. : " +  model.MobileNo;

            bool isSend = pSendMailRepository.SendMail(SendMailModel, null);
        }
    }
}
