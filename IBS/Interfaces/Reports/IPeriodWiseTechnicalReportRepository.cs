using IBS.Models.Reports;

namespace IBS.Interfaces.Reports
{
    public interface IPeriodWiseTechnicalReportRepository
    {
        public PeriodWiseTechnicalRefReportModel Getperiodwisetechrefdetails(string FromDate, string ToDate, string Region);
    }
}
