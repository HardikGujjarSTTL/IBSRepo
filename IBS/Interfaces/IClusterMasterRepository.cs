using IBS.Models;

namespace IBS.Interfaces
{
    public interface IClusterMasterRepository
    {
        public ClusterMasterModel FindByID(int Id);

        DTResult<ClusterMasterModel>GetClusterMasterList(DTParameters dtParameters);

        public int SaveDetails(ClusterMasterModel model);

        public int GetMaxClusterCd();

        public bool Remove(int Id, int UserID);

    }
}