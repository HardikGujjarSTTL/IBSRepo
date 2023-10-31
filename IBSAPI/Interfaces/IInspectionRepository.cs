using IBSAPI.Models;

namespace IBSAPI.Interfaces
{
    public interface IInspectionRepository
    {
        List<TodayInspectionModel> GetToDayInspection();
    }
}
