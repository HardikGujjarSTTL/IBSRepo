using IBS.Models;

namespace IBS.Interfaces
{
    public interface ILabPostingReportRepository
    {

        DTResult<LabPostingReport> labPostingReport(DTParameters dtParameters, string Regin);

    }
}
