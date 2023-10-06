using IBS.DataAccess;
using IBS.Interfaces.InspectionBilling;

namespace IBS.Repositories.InspectionBilling
{
    public class BillAdjustmentsNewRepository : IBillAdjustmentsNewRepository
    {
        private readonly ModelContext context;

        public BillAdjustmentsNewRepository(ModelContext context)
        {
            this.context = context;
        }
    }
}
