using IBS.Models.Reports;

namespace IBS.Interfaces.Reports
{
    public interface IDailyIEWorkPlanReportRepository
    {
        public DailyIECMWorkPlanReportModel GetDailyWorkData(string FromDate, string ToDate, string lstIE, string lstCM, string AllIEs, string ParticularIEs, string AllCM, string ParticularCMs, string ReportType, string IEWise, string CMWise,string Region, string SortedIE, string visitdate);
    }
}
