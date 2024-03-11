using IBS.Models;

namespace IBS.Interfaces
{
    public interface IAllow_Old_Bill_DateRepository
    {
        public Allow_Old_Bill_DateModel FindByID(string region);

        DTResult<Allow_Old_Bill_DateModel> GetMessageList(DTParameters dtParameters, string GetRegionCode);

        int DetailsInsertUpdate(Allow_Old_Bill_DateModel model);
    }
}
