using IBS.Models;

namespace IBS.Interfaces.InspectionBilling
{
    public interface ISupplementaryBillRepository
    {
        DTResult<BillDetailsModel> GetLoadTable(DTParameters dtParameters, string Region);
    }
}
