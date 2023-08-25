using IBS.Models;

namespace IBS.Interfaces
{
    public interface IOnlineComplaintsRepository
    {
        string GetItems(string ItemSno, string bkno, string setno, string InspRegionDropdown);
        string SaveComplaints(OnlineComplaints onlineComplaints, IFormFile complaintFile);
    }
}
