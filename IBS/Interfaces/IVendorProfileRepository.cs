using IBS.Models;

namespace IBS.Interfaces
{
    public interface IVendorProfileRepository
    {
        public VendorModel FindByID(int VendCd);
        DTResult<VendorlistModel> GetVendorList(DTParameters dtParameters);
        bool Remove(int VEND_CD, int UserID);
        int VendorDetailsInsertUpdate(VendorModel model, bool isSameVendor);
    }
}
