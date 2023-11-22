using IBSAPI.Models;

namespace IBSAPI.Interfaces
{
    public interface IInspectionRepository
    {
        #region IE Methods
        List<TodayInspectionModel> GetToDayInspection(int IeCd);
        List<TomorrowInspectionModel> GetTomorrowInspection(int IeCd);
        List<PendingInspectionModel> GetPendingInspection(int IeCd, string Region, DateTime CurrDate);
        CaseDetailIEModel GetCaseDetailForIE(string Case_No, DateTime CallRecvDt, int CallSNo, int IeCd);
        List<DateWiseRecentInspectionModel> GetDateWiseRecentInspection(int IeCd, DateTime FromDate, DateTime ToDate);
        List<CompleteInspectionModel> GetCompleteInspection(int IeCd);
        #endregion

        #region Vendor Methods
        List<VendorPedingInspectionModel> Get_Vendor_PendingInspection(int Vend_Cd, DateTime FromDate, DateTime ToDate);
        #endregion

        #region CM Methods
        List<RecentInspectionModel> Get_CM_RecentInspection(int CO_CD, DateTime CurrDate);
        #endregion

        #region Client Methods
        List<PendingInspectionModel> Get_Client_PendingInspection(string Rly_CD, string Rly_NonType, DateTime FromDate, DateTime ToDate);
        List<PendingInspectionModel> Get_Client_Region_Wise_PendingInspection(string Rly_CD, string Rly_NonType, string Region, DateTime FromDate, DateTime ToDate);
        #endregion

        List<PhotosModel> GetDocRecordsList(int DocumentCategoryID, string ApplicationID, string WebRootPath);
        int DeleteSingleRecord(DeleteICPhotoRequestModel model);
    }
}
