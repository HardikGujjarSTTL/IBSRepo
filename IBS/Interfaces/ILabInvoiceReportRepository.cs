using IBS.DataAccess;
using IBS.Models;
using System.Data;

namespace IBS.Interfaces
{
    public interface ILabInvoiceReportRepository
    {

        DTResult<LabInvoiceReportModel> LabInvoiceReport(DTParameters dtParameters,string Regin);
       
    }
}
