using IBS.DataAccess;
using IBS.Models;

namespace IBS.Interfaces
{
    public interface ILabPaymentFormRepository
    {

        DTResult<LabPaymentFormModel> GetLabPayments(DTParameters dtParameters, string Regin);
        List<LabPaymentFormModel> GetPayments(LabPaymentFormModel paymentFormModel);
        DTResult<LabPaymentFormModel> GetPaymentsEdit(DTParameters dtParameters);
        LabPaymentFormModel Edit(string PaymentID);
        bool SavePayment(LabPaymentFormModel LabPaymentFormModel);
        bool UpdatePayment(LabPaymentFormModel LabPaymentFormModel);
        LabPaymentFormModel PrintLabPayment(LabPaymentFormModel paymentFormModel);

    }
}
