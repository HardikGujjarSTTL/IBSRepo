using IBS.Models;

namespace IBS.Interfaces.Vendor
{
    public interface IVendorRepository 
    {
        public VendorModel FindByID(int Id);

        public DTResult<VendorlistModel> GetVendorList(DTParameters dtParameters);

        public VendorDetailsModel GetVendorDetails(string VendorCodeName);

        public bool IsDuplicate(VendorClusterModel model);

        public int SaveDetails(VendorClusterModel model);

        public bool Remove(int VendorCode, string DepartmentCode);
    }
}

