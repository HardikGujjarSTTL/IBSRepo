using IBS.Models;

namespace IBS.Interfaces.Reports.ManPower
{
    public interface IManpowerMasterDataReportRepository
    {
        DTResult<ManpowerModel> GetManpowerMasterReportData(DTParameters dtParameters);

        ProjectModel FindByID(int id);

        List<IEAndCallReport> GetIEAndCallReport(string P_FROMDATE, string P_TODATE);
    }
}
