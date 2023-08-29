using IBS.DataAccess;
using IBS.Models;
using System.Data;

namespace IBS.Interfaces
{
    public interface ILabInvoiceRptRepository
    {

        DTResult<LabInvoiceModel> GetLapInvoice(DTParameters dtParameters,string RegNo);
       
        LabInvoiceModel Getdtreg(string RegNo);

    }
}
