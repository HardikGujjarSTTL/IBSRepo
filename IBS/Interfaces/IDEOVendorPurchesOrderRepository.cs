using IBS.Models;

namespace IBS.Interfaces
{
    public interface IDEOVendorPurchesOrderRepository
    {
        DTResult<DEOVendorPurchesOrderModel> GetDataList(DTParameters dtParameters, string GetRegionCode, string RootHostName);
    }
}
