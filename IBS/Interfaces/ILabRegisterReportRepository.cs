using IBS.DataAccess;
using IBS.Models;
using System.Data;

namespace IBS.Interfaces
{
    public interface ILabRegisterReportRepository
    {

        DTResult<LabRegisterReport> labRegisterReport(DTParameters dtParameters,string Regin);
       
        

    }
}
