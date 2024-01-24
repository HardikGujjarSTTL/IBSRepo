using IBS.Models;

namespace IBS.Interfaces
{
    public interface ILabInvoiceRepository
    {
        DTResult<labInvoicelst> GetLabInvoice(DTParameters dtParameters);
    }
}
