using IBS.Models;

namespace IBS.Interfaces
{
    public interface IClientMasterRepository
    {
        DTResult<Clientmaster> GetClientList(DTParameters dtParameters);
    }
}
