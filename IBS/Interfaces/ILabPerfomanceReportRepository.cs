using IBS.Models;

namespace IBS.Interfaces
{
    public interface ILabPerfomanceReportRepository
    {

        DTResult<LabPerfomanceReport> labPerformanceReport(DTParameters dtParameters, string Regin);

    }
}
