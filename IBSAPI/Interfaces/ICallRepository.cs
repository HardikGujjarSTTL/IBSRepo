using IBSAPI.Models;

namespace IBSAPI.Interfaces
{
    public interface ICallRepository
    {
        List<CallListModel> GetCallList();
        int SheduleInspection(SheduleInspectionRequestModel sheduleInspectionRequestModel);
        List<CallStatusModel> Get_Call_Status_List();
    }
}
