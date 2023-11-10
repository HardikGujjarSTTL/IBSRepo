using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class LabInvoiceModel
    {
        public string CallDt { get; set; }
        public string CaseNo { get; set; }
        public string CallSNO { get; set; }
        public string SNO { get; set; }
        public string RegNo { get; set; }

        public string Vendor { get; set;}
        public string VendorName { get; set;}
        public string ManufacturerCD { get; set;}
        public string ManufacturerNM { get; set; }
        public string BPOCD { get; set; }
        public string BPOTYPE { get; set; }
        public string BPONM { get; set; }
        public string BPONMtext { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDt { get; set; }
        public string Item { get; set; }
        public string Quantity { get; set; }
        
        public string TestingCharges { get; set; }
        public string CGST { get; set; }
        public string SGST { get; set; }
        public string IGST { get; set; }
        public string GSTINNO { get; set; }
        public string State { get; set; }
        public string UserId { get; set; }
        public string Region { get; set; }
        public int ErrorCode { get; set; }


    }

}
