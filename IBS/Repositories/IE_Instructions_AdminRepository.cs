using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using System.Drawing;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace IBS.Repositories
{
    public class IE_Instructions_AdminRepository : IIE_Instructions_AdminRepository
    {
        private readonly ModelContext context;

        public IE_Instructions_AdminRepository(ModelContext context)
        {
            this.context = context;
        }

        public IE_Instructions_AdminModel FindByID(int MessageId, string RegionCode)
        {
            IE_Instructions_AdminModel model = new();
            T72IeMessage message = context.T72IeMessages.Find(MessageId, RegionCode);

            if (message == null)
                throw new Exception("General Message Not found");
            else
            {
                model.MessageId = message.MessageId;
                model.LetterNo = message.LetterNo;
                model.LetterDt = message.LetterDt;
                model.Message = message.Message;
                model.Datetime = message.Datetime;
                model.RegionCode = message.RegionCode;
                model.Isdeleted = message.Isdeleted;

                return model;
            }
        }

        public int FindByMaxID(int MessageId, string RegionCode)
        {
            IE_Instructions_AdminModel model = new();
            T72IeMessage message = context.T72IeMessages.Find(MessageId, RegionCode);
            
            DTResult<IE_Instructions_AdminModel> dTResult = new() { draw = 0 };
            IQueryable<IE_Instructions_AdminModel>? query = null;

            query = from l in context.T72IeMessages
                    where l.RegionCode == RegionCode && (l.Isdeleted == 0 || l.Isdeleted == null)
                    select new IE_Instructions_AdminModel
                    {
                        MessageId = l.MessageId,
                    };
            dTResult.recordsTotal = query.Count();

            return query.Count() + 1;
        }

        public DTResult<IE_Instructions_AdminModel> GetMessageList(DTParameters dtParameters, string RegionCode)
        {

            DTResult<IE_Instructions_AdminModel> dTResult = new() { draw = 0 };
            IQueryable<IE_Instructions_AdminModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "LetterNo";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "LetterNo";
                orderAscendingDirection = true;
            }
            query = from l in context.T72IeMessages
                    where l.RegionCode == RegionCode && (l.Isdeleted == 0 || l.Isdeleted == null)
                    select new IE_Instructions_AdminModel
                    {
                        MessageId = l.MessageId,
                        LetterNo = l.LetterNo,
                        LetterDt = l.LetterDt,
                        Message = l.Message,
                        Datetime = l.Datetime,
                        RegionCode = l.RegionCode,
                        Isdeleted = l.Isdeleted,
                        Createddate = l.Createddate,
                        Createdby = l.Createdby,
                        Updateddate = l.Updateddate,
                        Updatedby = l.Updatedby
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.LetterNo).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public bool Remove(int MessageId, string GetRegionCode)
        {
            var User = context.T72IeMessages.Find(MessageId, GetRegionCode);
            
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

        public int SaveDetails(IE_Instructions_AdminModel model, string Region)
        {
            int MESSAGE_ID = 0;
            var G_Message = context.T72IeMessages.Find(model.MessageId, Region);

            IE_Instructions_AdminModel objCount = new ();
            int MessageId = (from l in context.T72IeMessages where l.RegionCode == Region select l).Count();

            #region User save
            if (G_Message == null)
            {
                T72IeMessage obj = new T72IeMessage();
                obj.MessageId = MessageId + 1;
                obj.LetterNo = model.LetterNo;
                obj.LetterDt = model.LetterDt;
                obj.Message = model.Message;
                obj.RegionCode = Region;
                obj.Datetime = DateTime.Now;
                obj.MessageDt = DateTime.Now;
                obj.UserId = model.Createdby;
                obj.Datetime = DateTime.Now;
                obj.Isdeleted = Convert.ToByte(false);
                obj.Createdby = model.Createdby;
                obj.Createddate = DateTime.Now;
                context.T72IeMessages.Add(obj);
                context.SaveChanges();
                MESSAGE_ID = Convert.ToInt32(obj.MessageId);
            }
            else
            {
                G_Message.LetterNo = model.LetterNo;
                G_Message.LetterDt = model.LetterDt;
                G_Message.Message = model.Message;
                G_Message.Datetime = model.Datetime;
                G_Message.UserId = model.Createdby;
                G_Message.Datetime = DateTime.Now;
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
