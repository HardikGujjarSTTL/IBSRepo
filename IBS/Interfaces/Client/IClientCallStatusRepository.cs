using IBS.DataAccess;
using IBS.Models;

namespace IBS.Interfaces
{
    public interface IClientCallStatusRepository
    {

        DTResult<ClientCallRptModel> GetCallStatusR(DTParameters dtParameters, string OrgType, string Org);
        DTResult<ClientCallRptModel> GetCallStatusC(DTParameters dtParameters, string OrgType, string Org);
    }
}
