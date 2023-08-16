using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using IBS.Helper;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace IBS.Repositories
{
    public class GeneralMessageRepository : IGeneralMessageRepository
    {
        private readonly ModelContext context;

        public GeneralMessageRepository(ModelContext context)
        {
            this.context = context;
        }

        public GeneralMessageModel FindByID(int MessageId)
        {
            using (var dbContext = context.Database.GetDbConnection())
            {
                OracleParameter[] par = new OracleParameter[2];
                par[0] = new OracleParameter("p_MessageId", OracleDbType.Decimal, MessageId, ParameterDirection.Input);
                par[1] = new OracleParameter("p_Cursor", OracleDbType.RefCursor, ParameterDirection.Output);

                var ds = DataAccessDB.GetDataSet("SP_GET_T96_MESSAGE", par, 1);

                GeneralMessageModel model = new();
                if (ds != null && ds.Tables.Count > 0)
                {
                    string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                    model = JsonConvert.DeserializeObject<List<GeneralMessageModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }).FirstOrDefault();
                }
                return model;
            }
        }

        public DTResult<GeneralMessageModel> GetMessageList(DTParameters dtParameters)
        {

            DTResult<GeneralMessageModel> dTResult = new() { draw = 0 };
            IQueryable<GeneralMessageModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "MESSAGE";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "MESSAGE";
                orderAscendingDirection = true;
            }
            query = from l in context.T96Messages
                    where (l.Isdeleted == 0 || l.Isdeleted == null)
                    select new GeneralMessageModel
                    {
                        MESSAGE_ID = l.MessageId,
                        MESSAGE = l.Message,
                        Isdeleted = l.Isdeleted,
                        Createddate = l.Createddate,
                        Createdby = l.Createdby,
                        Updateddate = l.Updateddate,
                        Updatedby = l.Updatedby
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.MESSAGE).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public bool Remove(int MessageId)
        {
            var User = context.T96Messages.Find(Convert.ToDecimal(MessageId));
            if (User == null)
            {
                return false;
            }

            User.Isdeleted = Convert.ToByte(true);
            User.Updatedby = User.Createdby;
            User.Updateddate = DateTime.Now;
            context.SaveChanges();
            return true;
        }

        public int MessageDetailsInsertUpdate(GeneralMessageModel model)
        {
            int MESSAGE_ID = 0;
            var G_Message = context.T96Messages.Find(model.MESSAGE_ID);
            #region User save
            if (G_Message == null)
            {
                T96Message obj = new T96Message();
                obj.Message = model.MESSAGE;
                obj.UserId = model.Createdby;
                obj.Datetime = DateTime.Now;
                obj.Isdeleted = Convert.ToByte(false);
                obj.Createdby = model.Createdby;
                obj.Createddate = DateTime.Now;
                context.T96Messages.Add(obj);
                context.SaveChanges();
                MESSAGE_ID = Convert.ToInt32(obj.MessageId);
            }
            else
            {
                G_Message.Message = model.MESSAGE;
                G_Message.Isdeleted = Convert.ToByte(false);
                G_Message.Updatedby = model.Updatedby;
                G_Message.Updateddate = DateTime.Now;
                context.SaveChanges();
                MESSAGE_ID = Convert.ToInt32(G_Message.MessageId);
            }
            #endregion
            return MESSAGE_ID;
        }

    }
}
