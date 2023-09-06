using IBS.DataAccess;
using IBS.Models;
using System.Data;

namespace IBS.Interfaces
{
    public interface IDownloadBillsRepository
    {

        DTResult<DownloadBillsModel> GetReturnedBills(DTParameters dtParameters, string OrgType ,string Org, IWebHostEnvironment webHostEnvironment);
        
    }
}
