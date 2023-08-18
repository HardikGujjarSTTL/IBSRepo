using IBS.Models;

namespace IBS.Interfaces
{
    public interface IVendorCluster 
    {
        public VendorClusterModel FindByID(int VendorCode);
        DTResult<VendorClusterModel>GetVendorClusterList(DTParameters dtParameters);
        bool Remove(int VendorCode, int UserID);
        int VendorClusterDetailsInsertUpdate(VendorClusterModel model);
    }
}

