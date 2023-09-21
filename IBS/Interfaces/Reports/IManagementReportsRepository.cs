using IBS.Models.Reports;

namespace IBS.Interfaces.Reports
{
    public interface IManagementReportsRepository
    {
        public IEPerformanceModel GetIEPerformanceData(DateTime FromDate, DateTime ToDate, string Region);

        public ClusterPerformanceModel GetClusterPerformanceData(DateTime FromDate, DateTime ToDate, string Region);

        public RWBSummaryModel GetRWBSummaryData(string FromYearMonth, string ToYearMonth);

        public RWCOModel GetRWCOData(DateTime FromDate, string Outstanding);

        public ICSubmissionModel GetICSubmissionData(DateTime FromDate, DateTime ToDate, string Region);

        public PendingICAgainstCallsModel GetPendingICAgainstCallsData(DateTime FromDate, DateTime ToDate, string Region);

        public SuperSurpriseDetailsModel GetSuperSurpriseDetailsData(DateTime FromDate, DateTime ToDate, string Region, string ParticularCM, string ParticularSector);

        public SuperSurpriseSummaryModel GetSuperSurpriseSummaryData(DateTime FromDate, DateTime ToDate, string Region);
    }
}
