using DocumentFormat.OpenXml.Office2010.Excel;
using IBS.Filters;
using IBS.Interfaces;
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
            model = _chathub.GetMessageRecvList(Master_ID);
            model.HostUrl = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host;
            model.msg_recv_ID = model.lstMsg.Select(x => x.msg_recv_ID).FirstOrDefault();
            ViewBag.Master_ID = Master_ID;
            return View(model);
        }

        [HttpPost]
        public async Task SendMessage(string user, string message)
        {
            ChatMessage model = new ChatMessage();
            model.msg_send_ID = Master_ID;
            model.msg_recv_ID = Convert.ToInt32(user);
            model.message = message;
            var res = _chathub.ChatMessageSave(model);
            if (res > 0)
                await this.hubContext.Clients.All.SendAsync("messageReceivedFromApi", Master_ID, message);
            else
                await this.hubContext.Clients.All.SendAsync("messageReceivedFromApi", res, message);
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
