using IBSAPI.Models;

namespace IBSAPI.Interfaces
{
    public interface ICallListRepository
    {
        List<CallListModel> GetCallList();
        List<CallRegiModel> GetCaseDetailsforvendor(int UserID);
    }
}
