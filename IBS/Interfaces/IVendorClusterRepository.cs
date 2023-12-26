using IBS.Models;

namespace IBS.Interfaces
{
    public interface IVendorClusterRepository
    {
        public VendorClusterModel FindByID(int VendorCode, string DepartmentCode);

        DTResult<VendorClusterModel> GetVendorClusterList(DTParameters dtParameters);

        public VendorDetailsModel GetVendorDetails(string VendorCodeName);

        public bool IsDuplicate(VendorClusterModel model);

        public int SaveDetails(VendorClusterModel model);

        public bool Remove(int VendorCode, string DepartmentCode);
    }
}

