using IBS.Models;

namespace IBS.Interfaces
{
    public interface IVendorClusterRepository 
    {
        public VendorClusterModel FindByID(int VendorCode, string DepartmentCode);

        DTResult<VendorClusterModel>GetVendorClusterList(DTParameters dtParameters);

        public VendorDetailsModel GetVendorDetails(string VendorCodeName);

        public bool IsDuplicate(VendorClusterModel model);

        public int SaveDetails(VendorClusterModel model);

        bool Remove(int VendorCode, int UserID);
    }
}

