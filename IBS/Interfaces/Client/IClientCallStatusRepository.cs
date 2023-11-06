using IBS.DataAccess;
using IBS.Models;

namespace IBS.Interfaces
{
    public interface IClientCallStatusRepository
    {

        ClientCallRptModel GetCallStatusR(string FromDate,string ToDate,string ReportStatus,string OrgType, string Org);
        ClientCallRptModel GetCallStatusC(string FromDate, string ToDate, string ReportStatus, string OrgType, string Org);
    }
}
