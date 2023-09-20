using IBS.Models;

namespace IBS.Interfaces
{
    public interface IDefectCodeReportRepository
    {
        public DefectCodeReport GetDefectCodeWiseData(DateTime FromDate, DateTime ToDate, string Region);
    }
}
