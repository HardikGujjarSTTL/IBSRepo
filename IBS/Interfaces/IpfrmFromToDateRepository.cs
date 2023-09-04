using IBS.Models;

namespace IBS.Interfaces
{
    public interface IpfrmFromToDateRepository
    {
        DTResult<ICIsuued> GetDataList(DTParameters dtParameters);
    }
}
