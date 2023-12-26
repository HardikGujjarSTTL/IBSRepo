using IBS.Models;

namespace IBS.Interfaces
{
    public interface IBillRemarksRepository
    {

        DTResult<BillRemarksModel> GetBills(DTParameters dtParameters, string OrgType, string Org, IWebHostEnvironment webHostEnvironment);
        bool SaveData(BillRemarksModel BillRemarksModel);

    }
}
