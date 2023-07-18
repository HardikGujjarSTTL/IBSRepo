using IBS.Models;

namespace IBS.Interfaces
{
    public interface IDownloadDocumentsRepository
    {
        DTResult<DownloadDocumentsModel> GetMessageList(DTParameters dtParameters);
    }
}
