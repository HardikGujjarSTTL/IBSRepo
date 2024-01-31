using IBS.DataAccess;
using IBS.Interfaces;

namespace IBS.Repositories
{
    public class MultipleFileUploadRepository : IMultipleFileUploadRepository
    {
        private readonly ModelContext context;

        public MultipleFileUploadRepository(ModelContext context)
        {
            this.context = context;
        }
    }
}
