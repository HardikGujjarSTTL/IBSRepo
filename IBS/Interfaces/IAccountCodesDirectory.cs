using IBS.Models;

namespace IBS.Interfaces
{
    public interface IAccountCodesDirectory
    {
        public AccountCodesDirectoryModel FindByID(int AccCd);
        DTResult<AccountCodesDirectoryModel> GetAccountCodesDirectoryList(DTParameters dtParameters);
        bool Remove(int AccCd);
        int AccountCodesDirectoryDetailsInsertUpdate(AccountCodesDirectoryModel model);
    }
}