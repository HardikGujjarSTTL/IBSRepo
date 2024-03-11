namespace IBSAPI.Models
{
    public class CRISModel
    {
        public string bill_no { get; set; } = null!;

        public DateTime? invoicedate { get; set; }

        public string? bpo_cd { get; set; }

        public string? bpo_type { get; set; }

        public string? bpo_rly { get; set; }

        public string? bpo_name { get; set; }

        public string? bpo_orgn { get; set; }

        public string? bpo_add { get; set; }

        public string? bpo_city { get; set; }

        public string? case_no { get; set; }

        public string? region_code { get; set; }

        public string? po_no { get; set; }

        public DateTime? po_dt { get; set; }

        public string? vend_cd { get; set; }

        public string? vend_name { get; set; }

        public string? vend_add1 { get; set; }

        public string? vend_add2 { get; set; }

        public string? vendor_city { get; set; }

        public string consignee_cd { get; set; }

        public string? consignee { get; set; }

        public string? consignee_add1 { get; set; }

        public string? consignee_add2 { get; set; }

        public string? consignee_city { get; set; }

        public string? ie_cd { get; set; }

        public string? ie_co_cd { get; set; }

        public string? ic_no { get; set; }

        public DateTime? ic_dt { get; set; }

        public string? bk_no { get; set; }

        public string? set_no { get; set; }

        public string? call_instalment_no { get; set; }

        public string? material_value { get; set; }

        public string? visits { get; set; }

        public decimal? gsttaxableamt { get; set; }

        public decimal? cgstamt { get; set; }

        public decimal? sgstamt { get; set; }

        public decimal? igstamt { get; set; }

        public decimal? amount { get; set; }

        public string? invoiceno { get; set; }

        public string? rlygstin { get; set; }

        public string? au { get; set; }

        public string? partygstin { get; set; }

        public string? partycode { get; set; }

        public string? partyname { get; set; }

        public string? bank_acc_no { get; set; }

        public string? ifsc_code { get; set; }

        public string? bank_name { get; set; }

        public string? bill_resent_count { get; set; }

        public string? irfc_funded { get; set; }

        public string? item_srno { get; set; }

        public string? itemdesc { get; set; }

        public decimal? qty { get; set; }

        public decimal? rate { get; set; }

        public string? uom_factor { get; set; }

        public decimal? basic_value { get; set; }

        public decimal? value { get; set; }

        public string? INVOICE_PDF { get; set; }

        public string? IC_PDF { get; set;}

        public string? PO_PDF { get;set;}

        public string? BILLDESC { get; set; }

        public string? PARTYSTATE { get; set; }

        public string? REVERSECHARGE { get; set; }

        public string? ISGSTREGISTERED { get; set; }

        public string? GSTTDSDEDUCTION { get; set; }

        public string? COMPOSITETAXABLE { get; set; }

        public string? ITEMCATEGORY { get; set; }

        public string? HSNSAC { get; set; }

        public int? HSNSACCODE { get; set; }

        public string? ITCELIGIBLE { get; set; }

        public string? UNITCODE { get; set; }

        public int? SGSTRATE { get; set;}

        public int? CGSTRATE { get; set; }

        public int? UGSTRATE { get; set; }

        public int? UGSTAMT { get; set; }

        public int? IGSTRATE { get; set; }

        public string? STATESUPPLY { get; set; }

        public string? BILLCOUNT { get;set; }

        public string? IRFC_FUNDED1 { get; set; }

        public string? INVOICE_SUPP_DOCS { get; set; }

        //public string? RecipientGstinNo { get; set; }

        //public decimal? BillAmount { get; set; }

        //public decimal? InspFee { get; set; }

        //public decimal? Cgst { get; set; }

        //public decimal? Sgst { get; set; }

        //public decimal? Igst { get; set; }

        //public string? GstinNo { get; set; }

        //public string? RlyPartyCd { get; set; }

        //public string? BankAccNo { get; set; }
    }

    public class CRISBillListing
    {
        public string bill_no { get; set; } = null!;

        public string? region_code { get; set; }
    }

    public class CRISGetBillListing
    {
        public string bill_no { get; set; } = null!;

        public DateTime? invoicedate { get; set; }

        public string? bpo_cd { get; set; }

        public string? bpo_type { get; set; }

        public string? bpo_rly { get; set; }

        public string? bpo_name { get; set; }

        public string? bpo_orgn { get; set; }

        public string? bpo_add { get; set; }

        public string? bpo_city { get; set; }

        public string? case_no { get; set; }

        public string? region_code { get; set; }

        public string? po_no { get; set; }

        public DateTime? po_dt { get; set; }

        public string? vend_cd { get; set; }

        public string? vend_name { get; set; }

        public string? vend_add1 { get; set; }

        public string? vend_add2 { get; set; }

        public string? vendor_city { get; set; }

        public string consignee_cd { get; set; }

        public string? consignee { get; set; }

        public string? consignee_add1 { get; set; }

        public string? consignee_add2 { get; set; }

        public string? consignee_city { get; set; }

        public string? ie_cd { get; set; }

        public string? ie_co_cd { get; set; }

        public string? ic_no { get; set; }

        public DateTime? ic_dt { get; set; }

        public string? bk_no { get; set; }

        public string? set_no { get; set; }

        public string? call_instalment_no { get; set; }

        public string? material_value { get; set; }

        public string? visits { get; set; }

        public decimal? gsttaxableamt { get; set; }

        public decimal? cgstamt { get; set; }

        public decimal? sgstamt { get; set; }

        public decimal? igstamt { get; set; }

        public decimal? amount { get; set; }

        public string? invoiceno { get; set; }

        public string? rlygstin { get; set; }

        public string? au { get; set; }

        public string? partygstin { get; set; }

        public string? partycode { get; set; }

        public string? partyname { get; set; }

        public string? bank_acc_no { get; set; }

        public string? ifsc_code { get; set; }

        public string? bank_name { get; set; }

        public string? bill_resent_count { get; set; }

        public string? irfc_funded { get; set; }

        public string? item_srno { get; set; }

        public string? itemdesc { get; set; }

        public decimal? qty { get; set; }

        public decimal? rate { get; set; }

        public string? uom_factor { get; set; }

        public decimal? basic_value { get; set; }

        public decimal? value { get; set; }

        public string? INVOICE_PDF { get; set; }

        public string? IC_PDF { get; set; }

        public string? PO_PDF { get; set; }

        public string? BILLDESC { get; set; }

        public string? PARTYSTATE { get; set; }

        public string? REVERSECHARGE { get; set; }

        public string? ISGSTREGISTERED { get; set; }

        public string? GSTTDSDEDUCTION { get; set; }

        public string? COMPOSITETAXABLE { get; set; }

        public string? ITEMCATEGORY { get; set; }

        public string? HSNSAC { get; set; }

        public int? HSNSACCODE { get; set; }

        public string? ITCELIGIBLE { get; set; }

        public string UNITCODE { get; set; }

        public int? SGSTRATE { get; set; }

        public int? CGSTRATE { get; set; }

        public int? UGSTRATE { get; set; }

        public int? UGSTAMT { get; set; }

        public int? IGSTRATE { get; set; }

        public string STATESUPPLY { get; set; }

        public string BILLCOUNT { get; set; }

        public string? IRFC_FUNDED1 { get; set; }

        public string? INVOICE_SUPP_DOCS { get; set; }
    }
}
