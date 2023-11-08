using IBSAPI.Models;

namespace IBSAPI.Interfaces
{
    public interface ICallRepository
    {
        List<CallListModel> GetCallList();
        int SheduleInspection(SheduleInspectionRequestModel sheduleInspectionRequestModel);
    }
}
