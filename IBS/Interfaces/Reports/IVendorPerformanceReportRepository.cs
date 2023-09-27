using IBS.Models.Reports;

namespace IBS.Interfaces.Reports
{
    public interface IVendorPerformanceReportRepository
    {
        public VendorPerformanceReportModel GetVendorperformanceReport(string FromDate, string ToDate, string formonth, string forperiod, string month, string year, string vendcd, string Region);
    }
}
