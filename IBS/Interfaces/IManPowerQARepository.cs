using IBS.Models;

namespace IBS.Interfaces
{
    public interface IManPowerQARepository
    {
        DTResult<ManpowerModel> GetMasterList(DTParameters dtParameters);
        ManpowerModel FindByID(int id);
        int SaveDetails(ManpowerModel model);
    }
}
