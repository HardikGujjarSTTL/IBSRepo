using IBS.DataAccess;
using IBS.Models;
using System.Data;

namespace IBS.Interfaces
{
    public interface ILabPostingReportRepository
    {

        DTResult<LabPostingReport> labPostingReport(DTParameters dtParameters,string Regin);
       
    }
}
