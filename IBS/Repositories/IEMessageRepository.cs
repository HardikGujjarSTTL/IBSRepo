using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace IBS.Repositories
{
    public class IEMessageRepository : IIEMessageRepository
    {
        private readonly ModelContext context;

        public IEMessageRepository(ModelContext context)
        {
            this.context = context;
        }

        public DTResult<IEMessagesModel> GetUserList(DTParameters dtParameters, string GetRegionCode)
        {

            DTResult<IEMessagesModel> dTResult = new() { draw = 0 };
            IQueryable<IEMessagesModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "" || orderCriteria == null)
                {
                    orderCriteria = "MessageId";
                }
                //orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
                orderAscendingDirection = true;
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "MessageId";
                orderAscendingDirection = true;
            }

            var Totalcount = context.T72IeMessages.Count(x => x.RegionCode == GetRegionCode);

            query = from l in context.T72IeMessages
                    where l.RegionCode == GetRegionCode && l.MessageId != Totalcount
                    select new IEMessagesModel
                    {
                        MessageId = Convert.ToInt32(l.MessageId),
                        LetterNo = l.LetterNo,
                        LetterDt = l.LetterDt,
                        Message = l.Message,
                        MessageDt = l.MessageDt,
                        UserId = l.UserId,
                        Datetime = l.Datetime,
                        RegionCode = l.RegionCode,
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.Message).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.LetterNo).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }
    }
}
