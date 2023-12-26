using IBS.Models;

namespace IBS.Interfaces
{
    public interface IDashboardRepository
    {
        public DashboardModel GetDashBoardCount(string Region, string RoleName);
        public DashboardModel GetIEDDashBoardCount(int IeCd, string RegionCode, string RoleName);
        public DashboardModel GetDashBoardLabCount(int userid, string Regin);

        public DashboardModel GetCMDashBoardCount(int CoCd);
        public DashboardModel GetVendorDashBoardCount(int Vend_Cd, string RegionCode, string RoleName);
        public DashboardModel GetClientDashBoardCount(string OrgnType, string Organisation, string RegionCode, string RoleName);
        DTResult<IE_Per_CM_Model> Get_CM_Wise_IE_Detail(DTParameters dtParameters);
        DTResult<AdminCountListing> GetDataListTotalCallListing(DTParameters dtParameters, string Region);
        DTResult<AdminCountListing> Dashboard_Client_List(DTParameters dtParameters, string Region, string OrgnType, string Organisation);
        DTResult<CMDFOListing> CMDFO_List(DTParameters dtParameters);
        DTResult<DashboardModel> Dashboard_CMGeneral_ViewAll_List(DTParameters dtParameters, string COCD);
        DTResult<AdminViewAllList> Dashboard_CMDFO_ViewAll_List(DTParameters dtParameters);
        DTResult<VenderCallRegisterModel> GetDataCallDeskInfoListing(DTParameters dtParameters, string Region);


        DTResult<IEList> Get_IE_Dashboard_Details_List(DTParameters dtParameters);
        DTResult<PO_MasterModel> GetPOMasterList(DTParameters dtParameters);

        DTResult<DashboardLabData> LoadTableInvoice(DTParameters dtParameters, string Regin, int userid);
        DTResult<DashboardModel> Dashboard_Lab_ViewAll_List(DTParameters dtParameters, string Regin, int userid);
        DTResult<LabSampleInfoModel> LoadTableReportU(DTParameters dtParameters, string Regin);

        public LabSampleInfoModel GetNOOfRegisterCount(string Regin);

        DTResult<VendorDetailListModel> GetDataVendorListing(DTParameters dtParameters, string Vend_Cd);
        DTResult<AdminViewAllList> Dashboard_Admin_ViewAll_List(DTParameters dtParameters, string RegionCode);
        DTResult<VendorViewAllList> Dashboard_Vendor_ViewAll_List(DTParameters dtParameters, string RegionCode, int Vend_Cd);
        DTResult<IEViewAllList> Dashboard_IE_ViewAll_List(DTParameters dtParameters, int IE_CD, string RegionCode);

        public DashboardModel GetLODashBoardCount(string UserName);

        DTResult<LoListingModel> GetLoCallListingDetails(DTParameters dtParameters, string UserName);
        DTResult<CLientViewAllList> Dashboard_Client_ViewAll_List(DTParameters dtParameters, string RegionCode, string OrgnType, string Organisation);
        DTResult<CMDARListing> CMDARListing(DTParameters dtParameters);

        #region CM JI Dashboard
        DashboardModel GetCMJIDDashBoard(int CO_CD);
        DashboardModel GetCMGeneralDashBoard(int CO_CD);
        DashboardModel GetCMDARDashBoard(int CO_CD);
        DTResult<CM_JI_ViewAll_Model> Dashboard_CM_JI_ViewAll_List(DTParameters dtParameters, int CO_CD);
        #endregion
        #region CM DFO Dashboard
        DashboardModel GetCMDFODashBoard(int CO_CD);
        #endregion
    }
}