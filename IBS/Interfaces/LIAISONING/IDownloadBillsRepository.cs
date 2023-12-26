using IBS.Models;

namespace IBS.Interfaces
{
    public interface IDownloadBillsRepository
    {

        DownloadBillsModel GetReturnedBills(string Month, string Year, string FromDate, string ToDate, string OrgnType, string Org, string RBMonth);

    }
}
