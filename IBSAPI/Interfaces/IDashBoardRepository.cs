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
    }
}
