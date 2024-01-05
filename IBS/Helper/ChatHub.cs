using IBS.Interfaces.Hub;
using IBS.Models;
using Microsoft.AspNetCore.SignalR;

namespace IBS.Helper
{
    public class ChatHub : Hub
    {
        private readonly IChatRepository _chathub;

        public ChatHub(IChatRepository chathub)
        {
            _chathub = chathub;
        }

        public async Task OnConnectedAsync(string SenderId)
        {
            foreach (var item in Common.ConnectedUsers.Where(kvp => kvp.Value == SenderId).ToList())
            {
                Common.ConnectedUsers.Remove(item.Key);
            }

            Common.ConnectedUsers.Add(Context.ConnectionId, SenderId);
        }

        public async Task SendMessage(string SenderId, string ReceiverId, string message, int MsgType)
        {
            ChatMessage model = new ChatMessage();
            model.msg_send_ID = SenderId;
            model.msg_recv_ID = ReceiverId;
            model.message = message;
            var res = _chathub.ChatMessageSave(model);

            //var myKey = Common.ConnectedUsers.Where(x => x.Value == ReceiverId || x.Value == SenderId).ToList();

            //var myKey = new List<KeyValuePair<string, string>>();
            //var arrRecvId = string.IsNullOrEmpty(model.msg_recv_ID) ? new List<string>() : model.msg_recv_ID.Split(",").ToList();
            //if (arrRecvId.Count() > 0)
            //{
            //    foreach (var item in arrRecvId)
            //    {
            //        KeyValuePair<string, string> obj = new KeyValuePair<string, string>();
            //        obj = Common.ConnectedUsers.Where(x => x.Value == item || x.Value == SenderId).FirstOrDefault();
            //        myKey.Add(obj);
            //    }
            //}

            var arrRecvId = string.IsNullOrEmpty(model.msg_recv_ID) ? new List<string>() : model.msg_recv_ID.Split(",").ToList();
            if (arrRecvId.Count() > 0)
            {
                foreach (var recvID in arrRecvId)
                {
                    var myKey = Common.ConnectedUsers.Where(x => x.Value == recvID || x.Value == SenderId).ToList();

                    foreach (var k in myKey)
                    {
                        await Clients.Clients(k.Key).SendAsync("ReceiveMessage", recvID, message, MsgType);
                        //await Clients.Clients(item.Key).SendAsync("ReceiveMessage", item.Value, message, MsgType);
                    }
                }
            }

            //foreach (var item in myKey)
            //{
            //    await Clients.Clients(item.Key).SendAsync("ReceiveMessage", ReceiverId, message, MsgType);                
            //}
        }
    }
}
