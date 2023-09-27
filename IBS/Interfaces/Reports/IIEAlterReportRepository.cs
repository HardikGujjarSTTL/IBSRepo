using IBS.Models.Reports;

namespace IBS.Interfaces.Reports
{
    public interface IIEAlterReportRepository
    {
        public IEAlterMappingReportModel GetIEAlterMappingReport(string Region);
    }
}
