using IBS.Models;

namespace IBS.Interfaces.IE
{
    public interface IDailyWorkPlanRepository
    {
        DTResult<DailyWorkPlanModel> GetMessageList(DTParameters dtParameters,int GetIeCd);

        int DetailsInsertUpdate(DailyWorkPlanModel model);
    }
}
