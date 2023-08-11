using IBS.Models;

namespace IBS.Interfaces.Vendor
{
    public interface IVendorPOMARepository
    {
        DTResult<VendorPOMAModel> GetDataList(DTParameters dtParameters, string UserName);

        DTResult<VendorPOMAModel> FindMatchDetail(string CaseNo, string UserName);

        public VendorPOMAModel FindByID(string CaseNo, string MaNo, string MaDt, string MaStatus, string MaSNo, string UserName);

        public VendorPOMAModel FindManageByID(string CaseNo, int UserName);

        int DetailsSave(VendorPOMAModel model, string CaseNo, string MaNo, string MaDt, string UserName);

        DTResult<VendorPOMAModel> FindMatchDetailModify(string CaseNo, string MaNo, string MaDt, string UserName);

        public VendorPOMAModel FindManageModifyByID(string CaseNo, string MaNo, string MaDt, string MaStatus, byte MaSNo, int UserName);

        int DetailsUpdate(VendorPOMAModel model, string UserName);

        int GetDocument(VendorPOMAModel model);

        DTResult<VendorPOMAModel> GetSubDataList(DTParameters dtParameters, string UserName);

        public VendorPOMAModel FindByReportID(string CaseNo, string UserName);

        DTResult<PODetailsModel> GetDataListPODetails(DTParameters dtParameters, string UserName);

        DTResult<POIREPSModel> GetDataListIREPS(DTParameters dtParameters, string UserName);

        DTResult<VendorPOMAModel> GetDataListPOVENDOR(DTParameters dtParameters, string UserName);

        DTResult<PCallDetailsModel> GetDataListPCallDetails(DTParameters dtParameters, string UserName);

        DTResult<CComplaintsModel> GetDataListCComplaints(DTParameters dtParameters, string UserName);

        DTResult<RVendorPlaceModel> GetDataListRVendorPlace(DTParameters dtParameters, string UserName);
    }
}
