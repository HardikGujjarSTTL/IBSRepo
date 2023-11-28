using IBS.Models.Reports;

namespace IBS.Interfaces.Reports
{
    public interface IManagementReportsRepository
    {
        public IEPerformanceModel GetIEPerformanceData(DateTime FromDate, DateTime ToDate, string Region, int IeCd);

        public ClusterPerformanceModel GetClusterPerformanceData(DateTime FromDate, DateTime ToDate, string Region);

        public RWBSummaryModel GetRWBSummaryData(string FromYearMonth, string ToYearMonth);

        public RWCOModel GetRWCOData(DateTime FromDate, string Outstanding);

        public ICSubmissionModel GetICSubmissionData(DateTime FromDate, DateTime ToDate, string Region);

        public PendingICAgainstCallsModel GetPendingICAgainstCallsData(DateTime FromDate, DateTime ToDate, string Region);

        public SuperSurpriseDetailsModel GetSuperSurpriseDetailsData(DateTime FromDate, DateTime ToDate, string Region, string ParticularCM, string ParticularSector);

        public SuperSurpriseSummaryModel GetSuperSurpriseSummaryData(DateTime FromDate, DateTime ToDate, string Region);

        public ConsignRejectModel GetConsignRejectData(DateTime FromDate, DateTime ToDate, string Region, string InspRegion, string Status);

        public OutstandingOverRegionModel GetOutstandingOverRegion(DateTime FromDate);

        public ClientWiseRejectionModel GetClientWiseRejection(DateTime FromDate, DateTime ToDate, string ClientType, string BPORailway);

        public NonConformityModel GetNonConformityData(string FromYearMonth, string ToYearMonth, int IeCd);

        public PendingCallsModel GetPendingCallsData();

        public ICIssuedNotReceivedModel GetICIssuedNotReceived(DateTime FromDate, DateTime ToDate, string Region);

        public TentativeInspectionFeeWisePendingCallsModel GetTentativeInspectionFeeWisePendingCalls(DateTime FromDate, DateTime ToDate, string Region, string ParticularCM, string SortedOn);

        public CallRemarkingModel GetCallRemarkingData(DateTime FromDate, DateTime ToDate, string Region, string CallRemarkingDate, string CallsStatus);

        public CallDetailsDashboradModel GetCallDetailsDashborad(string Region);
    }
}
