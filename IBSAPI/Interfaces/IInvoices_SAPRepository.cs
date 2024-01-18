using IBSAPI.Models;

namespace IBSAPI.Interfaces
{
    public interface IInvoices_SAPRepository
    {
        List<Invoices_SAPModel> FindInvoiceList(DateTime frmdt, DateTime todt);
    }
}
