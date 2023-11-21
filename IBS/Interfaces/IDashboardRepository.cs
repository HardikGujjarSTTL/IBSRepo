using IBS.Models;

namespace IBS.Interfaces
{
    public interface IDashboardRepository
    {
        public DashboardModel GetIEDDashBoardCount(int IeCd);
        public DashboardModel GetCMDashBoardCount(int CoCd);
        public DashboardModel GetVendorDashBoardCount(int Vend_Cd);
        public DashboardModel GetClientDashBoardCount(string OrgnType,string Organisation);
        DTResult<IE_Per_CM_Model> Get_CM_Wise_IE_Detail(DTParameters dtParameters);
        DTResult<NCIssued_Per_IE> Get_IE_Dashboard_Details_List(DTParameters dtParameters);
    }
}