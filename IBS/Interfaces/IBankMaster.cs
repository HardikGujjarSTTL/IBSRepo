using IBS.Models;

namespace IBS.Interfaces
{
    public interface IBankMaster
    {
        public BankMasterModel FindByID(int BankCd);
        DTResult<BankMasterModel> GetBankMasterList(DTParameters dtParameters);
        bool Remove(int BankCd, int UserID);
        int BankMasterDetailsInsertUpdate(BankMasterModel model);
    }
}