using IBS.DataAccess;
using IBS.Models;

namespace IBS.Interfaces
{
    public interface IClientCallStatusRepository
    {

        DTResult<ClientCallRptModel> GetCallStatusR(DTParameters dtParameters);
        DTResult<ClientCallRptModel> GetCallStatusC(DTParameters dtParameters);
    }
}
