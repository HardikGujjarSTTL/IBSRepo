using IBS.DataAccess;
using IBS.Interfaces;

namespace IBS.Repositories
{
    public class CallMarkedRepository : ICallMarkedRepository
    {
        private readonly ModelContext context;
        public CallMarkedRepository(ModelContext context)
        {
            this.context = context;
        }


    }
}
