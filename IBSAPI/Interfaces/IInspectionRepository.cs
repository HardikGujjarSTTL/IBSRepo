using IBSAPI.Models;

namespace IBSAPI.Interfaces
{
    public interface IInspectionRepository
    {
        #region IE
        List<TodayInspectionModel> GetToDayInspection(int IeCd);
        List<TomorrowInspectionModel> GetTomorrowInspection(int IeCd);
        List<PendingInspectionModel> GetPendingInspection(int IeCd, string Region, DateTime CurrDate);
        CaseDetailIEModel GetCaseDetailForIE(string Case_No, DateTime CallRecvDt, int CallSNo, int IeCd);
        List<DateWiseRecentInspectionModel> GetDateWiseRecentInspection(int IeCd, DateTime FromDate, DateTime ToDate);
        List<CompleteInspectionModel> GetCompleteInspection(int IeCd);
        #endregion

        #region Vendor
        List<VendorPedingInspectionModel> Get_Vendor_PendingInspection(int Vend_Cd, DateTime FromDate, DateTime ToDate);
        #endregion

        #region CM
        List<RecentInspectionModel> Get_CM_RecentInspection(int CO_CD, DateTime CurrDate);
        #endregion
    }
}
