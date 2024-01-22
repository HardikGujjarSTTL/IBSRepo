using IBS.Models;

namespace IBS.Interfaces
{
    public interface IAllGeneratedBillsRepository
    {
        DTResult<AllGeneratedBills> GetBillDetails(DTParameters dtParameters);

        public AllGeneratedBills CreateBills(AllGeneratedBills model);

        public AllGeneratedBills ReturnBills(AllGeneratedBills model);
        List<ItemsDetail> GetBillItems(string Bill_No);
    }
}
