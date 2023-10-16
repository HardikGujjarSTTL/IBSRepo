using IBS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IBS.Interfaces
{
    public interface IIC_RPT_IntermediateRepository
    {
        IC_RPT_IntermediateModel GetDetails(string Case_No, string Call_Recv_Dt, string Call_SNo, string ITEM_SRNO_PO, string CONSIGNEE_CD);
        IC_RPT_IntermediateModel AcceptedFun(string Case_No, string Call_Recv_Dt, string Call_SNo, string CONSIGNEE_CD);
        string GetVisitsChanges(string Case_No, string Call_Recv_Dt, string Call_SNo, string VisitDate);

        IC_RPT_IntermediateModel FillItems(string Case_No, string Call_Recv_Dt, string Call_SNo, string CONSIGNEE_CD);

        DTResult<PO_Amendments> GetPOAmendment(DTParameters dtParameters);
        int SetAccepted(string Case_No, string Call_Recv_Dt, string Call_SNo, string CONSIGNEE_CD);

        List<SelectListItem> GetItems(string Case_No, string Call_Recv_Dt, string Call_SNo, string CONSIGNEE_CD);

        bool SaveDetail(IC_RPT_IntermediateModel model, UserSessionModel user);
        int SaveAmendment(string CaseNo, string PO_NO, PO_Amendments model, List<PO_Amendments> lst);

        string RefreshDetail(IC_RPT_IntermediateModel model, UserSessionModel user);
    }
}
