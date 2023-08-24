using IBS.Models;

namespace IBS.Interfaces
{
    public interface IRailwaysDirectoryRepository
    {
        public RailwaysDirectoryModel FindByID(int UomCd);

        public DTResult<RailwaysDirectoryModel> GetRMList(DTParameters dtParameters);

        public bool IsDuplicate(RailwaysDirectoryModel model);

        public int SaveDetails(RailwaysDirectoryModel model);

        public bool Remove(int Id);

        public string IsExistsRailwayCode(int Id);
    }
}