using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    [Authorization]
    public class GeneralMessagesController : BaseController
    {
        #region Variables
        private readonly IGeneralMessageRepository messageRepository;
        #endregion
        public GeneralMessagesController(IGeneralMessageRepository _messageRepository)
        {
            messageRepository = _messageRepository;
        }

        [Authorization("GeneralMessages", "MessageMaster", "view")]
        public IActionResult MessageMaster()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<GeneralMessageModel> dTResult = messageRepository.GetMessageList(dtParameters);
            return Json(dTResult);
        }
        [Authorization("GeneralMessages", "MessageMaster", "delete")]
        public IActionResult Delete(int MessageID)
        {
            try
            {
                if (messageRepository.Remove(MessageID))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "General Message", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("MessageMaster");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorization("GeneralMessages", "MessageMaster", "edit")]
        public IActionResult DetailsSave(GeneralMessageModel model)
        {
            try
            {
                string msg = "Message Inserted Successfully.";

                if (model.MESSAGE_ID > 0)
                {
                    msg = "Message Updated Successfully.";
                    model.Updatedby = Convert.ToString(UserId);
                }
                model.Createdby = Convert.ToString(UserId);
                int i = messageRepository.MessageDetailsInsertUpdate(model);
                if (i > 0)
                {
                    return Json(new { status = true, responseText = msg, Id = i });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "User", "UserDetailsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [Authorization("GeneralMessages", "MessageMaster", "view")]
        public IActionResult MessageManage(int MessageID)
        {
            GeneralMessageModel model = new();

            if (MessageID > 0)
            {
                model = messageRepository.FindByID(MessageID);
            }

            return View(model);
        }
    }
}
