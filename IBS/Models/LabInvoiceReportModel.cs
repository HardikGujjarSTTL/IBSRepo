namespace IBS.Models
{
    public class LabInvoiceReportModel
    {
        public string BPO_NAME { get; set; }
        public string recipient_gstin_no { get; set; }
        public string St_cd { get; set; }
        public string invoice_no { get; set; }
        public string invoice_dt { get; set; }
        public string Total_AMT { get; set; }
        public string INV_TYPE { get; set; }
        public string HSN_CD { get; set; }
        public string INV_amount { get; set; }
        public string INV_sgst { get; set; }
        public string INV_cgst { get; set; }
        public string INV_igst { get; set; }
        public string INVOICE_TYPE { get; set; }
        public string INC_TYPE { get; set; }
        public string Total_GST { get; set; }
        public string IRN_NO { get; set; }
        public string BILL_FINALIZE { get; set; }
        public string BILL_SENT { get; set; }
    }

    public class labInvoicelst
    {
        public string InvoiceBillNo { get; set; }
        public string InvoiceNo { get; set; }
        public string BillNO { get; set; }
        public string CLIENT_TYPE { get; set; }
        public string CLIENT_NAME { get; set; }
        public string BPO_NAME { get; set; }
        public string LOA { get; set; }
        public string FileSize { get; set; }
        public string FromDate { get; set; }
        public virtual string base64Logo { get; set; }
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
        public DateTime? ack_dt { get; set; }
        public string credit_doc_id { get; set; }
        public string po_no { get; set; }
        public string po_dt { get; set; }
        public string Vend_Name { get; set; }
        public string Vend_Add1 { get; set; }
        public string Vendor_City { get; set; }
        public string Consignee { get; set; }
        public string Consignee_Add1 { get; set; }
        public string Consignee_Add2 { get; set; }
        public string Consignee_City { get; set; }
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
        public string irfc_bpo_name { get; set; }
        public string irfc_bpo_rly { get; set; }
        public string irfc_bpo_add { get; set; }
        public string irfc_bpo_city { get; set; }
        public string CaseNo { get; set; }
        public DateTime? InvoiceDt { get; set; }
        public List<labInvoicelst> lstlabInvoicelst { get; set; }
    }
}
