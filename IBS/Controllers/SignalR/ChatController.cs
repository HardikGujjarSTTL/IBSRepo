using DocumentFormat.OpenXml.Office2010.Excel;
using IBS.Interfaces;
using IBS.Interfaces.Hub;
using IBS.Models;
using IBS.Repositories.Hub;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace IBS.Controllers.SignalR
{
    public class ChatController : BaseController
    {
        private readonly IHubContext<ChatHub> hubContext;
        private readonly IConfiguration _configuration;
        private readonly IChatRepository _chathub;
        public ChatController(IHubContext<ChatHub> hubContext, IConfiguration configuration, IChatRepository chathub)
        {
            this.hubContext = hubContext;
            _configuration = configuration;
            _chathub = chathub;
        }


        public IActionResult Index()
        {
            ChatMessage model = new ChatMessage();
            model = _chathub.GetMessageRecvList(UserName);
            model.HostUrl = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host;
            model.msg_recv_ID = model.lstMsg.Select(x => x.msg_recv_ID).FirstOrDefault();
            return View(model);
        }

        [HttpPost]
        public async Task SendMessage(string user, string message)
        {
            ChatMessage model = new ChatMessage();
            model.msg_send_ID = UserId;
            model.msg_recv_ID = UserId;
            model.send_message = message;
            model.recv_message = message;
            var restl = _chathub.ChatMessageSave(model, UserName, UserId);
            await this.hubContext.Clients.All.SendAsync("messageReceivedFromApi", user, message);
        }

        public IActionResult ChatUserReceiver()
        {
            ChatMessage model = new ChatMessage();
            model = _chathub.GetMessageRecvList(UserName);
            return PartialView(model);
        }

        public IActionResult ChatMessageHistory(int id)
        {
            ChatMessage model = new ChatMessage();
            model = _chathub.GetMessageList(UserId, id);
            model.GUserName = UserName;
            return PartialView(model);
        }
    }
}
