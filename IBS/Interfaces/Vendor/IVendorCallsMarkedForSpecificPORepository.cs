using IBS.Models;

namespace IBS.Interfaces.Vendor
{
    public interface IVendorCallsMarkedForSpecificPORepository
    {
        DTResult<VendorCallsMarkedForSpecificPOModel> GetDataList(DTParameters dtParameters, string UserName);

        DTResult<VendorCallsForSpecificPOReportModel> GetDataReportCallList(DTParameters dtParameters, string UserName);

        DTResult<VendorCallsMarkedForSpecificICModel> GetDataReportICList(DTParameters dtParameters, string UserName);

        DTResult<VendorCallsMarkedForSpecificICSubModel> GetDataReportICSubList(DTParameters dtParameters, string UserName);
    }
}
