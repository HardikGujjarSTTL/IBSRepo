using IBS.DataAccess;
using IBS.Models;
using System.Data;

namespace IBS.Interfaces
{
    public interface ILabPerfomanceReportRepository
    {

        DTResult<LabPerfomanceReport> labPerformanceReport(DTParameters dtParameters,string Regin);
       
    }
}
