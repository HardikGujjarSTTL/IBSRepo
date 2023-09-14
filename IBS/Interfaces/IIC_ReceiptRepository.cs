using IBS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IBS.Interfaces
{
    public interface IIC_ReceiptRepository
    {
        DTResult<IC_Receipt_Grid_Model> Get_IC_Receipt([FromBody] DTParameters dtParameters, string region);
        List<SelectListItem> Get_IE_Whome_Issued(string region);
        IC_ReceiptModel Get_Selected_IC_Receipt(string BK_NO, string SET_NO, string REGION);
        int IC_Receipt_InsertUpdate(IC_ReceiptModel model);
        int IC_Receipt_Delete(IC_ReceiptModel model);
        int CheckIC(IC_ReceiptModel model);

        DTResult<ICReportModel> Get_UnBilled_IC([FromBody] DTParameters dtParameters, string Region);

        DTResult<ICIssueNotReceiveModel> Get_IC_Issue_Not_Receive([FromBody] DTParameters dtParameters, UserSessionModel model);
        //DTResult<ICIssueNotReceiveModel> Get_IC_Issue_Not_Receive([FromBody] DTParameters dtParameters, string Region, string UserName, string Ic_Cd);

        DTResult<IC_ReceiptModel> Get_IC_Status([FromBody] DTParameters dtParameters, string Region);
    }
}
