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
        public int No_OF_PO { get; set; }
        public decimal PO_VALUE { get; set; }
    }
}
