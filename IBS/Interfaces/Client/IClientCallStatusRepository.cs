using IBS.DataAccess;
using IBS.Models;

namespace IBS.Interfaces
{
    public interface IClientCallStatusRepository
    {

        DTResult<ClientCallRptModel> GetCallStatus(DTParameters dtParameters);
    }
}
