using IBS.DataAccess;
using IBS.Interfaces.Hub;
using IBS.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace IBS.Repositories.Hub
{
    public class ChatHub : Hub<IChatHub>
    {
        private readonly ModelContext context;
        //private readonly IHubContext<ChatHub> hubContext;
        //public ChatHub(ModelContext context, IHubContext<ChatHub> hubContext)
        //{
        //    this.context = context;
        //    this.hubContext = hubContext;
        //}

        //public async Task BroadcastAsync(ChatMessage message)
        //{
        //    await Clients.All.MessageReceivedFromHub(message);
        //}

        public async Task OnConnectedAsync()
        {
            Common.userid.Add(Context.ConnectionId);
            await Clients.All.NewUserConnected("a new user connectd = " + Context.ConnectionId);
        }

        public async Task BroadcastAsync(string user, string message)
        {
            //await Clients.All.MessageReceivedFromHub(user, message);
            await Clients.All.MessageReceivedFromHub(user, message);
        }
        //public async Task SendMessage(string user, string message)
        //{
        //    await Clients.All.SendAsync("ReceiveMessage", user, message);
        //}


    }
}
