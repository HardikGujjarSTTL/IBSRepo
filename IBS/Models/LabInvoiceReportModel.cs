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
        public List<labInvoicelst> lstlabInvoicelst { get; set; }
    }

    public class labInvoicelst
    {
        public string InvoiceBillNo { get; set; }
        public string InvoiceNo { get; set; }
        public string BillNO { get; set; }
    }
}
