using IBSAPI.Models;

namespace IBSAPI.Interfaces
{
    public interface IInspectionRepository
    {
        List<TodayInspectionModel> GetToDayInspection(int IeCd);
        List<TomorrowInspectionModel> GetTomorrowInspection(int IeCd);
        List<PendingInspectionModel> GetPendingInspection(int IeCd, string Region, string CurrDate);
        CaseDetailIEModel GetCaseDetailForIE(string Case_No, string CallRecvDt, int CallSNo, int IeCd);
    }
}
