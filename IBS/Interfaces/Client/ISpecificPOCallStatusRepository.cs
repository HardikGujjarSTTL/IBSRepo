using IBS.DataAccess;
using IBS.Models;

namespace IBS.Interfaces
{
    public interface ISpecificPOCallStatusRepository
    {

        DTResult<ClientCallRptModel> GetPOCallStatusIndex(DTParameters dtParameters, string OrgType, string Org);
        DTResult<ClientCallRptModel> GetPOCallStatus(DTParameters dtParameters);
    }
}
