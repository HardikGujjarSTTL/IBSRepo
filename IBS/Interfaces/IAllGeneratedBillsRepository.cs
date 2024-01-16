using IBS.Models;

namespace IBS.Interfaces
{
    public interface IAllGeneratedBillsRepository
    {
        DTResult<AllGeneratedBills> GetBillDetails(DTParameters dtParameters);
    }
}
