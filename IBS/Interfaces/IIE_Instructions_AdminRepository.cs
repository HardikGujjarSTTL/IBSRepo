using IBS.Models;

namespace IBS.Interfaces
{
    public interface IIE_Instructions_AdminRepository
    {
        public IE_Instructions_AdminModel FindByID(int MessageId, string RegionCode);

        public int FindByMaxID(int MessageId, string RegionCode);

        DTResult<IE_Instructions_AdminModel> GetMessageList(DTParameters dtParameters, string RegionCode);

        bool Remove(int MessageId, string GetRegionCode);

        int SaveDetails(IE_Instructions_AdminModel model, string Region);
    }
}
