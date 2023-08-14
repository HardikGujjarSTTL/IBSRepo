using IBS.Models;

namespace IBS.Interfaces
{
    public interface IMasterItemsPLForm
    {
        public MasterItemsPLFormModel FindByID(int ItemCd);
        DTResult<MasterItemsPLFormModel>GetMasterItemsPLFormList(DTParameters dtParameters);
        bool Remove(int ItemCd, int UserID);
        int MasterItemsPLFormDetailsInsertUpdate(MasterItemsPLFormModel model);
    }
}
