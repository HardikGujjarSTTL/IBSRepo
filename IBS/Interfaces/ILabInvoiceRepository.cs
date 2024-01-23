using IBS.Models;

namespace IBS.Interfaces
{
    public interface ILabInvoiceRepository
    {
        DTResult<LabInvoiceReportModel> GetLabInvoice(DTParameters dtParameters);
    }
}
