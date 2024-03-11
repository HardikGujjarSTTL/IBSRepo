using IBS.Models;

namespace IBS.Interfaces.Reports
{
    public interface IFinanceReportsRepository
    {
        public FinanceReportModel GetFinanceReport(DateTime? FromDate, DateTime? ToDate, string AccCd, string Region);
    }
}
