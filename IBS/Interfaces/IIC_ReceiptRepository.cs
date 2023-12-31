﻿using IBS.Models;
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

        List<IC_Unbilled_List_Model> Get_UnBilled_IC(string FromDate, string ToDate, string Region);

        List<ICIssueNotReceiveModel> Get_IC_Issue_Not_Receive(string FromDate, string ToDate, UserSessionModel model);

        List<ICStatusListModel> Get_IC_Status(string FromDate, string ToDate, string IECD, string Region);
    }
}
