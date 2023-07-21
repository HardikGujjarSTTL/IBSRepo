using IBS.DataAccess;
using IBS.Interfaces.Administration;

namespace IBS.Repositories.Administration
{
    public class UploadDocRepository : IUploadDocRepository
    {
        private readonly ModelContext context;

        public UploadDocRepository(ModelContext context)
        {
            this.context = context;
        }
    }
}
