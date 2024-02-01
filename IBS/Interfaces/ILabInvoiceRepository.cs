using IBS.Models;

namespace IBS.Interfaces
{
    public interface ILabInvoiceRepository
    {
        public labInvoicelst GetLabInvoice(string FromDate, string ToDate, string Region);

        public labInvoicelst GetPDFLabInvoice(string FromDate, string ToDate, string Region);

        int UpdatePDFDetails(string InvoiceNo, string PDFNamee, string RelativePath);

        List<LabItemsDetail> GetBillItems(string InvoiceNo);
    }
}
