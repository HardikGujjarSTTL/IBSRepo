using IBS.Models;

namespace IBS.Interfaces.IE
{
    public interface IDailyWorkPlanRepository
    {
        public DailyWorkPlanModel FindByDetails(DailyWorkPlanModel model, string Region);

        DTResult<DailyWorkPlanModel> GetLoadTable(DTParameters dtParameters,string Region, int GetIeCd);

        DTResult<DailyWorkPlanModel> GetLoadTableCurrentDay(DTParameters dtParameters, string Region, int GetIeCd);

        int DetailsInsertUpdate(DailyWorkPlanModel model, string Region, int GetIeCd);

        int DetailsDelete(DailyWorkPlanModel model, string Region, int GetIeCd);

        //string SaveDetails(InspectionCertModel model, string Region);
    }
}
