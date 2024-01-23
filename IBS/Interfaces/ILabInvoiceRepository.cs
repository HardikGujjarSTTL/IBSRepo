using IBS.Models;

namespace IBS.Interfaces
{
    public interface ILabInvoiceRepository
    {
       public LabInvoiceReportModel GetLabInvoice(string FromDate, string ToDate, string Region);
    }
}
