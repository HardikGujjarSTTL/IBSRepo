using IBS.Models.Reports;

namespace IBS.Interfaces.Reports
{
    public interface IRemitanceReportsRepository
    {
        public RemitanceModel GetRemitanceReport(DateTime FromDate, DateTime ToDate, string AccCode, string Region, string RReport, string BPOName, string ClientType, string ClientName);
    }
}
