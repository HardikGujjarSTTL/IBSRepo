using IBS.Models;

namespace IBS.Interfaces
{
    public interface IMasterItemsPLFormRepository
    {
        public MasterItemsPLFormModel FindByID(string PlNo);

        DTResult<MasterItemsPLFormModel>GetMasterItemsPLFormList(DTParameters dtParameters);

        public void SaveDetails(MasterItemsPLFormModel model);

        public bool IsDuplicate(MasterItemsPLFormModel model);

        public bool Remove(string PlNo);
    }
}
