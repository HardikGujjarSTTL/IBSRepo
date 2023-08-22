using IBS.DataAccess;
using IBS.Models;

namespace IBS.Interfaces
{
    public interface ILabPaymentFormRepository
    {

        List<LabPaymentFormModel> GetLabPayments(LabPaymentFormModel paymentFormModel);
        List<LabPaymentFormModel> GetPayments(LabPaymentFormModel paymentFormModel);
        bool SavePayment(LabPaymentFormModel LabPaymentFormModel);
        LabPaymentFormModel PrintLabPayment(LabPaymentFormModel paymentFormModel);

    }
}
