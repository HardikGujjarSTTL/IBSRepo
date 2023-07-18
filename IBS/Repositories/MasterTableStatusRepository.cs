using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;

namespace IBS.Repositories
{
    public class MasterTableStatusRepository : IMasterTableStatusRepository
    {
        private readonly ModelContext context;

        public MasterTableStatusRepository(ModelContext context)
        {
            this.context = context;
        }

        public DTResult<MasterTableStatusModel> GetMessageList(DTParameters dtParameters)
        {

            DTResult<MasterTableStatusModel> dTResult = new() { draw = 0 };
            IQueryable<MasterTableStatusModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "Tablename";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "Tablename";
                orderAscendingDirection = true;
            }
            query = from l in context.Mastertablestatuses
                    select new MasterTableStatusModel
                    {
                        Tablename = l.Tablename,
                        Totalcount = l.Totalcount,
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.Tablename).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }
    }
}
