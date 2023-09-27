using IBS.Models.Reports;

namespace IBS.Interfaces.Reports
{
    public interface IMonthlyReportsRepository
    {
        AllICStatusModel GetAllICStatus(DateTime FromDate, DateTime ToDate, string IECD, string Region);
        ReInspectionICsModel GetReInspectionICs(DateTime FromDate, DateTime ToDate, string Region);
    }
}
