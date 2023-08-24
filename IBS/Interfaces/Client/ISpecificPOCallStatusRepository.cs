using IBS.DataAccess;
using IBS.Models;

namespace IBS.Interfaces
{
    public interface ISpecificPOCallStatusRepository
    {

        DTResult<ClientCallRptModel> GetPOCallStatusIndex(DTParameters dtParameters);
        DTResult<ClientCallRptModel> GetPOCallStatus(DTParameters dtParameters);
    }
}
