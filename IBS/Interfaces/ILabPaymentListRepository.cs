using IBS.DataAccess;
using IBS.Models;

namespace IBS.Interfaces
{
    public interface ILabPaymentListRepository
    {

        DTResult<LabPaymentListModel> GetLapPaymentList(DTParameters dtParameters, string Regin);
        LabPaymentListModel LoadPayment(string CaseNo, string CallSno, string CallRecvDt, string Regin);
        bool SaveData(LabPaymentListModel LabPaymentListModel);
    }
}
