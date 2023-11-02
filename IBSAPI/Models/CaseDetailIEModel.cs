namespace IBSAPI.Models
{
    public class CaseDetailIEModel
    {
        public string Case_No { get; set; }
        public DateTime? Call_Recv_DT { get; set; }
        public int Call_SNo { get; set; }
        public decimal? Qty { get; set; }
        public string Status { get; set; }
        public string Vendor { get; set; }
        public string Vendor_Code { get; set; }
        public string Vendor_City { get; set; }
        public string Vendor_Address { get; set; }
        public string PO_Type { get; set; }
        public string PO_No { get; set; }
        public DateTime? PO_DT { get; set; }
        public string PO_Source { get; set; }
        public string MobileNo { get; set; }
        public string EMail { get; set; }        
    }

    public class PhotosModel
    {
        public string Images { get; set; }
    }
}
