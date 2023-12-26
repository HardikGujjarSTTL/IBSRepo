using IBS.Models;

namespace IBS.Interfaces
{
    public interface IReturnedBillsRepository
    {

        DTResult<ReturnedBillsModel> GetReturnedBills(DTParameters dtParameters, string OrgType, string Org);

    }
}
