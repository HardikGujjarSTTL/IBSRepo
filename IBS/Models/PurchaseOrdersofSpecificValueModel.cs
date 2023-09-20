namespace IBS.Models
{
    public class PurchaseOrdersofSpecificValueModel
    {
        public string CLIENT { get; set; }
        public string CASE_NO { get; set; }
        public string PO_NO { get; set; }
        public string PO_DT { get; set; }
        public decimal PO_VALUE { get; set; }
        public string VENDOR { get; set; }
        public string CONSIGNEE { get; set; }
        public string ITEM_DESC { get; set; }
    }
    public class PurchaseOrdersofSummaryModel
    {
        public string CLIENT { get; set; }
        public string No_OF_PO { get; set; }
        public decimal PO_VALUE { get; set; }
    }

    public class ItemWiseInspectionsParamModel
    {
        public string OneRegion { get; set; }
        public string Region { get; set; }
        public string ItemDesc1 { get; set; }
        public string ItemDesc2 { get; set; }
        public string ItemDesc3 { get; set; }
        public string ItemDesc4 { get; set; }
        public string ItemDesc5 { get; set; }
        public DateTime frmDt { get; set; }
        public DateTime toDt { get; set; }
        public string Client { get; set; }
        public string RCode { get; set; }
    }

    public class InspectionDataModel
    {
        public string REGION_CODE { get; set; }
        public string ITEM_DESC { get; set; }
        public string CASE_NO { get; set; }
        public string PO_NO { get; set; }
        public string PO_DATE { get; set; }
        public string BILL_NO { get; set; }
        public string BILL_DATE { get; set; }
        public string BK_NO { get; set; }
        public string SET_NO { get; set; }
        public string VENDOR { get; set; }
        public string MANUFACTURER { get; set; }
        public string CONSIGNEE { get; set; }
        public string BPO_RLY { get; set; }
        public string QTY_PASSED { get; set; }
        public string QTY_REJECTED { get; set; }
        public decimal VALUE { get; set; }
        public string IC_NO { get; set; }
        public string IC_DATE { get; set; }
        public string CALL_DATE { get; set; }
        public string CALL_SNO { get; set; }
        public string FIRST_INSP_DATE { get; set; }
        public string LAST_INSP_DATE { get; set; }
        public string NO_OF_INSP { get; set; }
        public string TIME_TO_START { get; set; }
        public string TIME_TO_END { get; set; }
    }
}
