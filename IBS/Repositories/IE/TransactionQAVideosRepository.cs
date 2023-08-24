using IBS.DataAccess;
using IBS.Interfaces.IE;

namespace IBS.Repositories.IE
{
    public class TransactionQAVideosRepository : ITransactionQAVideosRepository
    {
        private readonly ModelContext context;

        public TransactionQAVideosRepository(ModelContext context)
        {
            this.context = context;
        }
    }
}
