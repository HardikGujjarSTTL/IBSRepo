using IBS.Models;

namespace IBS.Interfaces
{
    public interface IBankRepository
    {
        public BankMasterModel FindByID(int Id);

        DTResult<BankMasterModel> GetBankMasterList(DTParameters dtParameters);

        public int SaveDetails(BankMasterModel model);

        public bool Remove(int Id, int UserID);
    }
}