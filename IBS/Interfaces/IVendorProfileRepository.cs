using IBS.DataAccess;
using IBS.Models;

namespace IBS.Interfaces
{
    public interface IVendorProfileRepository
    {
        public VendorModel FindByID(int VendCd);
    }
}
