using IBS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IBS.Interfaces.Inspection_Billing
{
    public interface ICallMarkedOnlineRepository
    {
        DTResult<CallMarkedOnlineModel> Get_Call_Marked_Online(DTParameters dTParameters, string Region);
        List<SelectListItem> Get_Cluster_IE(string Region, string DeptName);

        CallMarkedOnlineModel Get_Call_Marked_Online_Detail(CallMarkedOnlineFilter obj);
        List<CallMaterialValueModel> Get_Call_Material_Value(CallMarkedOnlineFilter obj);
        bool Call_Rejected(CallMarkedOnlineFilter obj, UserSessionModel model);

        #region This method use for  Send Mail for Reject Call functionality. Note: But Now using Linq Query
        //VendorDetailModel Get_Vendor_For_Send_Mail(CallMarkedOnlineFilter obj);
        #endregion

        bool Call_Marked_Online_Save(CallMarkedOnlineModel model, UserSessionModel uModel);

        #region Case History Report
        CaseHistoryModel Get_Vendor_Detail_By_CaseNo(string Case_No, string Region);

        List<CaseHistoryItemModel> Get_Case_History_Item(DTParameters dTParameters, string Case_NO, string Region);
        List<CaseHistoryPoIREPSModel> Get_Case_History_PO_IREPS(DTParameters dTParameters, string PO_NO, string PO_DT);
        List<CaseHistoryPoVendorModel> Get_Case_History_PO_Vendor(DTParameters dTParameters, string CASE_NO);
        List<CaseHistoryPreviousCallModel> Get_Case_History_Previous_Call(DTParameters dTParameters, string CASE_NO);
        List<CaseHistoryConsigneeComplaintModel> Get_Case_History_Consignee_Complaints(DTParameters dtParameters, string VEND_CD);
        List<CaseHistoryRejectionVendorPlaceModel> Get_Case_History_Rejection_Vendor_Place(DTParameters dtParameters, string CASE_NO, string VEND_CD, string region);

        string Send_Vendor_Mail_For_Rejected_Call(CallMarkedOnlineModel obj, string Region);
        string Send_Vendor_Email_For_Incomplete_Call_Details(CallMarkedOnlineModel obj, string Region);
        int Delete_Incomplete_Call(CallMarkedOnlineFilter obj, UserSessionModel model);
        string GetRegionInfo(string Region);

        string Send_Vendor_Email(CallMarkedOnlineModel obj, string Region);
        #endregion

        #region SMS
        Task<string> send_IE_smsAsync(CallMarkedOnlineModel model);
        #endregion
    }
}
