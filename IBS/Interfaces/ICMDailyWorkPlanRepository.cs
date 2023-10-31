using IBS.Models;

namespace IBS.Interfaces
{
    public interface ICMDailyWorkPlanRepository
    {
        DTResult<CMDailyWorkPlanModel> GetLoadTable(DTParameters dtParameters, string Region);

        int SaveApproval(CMDailyWorkPlanModel model, string Region);
        int UpdateUrgency(CMDailyWorkPlanModel model, string Region);
    }
}
