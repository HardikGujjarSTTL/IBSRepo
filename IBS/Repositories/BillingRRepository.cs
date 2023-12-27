using IBS.DataAccess;
using IBS.Interfaces;

namespace IBS.Repositories
{
    public class BillingRRepository : IBillingRepository
    {
        private readonly ModelContext context;

        public BillingRRepository(ModelContext context)
        {
            this.context = context;
        }

    }
}
