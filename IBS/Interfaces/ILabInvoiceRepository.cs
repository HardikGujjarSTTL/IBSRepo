using IBS.Models;

namespace IBS.Interfaces
{
    public interface ILabInvoiceRepository
    {
        public labInvoicelst GetLabInvoice(string FromDate, string ToDate, string Region);

        public labInvoicelst UpdatePDFDetails(labInvoicelst model,string PDFNamee, string RelativePath);

        List<ItemsDetail> GetBillItems(string InvoiceNo);
    }
}
