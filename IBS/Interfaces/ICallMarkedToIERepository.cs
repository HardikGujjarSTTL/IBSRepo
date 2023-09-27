using IBS.Models;
using IBS.Models.Reports;

namespace IBS.Interfaces
{
    public interface ICallMarkedToIERepository
    {
        DTResult<CallsMarkedToIEModel> GetDataList(DTParameters dtParameters, string GetRegionCode, string UserId, int GetIeCd);

        public CallsMarkedToIEModel GetReport(int GetIeCd, string UserId, string type);
    }
}
