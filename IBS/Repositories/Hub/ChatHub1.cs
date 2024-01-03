using IBS.DataAccess;
using IBS.Interfaces.Hub;
using IBS.Models;
using Microsoft.AspNetCore.SignalR;

namespace IBS.Repositories.Hub
{
    public class ChatHub1 : Hub<IChatHub>
    {
        private readonly ModelContext context;
        private readonly IChatRepository _chathub;

        public ChatHub1(IChatRepository chathub)
        {
            _chathub = chathub;
        }

        public async Task OnConnectedAsync(string Master_ID)
        {
            foreach (var item in Common.ConnectedUsers.Where(kvp => kvp.Value == Master_ID).ToList())
            {
                Common.ConnectedUsers.Remove(item.Key);
            }

            Common.ConnectedUsers.Add(Context.ConnectionId, Master_ID);

            await Clients.All.NewUserConnected("a new user connectd = " + Context.ConnectionId);
        }

        public async Task BroadcastAsync(string user, string message)
        {
            await Clients.All.MessageReceivedFromHub(user, message);
        }

        public async Task SendMessage(string Master_ID, string user, string message)
        {
            string fromUserId = Context.ConnectionId;

            ChatMessage model = new ChatMessage();
            model.msg_send_ID = Convert.ToInt32(Master_ID);
            model.msg_recv_ID = Convert.ToInt32(user);
            model.message = message;
            var res = _chathub.ChatMessageSave(model);

            var myKey = Common.ConnectedUsers.FirstOrDefault(x => x.Value == user).Key;

            await this.Clients.Clients(myKey.ToString()).ReceiveMessage(user, message);
        }
    }
}
