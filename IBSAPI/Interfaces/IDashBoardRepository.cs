using IBSAPI.Models;

namespace IBSAPI.Interfaces
{
    public interface IDashBoardRepository
    {
        #region IE DashBoard
        int GetIETotalAssignInspection(int IeCd, string FromDate, string ToDate);
        int GetIECompletedInspection(int IeCd, string FromDate, string ToDate);
        int GetIEPendingInspection(int IeCd, string FromDate, string ToDate);
        #endregion

        #region Vendor DashBoard
        int GetVendorTotalAssignInspection(int Vendor_ID, string FromDate, string ToDate);
        int GetVendorCompletedInspection(int Vendor_ID, string FromDate, string ToDate);
        int GetVendorPendingInspection(int Vendor_ID, string FromDate, string ToDate);
        #endregion

        #region Client DashBoard
        int GetClientTotalInspection(string Rly_CD, string Rly_NoNType, string FromDate, string ToDate);
        int GetClientCompletedInspection(string Rly_CD, string Rly_NoNType, string FromDate, string ToDate);
        int GetClientPendingInspection(string Rly_CD, string Rly_NoNType, string FromDate, string ToDate);
        #endregion

        #region CM DashBoard
        List<IEModel> Get_CM_Wise_IE(int CO_CD);
        int Get_CM_TotalInspection(int CO_CD, int IE_CD, string FromDate, string ToDate);
        int Get_CM_PendingInspection(int CO_CD, int IE_CD, string FromDate, string ToDate);
        int Get_CM_RequestRejectedInspection(int CO_CD, int IE_CD, string FromDate, string ToDate);
        #endregion
    }
}
