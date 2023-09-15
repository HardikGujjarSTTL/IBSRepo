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
}
