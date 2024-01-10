using IBS.Interfaces.Hub;
using IBS.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.JSInterop;
using System.Text;
using System.Threading.Tasks;

namespace IBS.Helper
{
    public class ChatHub : Hub
    {
        private readonly IChatRepository _chathub;
        private readonly IWebHostEnvironment _env;

        public ChatHub(IChatRepository chathub, IWebHostEnvironment env)
        {
            _chathub = chathub;
            _env = env;
        }

        public async Task OnConnectedAsync(string SenderId)
        {
            foreach (var item in Common.ConnectedUsers.Where(kvp => kvp.Value == SenderId).ToList())
            {
                Common.ConnectedUsers.Remove(item.Key);
            }

            Common.ConnectedUsers.Add(Context.ConnectionId, SenderId);
        }

        public async Task SendMessage(string SenderId, string ReceiverId, string Message, int MsgType, IEnumerable<ImageData> images)
        {
            string FileDisplayName = null, Field_ID = null, Extension = null;
            byte[] buffer = new byte[] { };

            var arrRecvId = string.IsNullOrEmpty(ReceiverId) ? new List<string>() : ReceiverId.Split(",").ToList();
            if (arrRecvId.Count() > 0)
            {
                foreach (var recvID in arrRecvId)
                {
                    ChatMessage model = new ChatMessage();
                    model.msg_send_ID = SenderId;
                    model.msg_recv_ID = recvID;
                    model.message = Message;
                    model.Msg_Date = DateTime.Now;

                    foreach (var item in images ?? Enumerable.Empty<ImageData>())
                    {
                        var tokens = item.Image.Split(',');
                        if (tokens.Length > 1)
                        {
                            buffer = Convert.FromBase64String(tokens[1]);
                            if (images != null)
                            {
                                Guid newGuid = Guid.NewGuid();
                                model.RelativePath = "../ReadWriteData/CHAT_FILES";
                                model.Field_ID = newGuid.ToString() + Path.GetExtension(item.FileName);
                                model.Extension = Path.GetExtension(item.FileName);
                                model.FileDisplayName = item.FileName;
                                FileDisplayName = item.FileName;
                                Field_ID = model.Field_ID;
                                Extension = Path.GetExtension(item.FileName);
                            }
                        }
                    }

                    var Msg_Date = model.Msg_Date.ToString("hh:mm tt");
                    try
                    {
                        var res = _chathub.ChatMessageSave(model);

                        if (res > 0)
                        {
                            if (images != null)
                            {
                                var path = _env.WebRootPath + "/ReadWriteData/CHAT_FILES";
                                path = Path.Combine(path, model.Field_ID);
                                SaveByteArrayToFileWithBinaryWriter(buffer, path);
                            }
                        }

                        var myKey = Common.ConnectedUsers.Where(x => x.Value == recvID || x.Value == SenderId).ToList();

                        if (res > 0)
                        {
                            foreach (var k in myKey)
                            {
                                var lstChat = _chathub.GetMessageList(Convert.ToInt32(SenderId), Convert.ToInt32(recvID)).lstMsg;
                                var CurrDateCount = lstChat.Where(x => x.Msg_Date.Date == DateTime.Now.Date).Count();
                                await Clients.Clients(k.Key).SendAsync("ReceiveMessage", recvID, Message, MsgType, Msg_Date, Field_ID, FileDisplayName, Extension, CurrDateCount);
                            }
                        }
                        else
                        {
                            foreach (var k in myKey)
                            {
                                await Clients.Clients(k.Key).SendAsync("ReceiveMessage", "", "", MsgType, "", "", "", "", 0);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        var myKey = Common.ConnectedUsers.Where(x => x.Value == recvID || x.Value == SenderId).ToList();

                        foreach (var k in myKey)
                        {
                            await Clients.Clients(k.Key).SendAsync("ReceiveMessage", "", "", MsgType, "", "", "", "", 0);
                        }
                    }
                }
            }
        }

        public static void SaveByteArrayToFileWithBinaryWriter(byte[] data, string filePath)
        {
            using var writer = new BinaryWriter(File.OpenWrite(filePath));
            writer.Write(data);
        }

        public async Task SendMessage1(string SenderId, string ReceiverId, string message, int MsgType, IFormFile? file)
        {
            string FileDisplayName = null, Field_ID = null, Extension = null;

            ChatMessage model = new ChatMessage();
            model.msg_send_ID = SenderId;
            model.msg_recv_ID = ReceiverId;
            model.message = message;
            if (file != null)
            {
                Guid newGuid = Guid.NewGuid();
                model.RelativePath = "../ReadWriteData/CHAT_FILES";
                model.Field_ID = Path.GetFileNameWithoutExtension(file.FileName) + "_" + newGuid.ToString() + Path.GetExtension(file.FileName);
                model.Extension = Path.GetExtension(file.FileName);
                model.FileDisplayName = file.FileName;
                FileDisplayName = file.FileName;
                Field_ID = model.Field_ID;
                Extension = Path.GetExtension(file.FileName);
            }
            var res = _chathub.ChatMessageSave(model);

            if (file != null)
            {
                var path = _env.WebRootPath + "/ReadWriteData/CHAT_FILES";
                path = Path.Combine(path, model.Field_ID);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyToAsync(stream);
                }
            }

            var arrRecvId = string.IsNullOrEmpty(model.msg_recv_ID) ? new List<string>() : model.msg_recv_ID.Split(",").ToList();
            if (arrRecvId.Count() > 0)
            {
                foreach (var recvID in arrRecvId)
                {
                    var myKey = Common.ConnectedUsers.Where(x => x.Value == recvID || x.Value == SenderId).ToList();

                    foreach (var k in myKey)
                    {
                        var lstChat = _chathub.GetMessageList(Convert.ToInt32(SenderId), Convert.ToInt32(recvID)).lstMsg;
                        var CurrDateCount = lstChat.Where(x => x.Msg_Date.Date == DateTime.Now.Date).Count();
                        //model.CurrDateCount = model.lstMsg.Where(x => x.Msg_Date.Date == DateTime.Now.Date).Count();
                        await Clients.Clients(k.Key).SendAsync("ReceiveMessage", recvID, message, MsgType, Field_ID, FileDisplayName, Extension, CurrDateCount);
                    }
                }
            }
            #region Old Response
            //var myKey = Common.ConnectedUsers.Where(x => x.Value == ReceiverId || x.Value == SenderId).ToList();
            //foreach (var item in myKey)
            //{
            //    await Clients.Clients(item.Key).SendAsync("ReceiveMessage", ReceiverId, message, MsgType);                
            //}
            #endregion
        }
    }
}
