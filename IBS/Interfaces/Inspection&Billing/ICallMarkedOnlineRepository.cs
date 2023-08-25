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
    }
}
