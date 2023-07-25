using IBS.Models;

namespace IBS.Interfaces
{
    public interface ICallMarkedToIERepository
    {
        DTResult<CallsMarkedToIEModel> GetDataList(DTParameters dtParameters, string GetRegionCode, string UserId, int GetIeCd);
    }
}
