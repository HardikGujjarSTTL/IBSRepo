using IBS.Models;

namespace IBS.Interfaces.Vendor
{
    public interface IVendorRepository
    {
        public VendorMasterModel FindByID(int Id);

        public DTResult<VendorlistModel> GetVendorList(DTParameters dtParameters);

        public int SaveDetails(VendorMasterModel model);

        public bool Remove(int id);
    }
}

