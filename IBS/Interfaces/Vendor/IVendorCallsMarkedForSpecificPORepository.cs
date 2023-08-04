using IBS.Models;

namespace IBS.Interfaces.Vendor
{
    public interface IVendorCallsMarkedForSpecificPORepository
    {
        DTResult<VendorCallsMarkedForSpecificPOModel> GetDataList(DTParameters dtParameters, string UserName);
    }
}
