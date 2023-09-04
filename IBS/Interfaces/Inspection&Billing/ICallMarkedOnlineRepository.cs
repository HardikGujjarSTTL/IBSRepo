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
        bool Call_Rejected(CallMarkedOnlineFilter obj);

        VendorDetailModel Get_Vendor_For_Send_Mail(CallMarkedOnlineFilter obj);
        bool Call_Marked_Online_Save(CallMarkedOnlineModel model, UserSessionModel uModel);

        #region Case History Report
        CaseHistoryModel Get_Vendor_Detail_By_CaseNo(string Case_No, string Region);

        DTResult<CaseHistoryItemModel> Get_Case_History_Item(DTParameters dTParameters, string Region);
        DTResult<CaseHistoryPoIREPSModel> Get_Case_History_PO_IREPS(DTParameters dTParameters);
        DTResult<CaseHistoryPoVendorModel> Get_Case_History_PO_Vendor(DTParameters dTParameters);
        DTResult<CaseHistoryPreviousCallModel> Get_Case_History_Previous_Call(DTParameters dTParameters);
        DTResult<CaseHistoryConsigneeComplaintModel> Get_Case_History_Consignee_Complaints(DTParameters dtParameters);
        DTResult<CaseHistoryRejectionVendorPlaceModel> Get_Case_History_Rejection_Vendor_Place(DTParameters dtParameters, string region);
        #endregion
    }
}
