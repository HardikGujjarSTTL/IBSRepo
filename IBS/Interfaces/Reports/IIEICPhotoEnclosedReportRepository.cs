using IBS.Models;
using IBS.Models.Reports;

namespace IBS.Interfaces.Reports
{
    public interface IIEICPhotoEnclosedReportRepository
    {
        DTResult<IEICPhotoEnclosedModelReport> GetDataList(DTParameters dtParameters,string Region);
    }
}
