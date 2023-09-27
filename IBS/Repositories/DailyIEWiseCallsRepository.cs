using IBS.DataAccess;
using IBS.Interfaces;

namespace IBS.Repositories
{
    public class DailyIEWiseCallsRepository : IDailyIEWiseCallsRepository   
    {
        private readonly ModelContext context;
        public DailyIEWiseCallsRepository(ModelContext context)
        {
            this.context = context;
        }
    }
}
