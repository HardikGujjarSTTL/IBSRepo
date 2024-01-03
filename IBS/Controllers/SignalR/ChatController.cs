using IBS.Filters;
using IBS.Interfaces.Hub;
using IBS.Models;
using IBS.Repositories.Hub;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace IBS.Controllers.SignalR
{    
    [Authorization]
    public class ChatController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IChatRepository _chathub;

        public ChatController(IConfiguration configuration, IChatRepository chathub)
        {
            _configuration = configuration;
            _chathub = chathub;
        }

        public IActionResult Index()
        {
            ChatMessage model = new ChatMessage();
            model = _chathub.GetMessageRecvList(Master_ID);
            model.HostUrl = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host;
            model.msg_recv_ID = model.lstMsg.Select(x => x.msg_recv_ID).FirstOrDefault();
            ViewBag.SenderId = Master_ID;
            return View(model);
        }

        public IActionResult ChatUserReceiver()
        {
            ChatMessage model = new ChatMessage();
            model = _chathub.GetMessageRecvList(Master_ID);
            return PartialView(model);
        }

        public IActionResult ChatMessageHistory(int id)
        {
            ChatMessage model = new ChatMessage();
            model = _chathub.GetMessageList(Master_ID, id);
            model.Master_ID = Master_ID;
            return PartialView(model);
        }
    }
}
