using IBS.Helper;
using IBS.Helpers;
using IBS.Interfaces;
using IBS.Interfaces.WebsitePages;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IBS.Controllers
{
    public class FeedbackSuggestionController : BaseController
    {
        private readonly IFeedbackSuggestionRepository feedbackSuggestionRepository;

        public FeedbackSuggestionController(IFeedbackSuggestionRepository _feedbackSuggestionRepository)
        {
            feedbackSuggestionRepository = _feedbackSuggestionRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FeedbackSave(EmailFeedback model)
        {
            try
            {
                string msg = "Message Sent Sucessfully, We will respond you shortly.!!! ";

                string i = feedbackSuggestionRepository.SaveFeedback(model);

                return Json(new { status = true, responseText = msg });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "FeedbackSuggestion", "FeedbackSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
    }
}
