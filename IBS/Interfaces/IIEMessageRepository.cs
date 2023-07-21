using IBS.Models;

namespace IBS.Interfaces
{
    public interface IIEMessageRepository
    {
        DTResult<IEMessagesModel> GetUserList(DTParameters dtParameters, string GetRegionCode);
    }
}
