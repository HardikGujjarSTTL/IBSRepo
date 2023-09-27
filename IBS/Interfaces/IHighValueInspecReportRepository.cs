using IBS.Models;
using IBS.Models.Reports;

namespace IBS.Interfaces
{
    public interface IHighValueInspecReportRepository
    {
        public HighValueInspReport GetHighValueInspdata(string month,string year,string valinsp,string FromDate,string ToDate,string ICDate,string BillDate,string formonth,string forperiod, string Region);
    }
}
