using IBS.DataAccess;
using IBS.Models;
using System.Data;

namespace IBS.Interfaces
{
    public interface ILabSamInfoReportRepository
    {

        DTResult<LabSamInfoReportModel> LabSamInfoReport(DTParameters dtParameters,string Regin);
       
    }
}
