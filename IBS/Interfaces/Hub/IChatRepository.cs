using IBS.Models;

namespace IBS.Interfaces.Hub
{
    public interface IChatRepository
    {
        int ChatMessageSave(ChatMessage model, string userName,int userID);
        ChatMessage GetMessageList(string id);
        ChatMessage GetMessageRecvList(string id);
    }
}
