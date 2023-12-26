using IBS.DataAccess;
using IBS.Interfaces;

namespace IBS.Repositories
{
    public class ReceiptsRemitanceRRepository : IReceiptsRemitanceRepository
    {
        private readonly ModelContext context;

        public ReceiptsRemitanceRRepository(ModelContext context)
        {
            this.context = context;
        }

    }
}
