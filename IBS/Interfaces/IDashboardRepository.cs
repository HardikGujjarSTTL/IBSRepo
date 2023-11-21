using IBS.Models;

namespace IBS.Interfaces
{
    public interface IDashboardRepository
    {
        public DashboardModel GetIEDDashBoardCount(int IeCd);
        public DashboardModel GetVendorDashBoardCount(int Vend_Cd);
        public DashboardModel GetClientDashBoardCount(string OrgnType,string Organisation);

        DTResult<VenderCallRegisterModel> GetDataListTotalCallListing(DTParameters dtParameters, string Region);

        DTResult<VenderCallRegisterModel> GetDataCallDeskInfoListing(DTParameters dtParameters, string Region);

        
    }
}