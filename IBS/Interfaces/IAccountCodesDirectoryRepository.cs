using IBS.Models;

namespace IBS.Interfaces
{
    public interface IAccountCodesDirectoryRepository
    {
        public AccountCodesDirectoryModel FindByID(int AccCd);

        DTResult<AccountCodesDirectoryModel> GetAccountCodesDirectoryList(DTParameters dtParameters);

        public void SaveDetails(AccountCodesDirectoryModel model);

        public bool IsDuplicate(AccountCodesDirectoryModel model);

        public bool Remove(int AccCd);

    }
}