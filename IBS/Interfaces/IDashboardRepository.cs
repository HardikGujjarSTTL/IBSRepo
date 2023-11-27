using IBS.Models;

namespace IBS.Interfaces
{
    public interface IDashboardRepository
    {
        public DashboardModel GetDashBoardCount(string Region);
        public DashboardModel GetIEDDashBoardCount(int IeCd,string RegionCode);
        public DashboardModel GetDashBoardLabCount(int userid, string Regin);
        
        public DashboardModel GetCMDashBoardCount(int CoCd);
        public DashboardModel GetVendorDashBoardCount(int Vend_Cd,string RegionCode);
        public DashboardModel GetClientDashBoardCount(string OrgnType,string Organisation,string RegionCode);
        DTResult<IE_Per_CM_Model> Get_CM_Wise_IE_Detail(DTParameters dtParameters);
        DTResult<AdminCountListing> GetDataListTotalCallListing(DTParameters dtParameters, string Region);
        DTResult<VenderCallRegisterModel> GetDataCallDeskInfoListing(DTParameters dtParameters, string Region);


        DTResult<IEList> Get_IE_Dashboard_Details_List(DTParameters dtParameters);
        DTResult<PO_MasterModel> GetPOMasterList(DTParameters dtParameters);

        DTResult<LabReportsModel> LoadTableInvoice(DTParameters dtParameters, string Regin);
        DTResult<LabSampleInfoModel> LoadTableReportU(DTParameters dtParameters, string Regin);

        public LabSampleInfoModel GetNOOfRegisterCount(string Regin);

        DTResult<VendorDetailListModel> GetDataVendorListing(DTParameters dtParameters, string Vend_Cd);
        DTResult<AdminViewAllList> Dashboard_Admin_ViewAll_List(DTParameters dtParameters,string RegionCode);
        DTResult<VendorViewAllList> Dashboard_Vendor_ViewAll_List(DTParameters dtParameters,string RegionCode,int Vend_Cd);
        DTResult<IEViewAllList> Dashboard_IE_ViewAll_List(DTParameters dtParameters,int IE_CD,string RegionCode);
    }
}