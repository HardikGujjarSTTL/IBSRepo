using IBS.DataAccess;
using IBS.Interfaces;

namespace IBS.Repositories
{
    public class SAPInvoicesExportRepository : ISAPInvoicesExportRepository
    {
        private readonly ModelContext context;

        public SAPInvoicesExportRepository(ModelContext context)
        {
            this.context = context;
        }
    }
}
