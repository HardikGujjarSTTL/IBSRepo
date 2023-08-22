using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class LabInvoiceDownloadModel
    {
        public string IRN_NO { get; set; }
        public string SAMPLE_REG_NO { get; set; }
        public string CASE_NO { get; set; }
        public string BPO_NAME { get; set; }
        public string RECIPIENT_GSTIN_NO { get; set; }
        public string INVOICE_NO { get; set; }
        public string INVOICE_DT { get; set; }
        public string TOTAL_AMT { get; set; }
        public string INV_TYPE { get; set; }
        public string HSN_CD { get; set; }
        public string INV_AMOUNT { get; set; }
        public string INV_SGST { get; set; }
        public string INV_CGST { get; set; }
        public string INV_IGST { get; set; }
        public string TRANSACTION_NO { get; set; }
        public string TOTAL_GST { get; set; }
        public string Resign { get; set; }
    }

}
