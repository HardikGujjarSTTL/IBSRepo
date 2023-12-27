using IBS.Models;

namespace IBS.Interfaces
{
    public interface ILabInvoiceRptRepository
    {

        DTResult<LabInvoiceModel> GetLapInvoice(DTParameters dtParameters, string RegNo);

        LabInvoiceModel Getdtreg(string RegNo, string GetRegionCode, string UserId);
        string Save(LabInvoiceModel model);

    }
}
