using IBS.Filters;
using IBS.Interfaces.Hub;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

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
            model.HostUrl = _configuration.GetSection("AppSettings")["SiteUrl"];
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

        public IActionResult ChatMessageHistory(string id)
        {
            var recv_id = !string.IsNullOrEmpty(id) ? Convert.ToInt32(id) : 0;
            ChatMessage model = new ChatMessage();
            model = _chathub.GetMessageList(Master_ID, recv_id);
            model.Master_ID = Master_ID;
            return PartialView(model);
        }
    }
}
