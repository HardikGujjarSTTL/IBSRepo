using IBS.Models;

namespace IBS.Interfaces
{
    public interface IRailwaysDirectory
    {
        public RailwaysDirectoryModel FindByID(int UomCd);
        DTResult<RailwaysDirectoryModel> GetRMList(DTParameters dtParameters);
        bool Remove(int RlyCd, int UserID);
        int RailwaysDirectoryDetailsInsertUpdate(RailwaysDirectoryModel model);
    }
}