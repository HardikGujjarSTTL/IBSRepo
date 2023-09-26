using IBS.Models;
using IBS.Models.Reports;

namespace IBS.Interfaces.Reports.OtherReports
{
    public interface IOtherReportsRepository
    {
        ControllingOfficerIEModel GetControllingOfficerWiseIE(string Region);
        DTResult<CoIeWiseCallsListModel> GetCoIeWiseCalls(DTParameters dtParameters);//string CO, string Status, string IE,bool IsAllIE, bool IsCallDate);
    }
}
