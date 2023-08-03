using IBS.Models;

namespace IBS.Interfaces
{
    public interface IClusterMaster
    {
        public ClusterMasterModel FindByID(int ClusterCode);
        DTResult<ClusterMasterModel>GetClusterMasterList(DTParameters dtParameters);
        bool Remove(int ClusterCode, int UserID);
        int ClusterMasterDetailsInsertUpdate(ClusterMasterModel model);
    }
}