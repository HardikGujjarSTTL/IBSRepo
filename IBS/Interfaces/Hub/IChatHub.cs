namespace IBS.Interfaces.Hub
{
    public interface IChatHub
    {
        Task NewUserConnected(string message);

        Task MessageReceivedFromHub(string user, string message);

        Task ReceiveMessage(string Master_ID, string message);

    }
}
