using IBS.Models.Reports;

namespace IBS.Interfaces.Reports
{
    public interface INCRCWiseReportRepository
    {
        public NCRReport GetNCRIECOWiseData(string month, string year, string FromDate, string ToDate, string AllCM, string forCM, string All, string Outstanding, string formonth, string forperiod,string Region, string controllingmanager, string reporttype, string iename);
    }
}
