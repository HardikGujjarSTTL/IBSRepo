using IBS.Models;

namespace IBS.Interfaces
{
    public interface IMasterItemsListForm
    {
        public MasterItemsListFormModel FindByID(string ItemCd);
        DTResult<MasterItemsListFormModel> GetMasterItemsListFormList(DTParameters dtParameters);
        bool Remove(string ItemCd, int UserID);
        string MasterItemsListFormInsertUpdate(MasterItemsListFormModel model);
    }
}

