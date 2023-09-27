using IBS.Models.Reports;

namespace IBS.Interfaces.Reports
{
    public interface IPeriodWiseChecksheetReportRepository
    {
        public PeriodWiseChecksheetReportModel Getperiodwisechecksheetdetails(string FromDate, string ToDate,string Region);
    }
}
