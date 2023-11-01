using IBSAPI.Models;

namespace IBSAPI.Interfaces
{
    public interface IInspectionRepository
    {
        List<TodayInspectionModel> GetToDayInspection(int IeCd);
        List<TodayInspectionModel> GetTomorrowInspection(int IeCd);
    }
}
