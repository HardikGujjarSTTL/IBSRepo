using IBS.Models;

namespace IBS.Interfaces.IE
{
    public interface IIEJIRemarksPendingRepository
    {
        DTResult<IEJIRemarksPendingModel> GetDataList(DTParameters dtParameters, string GetRegionCode, string UserId, int GetIeCd);
    }
}
