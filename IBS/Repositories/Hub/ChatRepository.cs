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

        public int ChatMessageSave(ChatMessage model)
        {
            var result = 0;
            if (model.ID <= 0)
            {
                T113ChatMaster obj = new T113ChatMaster();
                obj.ConnectionId = null;
                obj.MsgSendId = model.msg_send_ID;
                obj.MsgRecvId = model.msg_recv_ID;
                obj.Message = model.message;
                obj.SendMsgDate = DateTime.Now;
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
                    dtlChat.Message = model.message;
                    dtlChat.SendMsgDate = DateTime.Now;
                    context.SaveChanges();
                    result = model.ID;
                }
            }
            return result;
        }

        public ChatMessage GetMessageList(int send_id, int recv_id)
        {
            ChatMessage model = new ChatMessage();
            model.lstMsg = new List<ChatMessage>();

            model.lstMsg = (from a in context.T113ChatMasters
                            orderby a.Id
                            where ((a.MsgSendId == send_id && a.MsgRecvId == recv_id)
                            || (a.MsgSendId == recv_id && a.MsgRecvId == send_id))
                            select new ChatMessage
                            {
                                msg_send_ID = a.MsgSendId,
                                msg_recv_ID = a.MsgRecvId,
                                message = a.Message,
                            }).ToList();
            return model;
        }

        public ChatMessage GetMessageRecvList(int id)
        {
            ChatMessage model = new ChatMessage();
            model.lstMsg = new List<ChatMessage>();

            model.lstMsg = (from a in context.T113ChatMasters
                            join b in context.UserMasters on a.MsgRecvId equals Convert.ToInt32(b.Id)
                            where a.MsgSendId == id
                            orderby a.Id descending
                            select new ChatMessage
                            {
                                msg_recv_ID = a.MsgRecvId,
                                Name = b.Name
                            }).Distinct().ToList();

            return model;

        }
    }
}
