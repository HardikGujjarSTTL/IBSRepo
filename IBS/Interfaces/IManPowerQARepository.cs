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
        DTResult<ManpowerDetailModel> GetDetailList(DTParameters dtParameters);
    }
}
