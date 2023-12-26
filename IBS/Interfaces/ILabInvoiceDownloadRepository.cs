using IBS.Models;
using System.Data;

namespace IBS.Interfaces
{
    public interface ILabInvoiceDownloadRepository
    {

        DTResult<LabInvoiceDownloadModel> GetLapInvoice(DTParameters dtParameters, string Regin);
        string GetSrNo(string InvoiceNo);
        DataSet Getdata(string CaseNo, string RegNo, string InvoiceNo, string TranNo);
        LabInvoiceDownloadModel Getdtreg(string InvoiceNo);

    }
}
