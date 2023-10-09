using IBS.Models;

namespace IBS.Interfaces
{
    public interface IMasterItemsListForm
    {
        public MasterItemsListFormModel FindByID(string ItemCd, string Region);

        DTResult<MasterItemsListFormModel> GetMasterItemsListFormList(DTParameters dtParameters);

        bool Remove(string ItemCd, int UserID);

        string DtlInsertUpdate(MasterItemsListFormModel model, string Region, int GetIeCd);
    }
}

