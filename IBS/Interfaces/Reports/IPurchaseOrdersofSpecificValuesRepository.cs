using IBS.Models;

namespace IBS.Interfaces.Reports
{
    public interface IPurchaseOrdersofSpecificValuesRepository
    {
        List<PurchaseOrdersofSpecificValueModel> GetDataList(string p_wAgency, string p_frmDt, string p_toDt, string p_SelCriteria, string p_wClient, string p_wFrmAmt, string p_wToAmt, string Region);
        List<PurchaseOrdersofSummaryModel> GetSummaryDataList(string p_wAgency, string p_frmDt, string p_toDt, string p_SelCriteria, string p_wFrmAmt, string p_wToAmt, string Region);
    }
}
