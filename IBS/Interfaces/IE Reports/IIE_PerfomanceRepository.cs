using IBS.Models;
using System.Data;

namespace IBS.Interfaces.IE_Reports
{
    public interface IIE_PerfomanceRepository
    {
        DTResult<IE_PerformanceModel> Get_IE_Performance(DTParameters dTParameters, IEPerformanceFilter model);
        IEPerformanceSummary Get_IE_Performance_Summary(IEFromToDate obj, IEPerformanceFilter model);
    }
}
