using IBS.Models;

namespace IBS.Interfaces
{
    public interface ILabRegisterReportRepository
    {

        DTResult<LabRegisterReport> labRegisterReport(DTParameters dtParameters, string Regin);



    }
}
