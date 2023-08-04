using IBS.Models;

namespace IBS.Interfaces.Vendor
{
    public interface IDownloadInspFeeBillRepository
    {

        DTResult<DownloadInspectionFeeBillModel> GetDataList(DTParameters dtParameters, string UserName);
    }
}
