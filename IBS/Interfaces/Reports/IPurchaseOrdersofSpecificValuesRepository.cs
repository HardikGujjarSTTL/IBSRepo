using IBS.Models;
using System.Data;

namespace IBS.Interfaces.Reports
{
    public interface IPurchaseOrdersofSpecificValuesRepository
    {
        List<PurchaseOrdersofSpecificValueModel> GetDataList(string p_wAgency, DateTime p_frmDt, DateTime p_toDt, string p_SelCriteria, string p_wClient, string p_wFrmAmt, string p_wToAmt, string Region);
        List<PurchaseOrdersofSummaryModel> GetSummaryDataList(string p_wAgency, DateTime p_frmDt, DateTime p_toDt, string p_SelCriteria, string p_wFrmAmt, string p_wToAmt, string Region);
        List<InspectionDataModel> GetItemWiseInspectionsList(ItemWiseInspectionsParamModel model);
        DataTable GetItemWiseInspectionsForTenderQueriesList(ItemWiseInspectionsParamModel model);
    }
}
