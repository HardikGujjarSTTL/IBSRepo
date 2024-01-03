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

        public async Task OnConnectedAsync(int SenderId)
        {
            foreach (var item in Common.ConnectedUsers.Where(kvp => kvp.Value == SenderId).ToList())
            {
                Common.ConnectedUsers.Remove(item.Key);
            }

            Common.ConnectedUsers.Add(Context.ConnectionId, SenderId);
        }

        public async Task SendMessage(int SenderId, int ReceiverId, string message, int MsgType)
        {
            ChatMessage model = new ChatMessage();
            model.msg_send_ID = Convert.ToInt32(SenderId);
            model.msg_recv_ID = Convert.ToInt32(ReceiverId);
            model.message = message;
            var res = _chathub.ChatMessageSave(model);

            var myKey = Common.ConnectedUsers.Where(x => x.Value == ReceiverId || x.Value == SenderId).ToList();

            foreach (var item in myKey)
            {
                await Clients.Clients(item.Key).SendAsync("ReceiveMessage", ReceiverId, message, MsgType);
            }
        }
    }
}
