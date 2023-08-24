using IBS.Models;

namespace IBS.Interfaces
{
    public interface IInspectionFeeBillRepository
    {
        public InspectionFeeBillModel FindByBillNo(string BillNo);
    }
}
