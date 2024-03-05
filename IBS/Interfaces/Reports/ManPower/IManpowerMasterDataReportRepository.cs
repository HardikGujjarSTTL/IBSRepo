using IBS.Models;

namespace IBS.Interfaces.Reports.ManPower
{
    public interface IManpowerMasterDataReportRepository
    {
        DTResult<ManpowerModel> GetManpowerMasterReportData(DTParameters dtParameters);
    }
}
