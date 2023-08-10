using IBS.Models;

namespace IBS.Interfaces.Vendor
{
    public interface IVendorPOMARepository
    {
        DTResult<VendorPOMAModel> GetDataList(DTParameters dtParameters, string UserName);

        DTResult<VendorPOMAModel> FindMatchDetail(string CaseNo, string UserName);

        public VendorPOMAModel FindByID(string CaseNo, string MaNo, string MaDt, string MaStatus, string MaSNo, string UserName);
    }
}
