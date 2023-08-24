using IBS.Models;

namespace IBS.Interfaces
{
    public interface I_IE_MaximumCallLimitForm
    {
        public IE_MaximumCallLimitFormModel FindByID(string RegionCode);

        DTResult<IE_MaximumCallLimitFormModel>GetIE_MaximumCallLimitFormList(DTParameters dtParameters, string GetRegionCode);

        bool Remove(int RegionCode, int UserID);

        string IE_MaximumCallLimitFormDetailsInsertUpdate(IE_MaximumCallLimitFormModel model);
    }
}

