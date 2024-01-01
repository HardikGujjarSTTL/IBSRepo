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
            return View(model);
        }

        [HttpPost]
        public async Task SendMessage(string user, string message)
        {
            ChatMessage model = new ChatMessage();
            model.msg_send_ID = UserName;
            model.msg_recv_ID = user;
            model.send_message = message;
            model.recv_message = message;
            var restl = _chathub.ChatMessageSave(model, UserName, UserId);
            await this.hubContext.Clients.All.SendAsync("messageReceivedFromApi", user, message);
        }        

        public IActionResult ChatMessageHistory(string id)
        {
            ChatMessage model = new ChatMessage();
            model = _chathub.GetMessageList(id);
            return PartialView(model);
        }
    }
}
