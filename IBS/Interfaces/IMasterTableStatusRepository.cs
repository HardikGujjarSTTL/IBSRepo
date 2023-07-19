using IBS.Models;

namespace IBS.Interfaces
{
    public interface IMasterTableStatusRepository
    {
        DTResult<MasterTableStatusModel> GetMessageList(DTParameters dtParameters);
    }
}
