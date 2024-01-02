using IBS.Models;

namespace IBS.Interfaces.Hub
{
    public interface IChatRepository
    {
        int ChatMessageSave(ChatMessage model, string userName,int userID);

        ChatMessage GetMessageList(int send_id, int recv_id);

        ChatMessage GetMessageRecvList(string id);
    }
}
