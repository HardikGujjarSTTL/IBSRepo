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

        public async Task OnConnectedAsync(string Master_ID)
        {
            var groupName = "HubConnection";
            //Common.userid.Add(Context.ConnectionId);
            Common.connectedUser.Add(Master_ID, Context.ConnectionId);
            var group = Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Groups(groupName).NewUserConnected("a new user connectd = " + Context.ConnectionId);
        }

        public async Task BroadcastAsync(string Master_ID, string user, string message)
        {
            var users = Common.connectedUser.Where(x => x.Key == Master_ID || x.Key == user).ToList();
            foreach(var item in users)
            {
                await Clients.Client(item.Value).MessageReceivedFromHub(user, message);
            }
        }

    }
}
