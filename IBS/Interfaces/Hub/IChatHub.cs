using IBS.Models;

namespace IBS.Interfaces.Hub
{
    public interface IChatHub 
    {
        //Task MessageReceivedFromHub(ChatMessage message);

        Task NewUserConnected(string message);

        Task MessageReceivedFromHub(string user, string message);
        //Task SendAsync(string Receive, string user, string message);       
    }
}
