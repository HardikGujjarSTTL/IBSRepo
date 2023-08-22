using IBS.DataAccess;
using IBS.Models;

namespace IBS.Interfaces
{
    public interface ILabBillFinalisationRepository
    {

        List<LabBillFinalisationModel> GetBill(string FromDate, string ToDate, string Regin);
        bool UpdateBill(LabBillFinalisationModel LabBillFinalisationModel);
    }
}
