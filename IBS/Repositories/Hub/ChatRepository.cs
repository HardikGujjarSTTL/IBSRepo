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
            var arrRecvId = string.IsNullOrEmpty(model.msg_recv_ID) ? new List<string>() : model.msg_recv_ID.Split(",").ToList();
            if (arrRecvId.Count() > 0)
            {
                foreach (var item in arrRecvId)
                {
                    T113ChatMaster obj = new T113ChatMaster();
                    obj.MsgSendId = Convert.ToInt32(model.msg_send_ID);
                    obj.MsgRecvId = Convert.ToInt32(item);
                    obj.Message = model.message;
                    obj.SendMsgDate = DateTime.Now;
                    obj.Relativepath = model.RelativePath;
                    obj.FieldId = model.Field_ID;
                    obj.Extension = model.Extension;
                    obj.Filedisplayname = model.FileDisplayName;
                    context.T113ChatMasters.Add(obj);
                    context.SaveChanges();
                    result = obj.Id;
                }
            }            
            return result;
        }

        public ChatMessage GetMessageList(int send_id, int recv_id)
        {
            ChatMessage model = new ChatMessage();
            model.lstMsg = new List<ChatMessage>();
            
            OracleParameter[] par = new OracleParameter[3];
            par[0] = new OracleParameter("P_SENDER_ID", OracleDbType.Int32, send_id, ParameterDirection.Input);
            par[1] = new OracleParameter("P_RECEIVER_ID", OracleDbType.Int32, recv_id, ParameterDirection.Input);
            par[2] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("SP_GET_CHAT_MESSAGES", par);

            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                model.lstMsg = JsonConvert.DeserializeObject<List<ChatMessage>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                model.CurrDateCount = model.lstMsg.Where(x => x.Msg_Date.Date == DateTime.Now.Date).Count();
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
