using IBS.Models;

namespace IBS.Interfaces
{
    public interface ICallRemarkingRepository
    {
        public int GetPendingCallsFromIE(string Region, int IeCd);

        public DTResult<PendingCallsListModel> GetPendingCallsList1(DTParameters dtParameters);

        public DTResult<PendingCallsListModel> GetPendingCallsList2(DTParameters dtParameters);

        public void SaveDetails(CallRemarkingModel model);
    }
}