using IBS.Filters;
using IBS.Helper;
using IBS.Interfaces.Hub;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace IBS.Controllers.SignalR
{
    [Authorization]
    public class ChatController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IChatRepository _chathub;
        private readonly IWebHostEnvironment env;
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatController(IConfiguration configuration, IChatRepository chathub, IWebHostEnvironment _env, IHubContext<ChatHub> hubContext)
        {
            _configuration = configuration;
            _chathub = chathub;
            env = _env;
            _hubContext = hubContext;
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

        [HttpPost]
        public async Task UploadFile(string SenderId, string ReceiverId, string message, int MsgType, IFormFile file)
        {
            //if (file == null || file.Length == 0)
            //return Content("file not selected");

            //var path = Path.Combine("/ReadWriteData/CHAT_FILES", file.FileName);
            var path = env.WebRootPath + "/ReadWriteData/CHAT_FILES";
            path = Path.Combine(path, file.FileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                file.CopyToAsync(stream);
            }

            var FileName = file.FileName;
            ChatMessage model = new ChatMessage();
            model.msg_send_ID = SenderId;
            model.msg_recv_ID = ReceiverId;
            model.message = message;
            model.FileName = FileName;
            var res = _chathub.ChatMessageSave(model);

            var arrRecvId = string.IsNullOrEmpty(model.msg_recv_ID) ? new List<string>() : model.msg_recv_ID.Split(",").ToList();
            if (arrRecvId.Count() > 0)
            {
                foreach (var recvID in arrRecvId)
                {
                    var myKey = Common.ConnectedUsers.Where(x => x.Value == recvID || x.Value == SenderId).ToList();

                    foreach (var k in myKey)
                    {
                        var extention = Path.GetExtension(FileName);
                        await _hubContext.Clients.Clients(k.Key).SendAsync("ReceiveMessage", recvID, message, MsgType, FileName, extention);
                    }
                }
            }
        }
    }
}
