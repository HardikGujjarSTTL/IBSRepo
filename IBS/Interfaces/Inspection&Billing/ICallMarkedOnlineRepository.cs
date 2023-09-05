using IBS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IBS.Interfaces.Inspection_Billing
{
    public interface ICallMarkedOnlineRepository
    {
        DTResult<CallMarkedOnlineModel> Get_Call_Marked_Online(DTParameters dTParameters, string Region);
        List<SelectListItem> Get_Cluster_IE(string Region);

        CallMarkedOnlineModel Get_Call_Marked_Online_Detail(CallMarkedOnlineFilter obj);
        List<CallMaterialValueModel> Get_Call_Material_Value(CallMarkedOnlineFilter obj);
        bool Call_Rejected(CallMarkedOnlineFilter obj, UserSessionModel model);

        #region This method use for  Send Mail for Reject Call functionality. Note: But Now using Linq Query
        //VendorDetailModel Get_Vendor_For_Send_Mail(CallMarkedOnlineFilter obj);
        #endregion

        bool Call_Marked_Online_Save(CallMarkedOnlineModel model, UserSessionModel uModel);

        #region Case History Report
        CaseHistoryModel Get_Vendor_Detail_By_CaseNo(string Case_No, string Region);

        DTResult<CaseHistoryItemModel> Get_Case_History_Item(DTParameters dTParameters, string Region);
        DTResult<CaseHistoryPoIREPSModel> Get_Case_History_PO_IREPS(DTParameters dTParameters);
        DTResult<CaseHistoryPoVendorModel> Get_Case_History_PO_Vendor(DTParameters dTParameters);
        DTResult<CaseHistoryPreviousCallModel> Get_Case_History_Previous_Call(DTParameters dTParameters);
        DTResult<CaseHistoryConsigneeComplaintModel> Get_Case_History_Consignee_Complaints(DTParameters dtParameters);
        DTResult<CaseHistoryRejectionVendorPlaceModel> Get_Case_History_Rejection_Vendor_Place(DTParameters dtParameters, string region);

        string Send_Vendor_Mail_For_Rejected_Call(CallMarkedOnlineModel obj, string Region);
        string Send_Vendor_Email_For_Incomplete_Call_Details(CallMarkedOnlineModel obj, string Region);
        int Delete_Incomplete_Call(CallMarkedOnlineFilter obj, UserSessionModel model);
        string GetRegionInfo(string Region);
        #endregion
    }
}
