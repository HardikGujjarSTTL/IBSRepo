using IBS.DataAccess;
using IBS.Interfaces;

namespace IBS.Repositories
{
    public class ComplaintsJIRequiredReportRepository : IComplaintsJIRequiredReportRepository
    {
        private readonly ModelContext context;

        public ComplaintsJIRequiredReportRepository(ModelContext context)
        {
            this.context = context;
        }
    }
}
