using IBS.DataAccess;
using IBS.Interfaces.Vendor;

namespace IBS.Repositories.Vendor
{
    public class DownloadInspFeeBillRepository : IDownloadInspFeeBillRepository
    {
        private readonly ModelContext context;

        public DownloadInspFeeBillRepository(ModelContext context)
        {
            this.context = context;
        }

    }
}
