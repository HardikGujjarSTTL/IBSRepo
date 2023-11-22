using IBS.Models;

namespace IBS.Interfaces
{
    public interface IDashboardRepository
    {
        public DashboardModel GetDashBoardCount(int UserId);
        public DashboardModel GetIEDDashBoardCount(int IeCd);
        public DashboardModel GetCMDashBoardCount(int CoCd);
        public DashboardModel GetVendorDashBoardCount(int Vend_Cd);
        public DashboardModel GetClientDashBoardCount(string OrgnType,string Organisation);
        DTResult<IE_Per_CM_Model> Get_CM_Wise_IE_Detail(DTParameters dtParameters);
        DTResult<VenderCallRegisterModel> GetDataListTotalCallListing(DTParameters dtParameters, string Region);
        DTResult<VenderCallRegisterModel> GetDataCallDeskInfoListing(DTParameters dtParameters, string Region);

        
        DTResult<DashboardModel> Get_IE_Dashboard_Details_List(DTParameters dtParameters);
        DTResult<PO_MasterModel> GetPOMasterList(DTParameters dtParameters);

        DTResult<LabReportsModel> LoadTableInvoice(DTParameters dtParameters, string Regin);
        DTResult<LabSampleInfoModel> LoadTableReportU(DTParameters dtParameters, string Regin);
    }
}