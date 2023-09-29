using IBS.Models;
using IBS.Models.Reports;

namespace IBS.Interfaces.Reports
{
    public interface IIEICPhotoEnclosedReportRepository
    {
        DTResult<IEICPhotoEnclosedModelReport> GetDataList(DTParameters dtParameters,string Region);
        IEICPhotoEnclosedModelReport GetDataListReport(string CaseNo, string CallRecDT, string CallSno, string BKNO, string SETNO,string Region);
    }
}
