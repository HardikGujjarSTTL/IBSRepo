using IBS.Filters;
using IBS.Helper;
using IBS.Interfaces.Hub;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace IBS.Controllers.SignalR
{
    public class MobileChatController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IChatRepository _chathub;
        private readonly IWebHostEnvironment _env;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly ChatHub _chatHub;

        public MobileChatController(IConfiguration configuration, IChatRepository chathub, IWebHostEnvironment env, IHubContext<ChatHub> hubContext, ChatHub chatHub)
        {
            _configuration = configuration;
            _chathub = chathub;
            _env = env;
            _hubContext = hubContext;
            _chatHub = chatHub;
        }

        public IActionResult Index(int SenderID)
        {
            ChatMessage model = new ChatMessage();
            model = _chathub.GetMessageRecvList(SenderID);
            model.HostUrl = _configuration.GetSection("AppSettings")["SiteUrl"];
            model.msg_recv_ID = model.lstMsg.Select(x => x.msg_recv_ID).FirstOrDefault();
            ViewBag.SenderId = SenderID;
            return View(model);
        }

        public IActionResult ChatUserReceiver(int SenderID)
        {
            ChatMessage model = new ChatMessage();
            model = _chathub.GetMessageRecvList(SenderID);
            return PartialView(model);
        }

        public IActionResult ChatMessageHistory(int SenderID,string ReceiverId)
        {
            var recv_id = !string.IsNullOrEmpty(ReceiverId) ? Convert.ToInt32(ReceiverId) : 0;
            ChatMessage model = new ChatMessage();
            model = _chathub.GetMessageList(SenderID, recv_id);
            model.Master_ID = SenderID;
            model.lstMsg.Select(x => x.RelativePath = !string.IsNullOrEmpty(x.RelativePath) ? _configuration.GetSection("AppSettings")["SiteUrl"] + x.RelativePath : x.RelativePath).ToList();
            return PartialView(model);
        }

        [HttpPost]
        public async Task UploadFile(FormData formData)
        {
            var FileName = formData.MyFiles.FileName;
            byte[] fileBytes = ConvertFileToBytes(formData.MyFiles);
            //await _chatHub.SendMessage(formData.SenderId, formData.ReceiverId, formData.Message, formData.MsgType, formData.MyFiles);

            string FileDisplayName = null, Field_ID = null, Extension = null;
            ChatMessage model = new ChatMessage();
            model.msg_send_ID = formData.SenderId;
            model.msg_recv_ID = formData.ReceiverId;
            model.message = formData.Message;
            if (formData.MyFiles != null)
            {
                Guid newGuid = Guid.NewGuid();
                model.RelativePath = "../ReadWriteData/CHAT_FILES";
                model.Field_ID = Path.GetFileNameWithoutExtension(formData.MyFiles.FileName) + "_" + newGuid.ToString() + Path.GetExtension(formData.MyFiles.FileName);
                model.Extension = Path.GetExtension(formData.MyFiles.FileName);
                model.FileDisplayName = formData.MyFiles.FileName;
                FileDisplayName = formData.MyFiles.FileName;
                Field_ID = model.Field_ID;
                Extension = Path.GetExtension(formData.MyFiles.FileName);
            }
            var res = _chathub.ChatMessageSave(model);

            if (formData.MyFiles != null)
            {
                var path = _env.WebRootPath + "/ReadWriteData/CHAT_FILES";
                path = Path.Combine(path, model.Field_ID);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    formData.MyFiles.CopyToAsync(stream);
                }
            }
            var arrRecvId = string.IsNullOrEmpty(model.msg_recv_ID) ? new List<string>() : model.msg_recv_ID.Split(",").ToList();
            if (arrRecvId.Count() > 0)
            {
                foreach (var recvID in arrRecvId)
                {
                    var myKey = Common.ConnectedUsers.Where(x => x.Value == recvID || x.Value == formData.SenderId).ToList();

                    foreach (var k in myKey)
                    {
                        var lstChat = _chathub.GetMessageList(Convert.ToInt32(formData.SenderId), Convert.ToInt32(recvID)).lstMsg;
                        var CurrDateCount = lstChat.Where(x => x.Msg_Date.Date == DateTime.Now.Date).Count();
                        await _hubContext.Clients.Clients(k.Key).SendAsync("ReceiveMessage", recvID, formData.Message, formData.MsgType, Field_ID, FileDisplayName, Extension, CurrDateCount);
                    }
                }
            }
        }

        private byte[] ConvertFileToBytes(IFormFile file)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Copy the file stream to the memory stream
                file.CopyTo(ms);

                // Return the byte array
                return ms.ToArray();
            }
        }

        public IActionResult GetUsersByUserType(string type, string id)
        {
            return Json(Common.GetUserMaster(type, id).ToList());
        }
    }
}
