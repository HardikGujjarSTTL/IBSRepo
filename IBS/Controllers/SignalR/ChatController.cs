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
        private readonly IHubContext<ChatHub1> hubContext;
        private readonly IConfiguration _configuration;
        private readonly IChatRepository _chathub;

        public ChatController(IHubContext<ChatHub1> hubContext, IConfiguration configuration, IChatRepository chathub)
        {
            this.hubContext = hubContext;
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

        public IActionResult ChatMessageHistory(int send_ID, int recv_ID)
        {
            ChatMessage model = new ChatMessage();
            model = _chathub.GetMessageList(send_ID, recv_ID);
            model.Master_ID = Master_ID;
            return PartialView(model);
        }
    }
}
