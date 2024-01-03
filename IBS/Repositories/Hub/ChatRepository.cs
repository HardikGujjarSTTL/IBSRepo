using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces.Hub;
using IBS.Models;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Data;

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

            //model.lstMsg = (from a in context.T113ChatMasters
            //                orderby a.Id
            //                where ((send_id == recv_id && a.MsgSendId == send_id || a.MsgRecvId == recv_id) ||
            //                ((a.MsgSendId == send_id && a.MsgRecvId == recv_id) || (a.MsgSendId == recv_id && a.MsgRecvId == send_id)))
            //                select new ChatMessage
            //                {
            //                    msg_send_ID = a.MsgSendId,
            //                    msg_recv_ID = a.MsgRecvId,
            //                    message = a.Message,
            //                }).ToList();

            OracleParameter[] par = new OracleParameter[3];
            par[0] = new OracleParameter("P_SENDER_ID", OracleDbType.Int32, send_id, ParameterDirection.Input);
            par[1] = new OracleParameter("P_RECEIVER_ID", OracleDbType.Int32, recv_id, ParameterDirection.Input);
            par[2] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("SP_GET_CHAT_MESSAGES", par);

            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                model.lstMsg = JsonConvert.DeserializeObject<List<ChatMessage>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
            return model;
        }

        public ChatMessage GetMessageRecvList(int id)
        {
            ChatMessage model = new ChatMessage();
            model.lstMsg = new List<ChatMessage>();


            OracleParameter[] par = new OracleParameter[2];
            par[0] = new OracleParameter("P_SEND_RECV_ID", OracleDbType.Int32, id, ParameterDirection.Input);            
            par[1] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("SP_GET_MESSAGE_RECEIVER_NAME", par);

            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                model.lstMsg = JsonConvert.DeserializeObject<List<ChatMessage>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
            return model;

        }
    }
}
