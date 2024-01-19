namespace IBS.Models
{
    public class AllGeneratedBills
    {
        public string BILL_NO { get; set; }
        public DateTime? BILL_DT { get; set; }
        public string REGION_CODE { get; set; }
        public string CLIENT_TYPE { get; set; }
        public string CLIENT_NAME { get; set; }
        public string BPO_NAME { get; set; }
        public string LOA { get; set; }
        public string FileSize { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string bpo_orgn { get; set; }
        public string bpo_add { get; set; }
        public string bpo_city { get; set; }
        public string recipient_gstin_no { get; set; }
        public string irfc_bpo_cd { get; set; }
        public string irfc_funded { get; set; }
        public string qr_code { get; set; }
        public string irn_no { get; set; }
        public string remarks { get; set; }
        public string ack_no { get; set; }
        public string ack_dt { get; set; }
        public string credit_doc_id { get; set; }
        public string po_no { get; set; }
        public string po_dt { get; set; }
        public string VendName { get; set; }
        public string VendAdd1 { get; set; }
        public string VendorCity { get; set; }
        public string Consignee { get; set; }
        public string ConsigneeAdd1 { get; set; }
        public string ConsigneeCity { get; set; }
        public string IC_NO { get; set; }
        public string IC_DT { get; set; }
        public string bk_no { get; set; }
        public string set_no { get; set; }
        public string call_instalment_no { get; set; }
        public string RailwayChk { get; set; }
        public string invoice_no { get; set; }
        public string material_value { get; set; }
        public decimal insp_fee { get; set; }
        public decimal sgst { get; set; }
        public decimal cgst { get; set; }
        public decimal igst { get; set; }
        public decimal fee_rate { get; set; }
        public decimal BILL_AMOUNT { get; set; }
        public List<ItemsDetail> items { get; set; } = new List<ItemsDetail>();
        public List<AllGeneratedBills> lstBillDetailsForPDF { get; set; } //lstBillDetailsForPDF
    }

    public class ItemsDetail
    {
        public string item_desc { get; set; }
        public decimal? qty { get; set; }
        public decimal? rate { get; set; }
        public string uom_s_desc { get; set; }
        public string uom_factor { get; set; }
        public decimal? basic_value { get; set; }
        public string sales_tax { get; set; }
        public string sales_tax_per { get; set; }
        public string discount_type { get; set; }
        public string discount { get; set; }
        public string discount_per { get; set; }
        public string ot_charge_type { get; set; }
        public string ot_charge_per { get; set; }
        public string other_charges { get; set; }
        public int? Item_SrNo { get; set; }
        public string UnitCode { get; set; }
        public decimal? Value { get; set; }
        public string BILL_NO { get; set; }
    }
}
