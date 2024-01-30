using IBS.Models;

namespace IBS.Interfaces
{
    public interface ILabInvoiceRepository
    {
        public labInvoicelst GetLabInvoice(string FromDate, string ToDate, string Region);

        string UpdatePDFDetails(string InvoiceNo, string PDFNamee, string RelativePath);

        List<LabItemsDetail> GetBillItems(string InvoiceNo);
    }
}
