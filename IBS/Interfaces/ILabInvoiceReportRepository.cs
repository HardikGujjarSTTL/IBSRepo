using IBS.Models;

namespace IBS.Interfaces
{
    public interface ILabInvoiceReportRepository
    {

        DTResult<LabInvoiceReportModel> LabInvoiceReport(DTParameters dtParameters, string Regin);

    }
}
