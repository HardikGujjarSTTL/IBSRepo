using IBS.DataAccess;
using IBS.Interfaces.Hub;
using IBS.Models;

namespace IBS.Repositories.Hub
{
    public class ChatRepository : IChatRepository
    {
        private readonly ModelContext context;
        public ChatRepository(ModelContext context)
        {
            this.context = context;
        }
        public int ChatMessageSave(ChatMessage model, string UserName, int userID)
        {
            var result = 0;
            if (model.ID <= 0)
            {
                T113ChatMaster obj = new T113ChatMaster();
                obj.ConnectionId = null;
                obj.MsgSendId = model.msg_send_ID;
                obj.MsgRecvId = model.msg_recv_ID;
                obj.Message = model.send_message;
                //obj.UserId = model.msg_send_ID;
                obj.Createdby = userID;
                obj.Createddate = DateTime.Now;
                context.T113ChatMasters.Add(obj);
                context.SaveChanges();
                result = obj.Id;
            }
            else
            {
                var dtlChat = (from a in context.T113ChatMasters
                               where a.Id == model.ID
                               select a).FirstOrDefault();
                if (dtlChat != null)
                {
                    dtlChat.ConnectionId = null;
                    dtlChat.MsgSendId = model.msg_send_ID;
                    dtlChat.MsgRecvId = model.msg_recv_ID;
                    dtlChat.Message = model.send_message;
                    //dtlChat.UserId = msg_send_ID;
                    dtlChat.Updatedby = userID;
                    dtlChat.Updateddate = DateTime.Now;
                    context.SaveChanges();
                    result = model.ID;
                }
            }
            return result;
        }

        public ChatMessage GetMessageList(string id)
        {
            ChatMessage model = new ChatMessage();
            model.lstMsg = new List<ChatMessage>();

            model.lstMsg = (from a in context.T113ChatMasters
                            where a.MsgRecvId == id
                            select new ChatMessage
                            {
                                msg_send_ID = a.MsgSendId,
                                msg_recv_ID = a.MsgRecvId,
                                send_message = a.Message,
                                recv_message = a.Message,
                            }).ToList();
            return model;
        }

        public ChatMessage GetMessageRecvList(string id)
        {
            ChatMessage model = new ChatMessage();
            model.lstMsg = new List<ChatMessage>();

            model.lstMsg = (from a in context.T113ChatMasters
                            join b in context.T09Ies on Convert.ToInt32(a.MsgRecvId) equals b.IeCd
                            orderby a.Id descending
                            where a.MsgSendId == id
                            select new ChatMessage
                            {
                                msg_recv_ID = a.MsgRecvId,
                                Name = b.IeName
                            }).Distinct().ToList();

            return model;

        }
    }
}
