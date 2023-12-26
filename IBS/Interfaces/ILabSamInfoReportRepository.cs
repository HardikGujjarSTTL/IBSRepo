using IBS.Models;

namespace IBS.Interfaces
{
    public interface ILabSamInfoReportRepository
    {

        DTResult<LabSamInfoReportModel> LabSamInfoReport(DTParameters dtParameters, string Regin);

    }
}
