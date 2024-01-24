using IBS.Models;

namespace IBS.Interfaces
{
    public interface ILabInvoiceRepository
    {
        public labInvoicelst GetLabInvoice(string FromDate, string ToDate, string Region);
    }
}
