using IBS.DataAccess;
using IBS.Interfaces.IE;

namespace IBS.Repositories.IE
{
    public class IEJIRemarksPendingRepository : IIEJIRemarksPendingRepository
    {
        private readonly ModelContext context;

        public IEJIRemarksPendingRepository(ModelContext context)
        {
            this.context = context;
        }
    }
}
