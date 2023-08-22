using IBS.DataAccess;
using IBS.Models;

namespace IBS.Interfaces
{
    public interface ILabSamplePaymentRptRepository
    {

        DTResult<LabSamplePaymentRptModel> GetPaymentReport(DTParameters dtParameters, string Regin);
    }
}
