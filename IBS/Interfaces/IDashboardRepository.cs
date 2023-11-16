using IBS.Models;

namespace IBS.Interfaces
{
    public interface IDashboardRepository
    {
        public DashboardModel GetIEDDashBoardCount(int IeCd);
        public DashboardModel GetVendorDashBoardCount(int Vend_Cd);
        public DashboardModel GetClientDashBoardCount(string OrgnType,string Organisation);
    }
}