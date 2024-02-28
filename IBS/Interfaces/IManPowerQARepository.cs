using IBS.Models;

namespace IBS.Interfaces
{
    public interface IManPowerQARepository
    {
        DTResult<ManpowerModel> GetMasterList(DTParameters dtParameters);
        ManpowerModel FindByID(int id);
        int SaveMaster(ManpowerModel model);

        ManpowerDetailModel DetailFindByID(int id);
        int SaveDetails(ManpowerDetailModel model);
        public DTResult<ManpowerDetailModel> GetManpowerDetailList(DTParameters dtParameters, List<ManpowerDetailModel> ClusterModels);
        int DeleteManpower(int ID, int UserID);
        int DeleteManpowerDetail(int DetailID, int ManpowerID);
    }
}
