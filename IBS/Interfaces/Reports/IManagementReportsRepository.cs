using IBS.Models.Reports;

namespace IBS.Interfaces.Reports
{
    public interface IManagementReportsRepository
    {
        public IEPerformanceModel GetIEPerformanceData(DateTime FromDate, DateTime ToDate, string Region);

        public ClusterPerformanceModel GetClusterPerformanceData(DateTime FromDate, DateTime ToDate, string Region);
    }
}
