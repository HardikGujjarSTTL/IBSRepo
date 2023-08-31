using IBS.DataAccess;
using IBS.Models;
using System.Data;

namespace IBS.Interfaces
{
    public interface IReturnedBillsRepository
    {

        DTResult<ReturnedBillsModel> GetReturnedBills(DTParameters dtParameters, string OrgType ,string Org);
        
    }
}
