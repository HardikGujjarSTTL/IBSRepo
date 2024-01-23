using IBS.DataAccess;
using IBS.Models;

namespace IBS.Interfaces
{
    public interface IAllGeneratedBillsRepository
    {
        DTResult<AllGeneratedBills> GetBillDetails(DTParameters dtParameters);

        public AllGeneratedBills CreateBills(AllGeneratedBills model);

        public AllGeneratedBills ReturnBills(AllGeneratedBills model);

        List<ItemsDetail> GetBillItems(string Bill_No);

        List<T22Bill> GetBillByBillNo(string Bill_No);

        public string UpdateBillCount(string Bill_No,int count);

        public string UpdateGEN_Bill_Date(string Bill_No);
    }
}
