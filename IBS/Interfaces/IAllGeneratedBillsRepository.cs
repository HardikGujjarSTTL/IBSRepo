using IBS.Models;

namespace IBS.Interfaces
{
    public interface IAllGeneratedBillsRepository
    {
        DTResult<AllGeneratedBills> GetBillDetails(DTParameters dtParameters);

        public AllGeneratedBills GenerateBill(AllGeneratedBills model);

        public AllGeneratedBills GenerateReturnBill(AllGeneratedBills model);
    }
}
