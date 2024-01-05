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
            
            var arrRecvId = string.IsNullOrEmpty(model.msg_recv_ID) ? new List<string>() : model.msg_recv_ID.Split(",").ToList();
            if (arrRecvId.Count() > 0)
            {
                foreach (var recvID in arrRecvId)
                {
                    var myKey = Common.ConnectedUsers.Where(x => x.Value == recvID || x.Value == SenderId).ToList();

                    foreach (var k in myKey)
                    {
                        await Clients.Clients(k.Key).SendAsync("ReceiveMessage", recvID, message, MsgType);
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

        //public async Task SendFile(string SenderId, string ReceiverId)
        //{            
        //    await Clients.Caller.SendAsync("FileReceived", ReceiverId);
        //}

        public async Task SendFile(IFormFile file)
        {
            try
            {
                // Process the file on the server
                // ...

                // Send a success response back to the client
                await Clients.Caller.SendAsync("FileSentResponse", "File received successfully");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.Error.WriteLine($"Error processing file: {ex.Message}");

                // Send an error response back to the client
                await Clients.Caller.SendAsync("FileSentError", "Failed to process file. Please try again.");
            }
            await Clients.Caller.SendAsync("FileReceived", file);
        }
        
    }
}
