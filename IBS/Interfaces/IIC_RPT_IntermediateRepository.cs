using IBS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IBS.Interfaces
{
    public interface IIC_RPT_IntermediateRepository
    {
        IC_RPT_IntermediateModel GetDetails(string Case_No, string Call_Recv_Dt, string Call_SNo, string ITEM_SRNO_PO, string CONSIGNEE_CD);
    }
}
