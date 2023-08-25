using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class LabBillFinalisationModel
    {
        public string BPO_NAME { get; set; }
        public string RecipientGSTINNo { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string TotalAmount { get; set; }
        public string InvoiceAmount { get; set; }
        public string InvoiceSGST { get; set; }
        public string InvoiceCGST { get; set; }
        public string InvoiceIGST { get; set; }
    }

}
