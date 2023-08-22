using IBS.Models;

namespace IBS.Interfaces
{
    public interface ICentralRejectionStatusRepository
    {
        public CentralRejectionStatusModel FindByID(int ID);
        DTResult<CentralRejectionStatusModel> GetCentralRejectionStatusList(DTParameters dtParameters);
        bool Remove(int ID, int UserID);
        int InsertUpdateCentralRejectionStatus(CentralRejectionStatusModel model);
    }
}
