using IBSAPI.Models;

namespace IBSAPI.Interfaces
{
    public interface ICallRepository
    {
        List<CallListModel> GetCallList();
        int SheduleInspection(SheduleInspectionRequestModel sheduleInspectionRequestModel,int PlanDHours);
        List<CallStatusModel> Get_Call_Status_List();

        int CancelInspection(int IeCd, string CaseNo, DateTime PlanDt, DateTime CallRecvDt, int CallSno);

        VenderCallStatusModel CallStatusAcceptRej(VenderCallStatusModel model);
    }
}
